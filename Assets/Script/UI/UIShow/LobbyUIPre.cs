// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class LobbyUIArg : UIArgs
// {
//     public string playerName;
//     public int playerLevel;
//     public string avatarURL;
// }
//
// public class LobbyUIPre : BaseUI
// {
//     public Action onClickCloseBtn;
//     public Action onClickHeroListBtn;
//     public Action onClickMainTaskBtn;
//     public Action onClickTeamBtn;
//
//     Button closeBtn;
//     Button heroListBtn;
//     Button mainTaskBtn;
//     Button teamBtn;
//     Text playerNameText;
//     Text playerLevelText;
//     private Image playerIconImg;
//
//     protected override void OnInit()
//     {
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//         heroListBtn = this.transform.Find("heroListBtn").GetComponent<Button>();
//         mainTaskBtn = this.transform.Find("mainTaskBtn").GetComponent<Button>();
//         teamBtn = this.transform.Find("teamBtn").GetComponent<Button>();
//         playerNameText = this.transform.Find("heroInfo/playerNameRoot/name").GetComponent<Text>();
//         playerLevelText = this.transform.Find("heroInfo/level").GetComponent<Text>();
//         playerIconImg = this.transform.Find("heroInfo/avatarBg/avatar").GetComponent<Image>();
//
//         closeBtn.onClick.AddListener(() => { onClickCloseBtn?.Invoke(); });
//
//         heroListBtn.onClick.AddListener(() => { onClickHeroListBtn?.Invoke(); });
//
//         mainTaskBtn.onClick.AddListener(() => { onClickMainTaskBtn?.Invoke(); });
//
//         teamBtn.onClick.AddListener(() => { onClickTeamBtn?.Invoke(); });
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//         var lobbyArg = (LobbyUIArg)args;
//         var playerNameStr = lobbyArg.playerName;
//         this.playerNameText.text = playerNameStr;
//         this.playerLevelText.text = "" + lobbyArg.playerLevel;
//         var iconResId = int.Parse(lobbyArg.avatarURL);
//         ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) =>
//         {
//             playerIconImg.sprite = sprite;
//         });
//     }
//
//     protected override void OnUnload()
//     {
//         onClickCloseBtn = null;
//         onClickHeroListBtn = null;
//         onClickMainTaskBtn = null;
//         onClickTeamBtn = null;
//     }
// }