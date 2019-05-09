//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:09:27
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILobbyStartSystem: StartSystem<FUILobby.FUILobby>
    {
        public override void Start(FUILobby.FUILobby self)
        {
            self.normalPVPBtn.self.onClick.Add(()=>this.EnterMapAsync().Coroutine());
        }
        
        private async ETVoid EnterMapAsync()
        {
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.ShowLoadingUI);
            await MapHelper.EnterMapAsync();
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.CloseLoadingUI);
        }
    }
}