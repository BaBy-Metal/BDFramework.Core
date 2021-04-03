using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BDFramework.Core.Debugger;
using BDFramework.Mgr;
using BDFramework.ResourceMgr;
using BDFramework.Sql;
using BDFramework.Core.Tools;
using BDFramework.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace BDFramework.Editor.EditorLife
{
    /// <summary>
    /// 这个类用以编辑器环境下辅助BD生命周期的开发
    /// </summary>
    [InitializeOnLoad]
    static public class BDFrameEditorLife
    {
        static BDFrameEditorLife()
        {
            EditorApplication.playModeStateChanged += OnPlayExit;
        }

        /// <summary>
        /// 代码编译完成后
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts(0)]
        static void OnScriptReload()
        {
            OnCodeBuildComplete();
            //编译dll
            if (BDFrameEditorConfigHelper.EditorConfig.BuildAssetConfig.IsAutoBuildDll)
            {
                ScriptBuildTools.BuildDll(Application.streamingAssetsPath, Application.platform,
                    ScriptBuildTools.BuildMode.Release, false);
            }

            
            //EditorWindow_ScriptBuildDll.RoslynBuild(Application.streamingAssetsPath, RuntimePlatform.Android, ScriptBuildTools.BuildMode.Release);
        }
        
        

        /// <summary>
        /// 退出播放模式
        /// </summary>
        /// <param name="state"></param>
        static private void OnPlayExit(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                InitEditorFrame();
            }
        }

        /// <summary>
        /// Editor代码刷新后执行
        /// </summary>
        static public void OnCodeBuildComplete()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            InitEditorFrame();
        }

        static public void InitEditorFrame()
        {
            //BD生命周期启动
            BDApplication.Init();
            BDFrameEditorConfigHelper.Init();
            //编辑器下加载初始化
            BResources.Load(AssetLoadPath.Editor);
        }


        /// <summary>
        /// 游戏逻辑的Assembly
        /// </summary>
        static public Type[] Types { get; set; }

        /// <summary>
        /// 外部注册主工程的Assembly
        /// </summary>
        /// <param name="gameLogicAssembly"></param>
        /// <param name="gameEditorAssembly"></param>
        static public void RegisterMainProjectAssembly(Assembly gameLogicAssembly, Assembly gameEditorAssembly)
        {
            //编辑器所有类
            List<Type> typeList = new List<Type>();
            typeList.AddRange(gameLogicAssembly.GetTypes());
            typeList.AddRange(gameEditorAssembly.GetTypes());
            //BD编辑器下所有的类
            typeList.AddRange(typeof(BDFrameEditorLife).Assembly.GetTypes());
            //BDRuntime下所有类
            typeList.AddRange(typeof(BDLauncher).Assembly.GetTypes());
            Types = typeList.ToArray();
            //Editor的管理器初始化
            RegisterEditorMgrbase(Types);
            BDFrameEditorBehaviorHelper.Init();
            //调试器启动
            DebuggerServerProcessManager.Inst.Start();
        }

        /// <summary>
        /// 注册所有管理器，让管理器在编辑器下生效
        /// </summary>
        static private void RegisterEditorMgrbase(Type[] types)
        {
            //
            List<IMgr> mgrs = new List<IMgr>();
            foreach (var t in types)
            {
                if (t != null && t.BaseType != null && t.BaseType.FullName != null &&
                    t.BaseType.FullName.Contains(".ManagerBase`2"))
                {
                    var i = t.BaseType.GetProperty("Inst").GetValue(null, null) as IMgr;
                    mgrs.Add(i);
                }
            }

            foreach (var type in types)
            {
                var baseAttributes = type.GetCustomAttributes();
                if (baseAttributes.Count() == 0)
                {
                    continue;
                }

                //1.类型注册到管理器
                var attributes = baseAttributes.Where((attr) => attr is ManagerAtrribute);
                if (attributes.Count() > 0)
                {
                    foreach (var mgr in mgrs)
                    {
                        mgr.CheckType(type, attributes);
                    }
                }
            }
        }
    }
}