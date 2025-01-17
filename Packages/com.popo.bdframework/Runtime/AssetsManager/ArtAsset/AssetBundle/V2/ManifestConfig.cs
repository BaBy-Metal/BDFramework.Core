﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace BDFramework.ResourceMgr.V2
{

    /// <summary>
    /// 这里是解决litjson初始化 无参构造不传递对比参数
    /// </summary>
    public class AssetDictionary : Dictionary<string, ManifestItem>
    {
        public AssetDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
            
        }

    }
    /// <summary>
    /// 配置文件
    /// </summary>
    public class ManifestConfig
    {
        /// <summary>
        /// 是否为hash命名
        /// </summary>
        public bool IsHashName { get; set; } = false;

        /// <summary>
        /// 加载路径名-资源数据
        /// </summary>
        public AssetDictionary ManifestMap { get; private set; } = new AssetDictionary();


        /// <summary>
        /// 获取单个依赖
        /// </summary>
        /// <param name="menifestName"></param>
        /// <returns>这个list外部不要修改</returns>
        public List<string> GetDependenciesByName(string name)
        {
            ManifestItem item = null;
            if (this.ManifestMap.TryGetValue(name, out item))
            {
                var list = new List<string>(item.Depend);
                return list;
            }

            BDebug.LogError("【config】不存在资源:" + name);
            return null;
        }


        /// <summary>
        /// 获取单个menifestItem
        /// </summary>
        /// <param name="manifestName"></param>
        /// <returns></returns>
        public ManifestItem GetManifest(string manifestName)
        {
            if (!string.IsNullOrEmpty(manifestName))
            {
                ManifestItem item = null;
                if (this.ManifestMap.TryGetValue(manifestName, out item))
                {
                    return item;
                }
            }
            return null;
        }
    }
}