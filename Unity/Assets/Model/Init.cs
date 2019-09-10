﻿using System;
using System.Threading;
using NETCoreTest.Framework;
using UnityEngine;

namespace ETModel
{
    public class Init: MonoBehaviour
    {
        public bool isEditorMode = false;

        private FixedUpdate fixedUpdate;

        private void Start()
        {
            Define.ResModeIsEditor = this.isEditorMode;
            this.StartAsync().Coroutine();
        }

        private async ETVoid StartAsync()
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
                DontDestroyOnLoad(gameObject);
                Game.EventSystem.Add(DLLType.Model, typeof (Init).Assembly);
                
                fixedUpdate = new FixedUpdate(){UpdateCallback = ()=>Game.EventSystem.FixedUpdate()};

                Game.Scene.AddComponent<TimerComponent>();

                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<PlayerComponent>();
                Game.Scene.AddComponent<UnitComponent>();

                Game.Scene.AddComponent<FUIPackageComponent>();
                Game.Scene.AddComponent<FUIComponent>();
                Game.Scene.AddComponent<FUIInitComponent>();

                //用户输入管理组件
                Game.Scene.AddComponent<UserInputComponent>();

                Game.Scene.AddComponent<GameObjectPool<Unit>>();

                // 下载ab包 
                await BundleHelper.DownloadBundle();

                Game.Hotfix.LoadHotfixAssembly();

                // 加载配置
                Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();

                Game.Scene.AddComponent<HeroBaseDataRepositoryComponent>();
                
                Game.Scene.AddComponent<B2S_DebuggerComponent>();

                Game.Hotfix.GotoHotfix();

                Game.Scene.AddComponent<NP_SyncComponent>();
                Game.Scene.AddComponent<NP_RuntimeTreeRepository>();

                Game.Scene.AddComponent<TestDeSeriComponent>();
                //Game.Scene.AddComponent<TestHelloWorldComponent>();
                // Game.Scene.AddComponent<TestDeSeriComponent>();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }


        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();

            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void FixedUpdate()
        {
            this.fixedUpdate.Tick();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }
    }
}