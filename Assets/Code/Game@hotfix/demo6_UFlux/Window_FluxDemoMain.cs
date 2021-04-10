﻿using BDFramework.UFlux;
using BDFramework.UFlux.View.Props;
using BDFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.demo6_UFlux
{
    [UI((int) WinEnum.Win_Demo6, "Windows/UFlux/Window_FluxMain")]
    public class Window_FluxDemoMain : AWindow
    {
        public Window_FluxDemoMain(string path) : base(path)
        {
        }

        [ButtonOnclick("Btns/btn_01")]
        private void btn_01()
        {
            Debug.Log("Flux demo1-0,点击这里追踪代码!");

            //可以F12查看Test01Component的代码
            var com = new Component_Test001();
            //这里是同步加载 
            if (com.IsLoad)
            {
                com.Transform.SetParent(this.Transform, false);
                com.Open();
            }
            else
            {
                com.AsyncLoad(() => { BDebug.Log("加载完成"); });
            }
        }
        [ButtonOnclick("Btns/btn_01_1")]
        private void btn_01_1()
        {
            Debug.Log("Flux demo1-1,点击这里追踪代码!");

            //可以F12查看Test01Component的代码
            var com = new Component_Test002();
            //这里是同步加载 
            if (com.IsLoad)
            {
                com.Transform.SetParent(this.Transform, false);
                com.Open();
            }
            else
            {
                com.AsyncLoad(() =>
                {
                    //
                    BDebug.Log("加载完成");
                });
            }
        }

        
        [ButtonOnclick("Btns/btn_02")]
        private void btn_02()
        {
            Debug.Log("Flux demo2,点击这里追踪代码!");
            UIManager.Inst.LoadWindow(WinEnum.Win_Demo6_Test002);
            UIManager.Inst.ShowWindow(WinEnum.Win_Demo6_Test002);
        }

        [ButtonOnclick("Btns/btn_02_1")]
        private void btn_02_1()
        {
            Debug.Log("Flux demo3,点击这里追踪代码!");
            UIManager.Inst.LoadWindow(WinEnum.Win_Demo6_Test003);
            UIManager.Inst.ShowWindow(WinEnum.Win_Demo6_Test003);
        }

        [ButtonOnclick("Btns/btn_03")]
        private void btn_03()
        {
        }

        [ButtonOnclick("Btns/btn_04")]
        private void btn_04()
        {
            Debug.Log("Flux demo4,点击这里追踪代码!");
            UIManager.Inst.LoadWindow(WinEnum.Win_Demo6_Test004);
            UIManager.Inst.ShowWindow(WinEnum.Win_Demo6_Test004);
        }

        [ButtonOnclick("Btns/btn_05")]
        private void btn_05()
        {
            Debug.Log("Flux demo5,点击这里追踪代码!");
            UIManager.Inst.LoadWindow(WinEnum.Win_Demo6_Test005);
            UIManager.Inst.ShowWindow(WinEnum.Win_Demo6_Test005);
        }

        [ButtonOnclick("Btns/btn_06")]
        private void btn_06()
        {
            Debug.Log("Flux demo6,点击这里追踪代码!");
            UIManager.Inst.LoadWindow(WinEnum.Win_Demo6_Test006);
            UIManager.Inst.ShowWindow(WinEnum.Win_Demo6_Test006);
        }

        [ButtonOnclick("btn_close")]
        private void btn_close()
        {
            this.Close();
        }
    }
}