﻿// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Table;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class SelectHeroUIPre : BaseUI
// {
//     Action event_ClickCloseBtn;
//     Action<int> event_ClickConfirmBtn;
//     Action<int> event_ClickOneHeroOption;
//
//     Button confirmBtn;
//     private Button closeBtn;
//
//     private Text currHeroNameText;
//     private Image currHeroAvatarImg;
//     private Text currHeroDesText;
//
//     Transform heroRoot;
//
//     Button clickBtn;
//     public List<HeroCardUIData> heroCardUIDataList;
//     public List<SelectHeroOptionShowObj> heroShowObjList;
//
//     int currSelectHeroGuid;
//
//     protected override void OnInit()
//     {
//         heroRoot = this.transform.Find("scroll/mask/content");
//         confirmBtn = this.transform.Find("confirmBtn").GetComponent<Button>();
//
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//
//         currHeroNameText = this.transform.Find("selectRoleNameText").GetComponent<Text>();
//         currHeroAvatarImg = this.transform.Find("selectRolePic").GetComponent<Image>();
//         currHeroDesText = this.transform.Find("describeText").GetComponent<Text>();
//
//
//         confirmBtn.onClick.AddListener(() =>
//         {
//             event_ClickConfirmBtn?.Invoke(currSelectHeroGuid);
//         });
//
//         closeBtn.onClick.AddListener(() =>
//         {
//             //event_ClickCloseBtn?.Invoke();
//             this.Hide();
//         });
//
//         heroShowObjList = new List<SelectHeroOptionShowObj>();
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//         SelectHeroUIArgs selectHeroUIArgs = (SelectHeroUIArgs)args;
//         this.heroCardUIDataList = selectHeroUIArgs.heroCardUIDataList;
//         this.event_ClickConfirmBtn = selectHeroUIArgs.event_ClickConfirmBtn;
//         this.event_ClickOneHeroOption = selectHeroUIArgs.event_ClickOneHeroOption;
//
//         currSelectHeroGuid = selectHeroUIArgs.currSelectHeroGuid;
//
//         this.RefreshInfo();
//     }
//
//     void RefreshInfo()
//     {
//         //选择英雄项
//         for (int i = 0; i < this.heroCardUIDataList.Count; i++)
//         {
//             var data = this.heroCardUIDataList[i];
//             GameObject go = null;
//             SelectHeroOptionShowObj heroOption = null;
//             if (i < this.heroRoot.childCount)
//             {
//                 go = this.heroRoot.GetChild(i).gameObject;
//             }
//             else
//             {
//                 var prefab = this.heroRoot.GetChild(0).gameObject;
//                 go = GameObject.Instantiate(prefab, this.heroRoot, false);
//             }
//
//             if (i < heroShowObjList.Count)
//             {
//                 heroOption = heroShowObjList[i];
//             }
//             else
//             {
//                 heroOption = new SelectHeroOptionShowObj();
//                 //理论上不会出现加一个还是空的的情况 
//                 heroShowObjList.Add(heroOption);
//
//                 heroOption.Init(go);
//                 heroOption.AddClickListener(event_ClickOneHeroOption);
//             }
//
//             heroOption.Refresh(data);
//             heroOption.Show();
//         }
//
//         for (int i = this.heroRoot.childCount - 1; i >= this.heroCardUIDataList.Count; i--)
//         {
//             var go = this.heroRoot.GetChild(i).gameObject;
//
//             if (i < heroShowObjList.Count)
//             {
//                 heroShowObjList[i].Hide();
//                 heroShowObjList[i].Release();
//                 heroShowObjList.RemoveAt(i);
//             }
//             else
//             {
//                 go.SetActive(false);
//             }
//         }
//
//
//         SelectHero(currSelectHeroGuid);
//     }
//
//     public void SelectHero(int guid)
//     {
//         SelectHeroOptionShowObj currShowObj = null;
//         currSelectHeroGuid = guid;
//         foreach (var showObj in heroShowObjList)
//         {
//             if (showObj.avatar.uiData.guid == guid)
//             {
//                 showObj.SetSelectState(true);
//                 currShowObj = showObj;
//             }
//             else
//             {
//                 showObj.SetSelectState(false);
//             }
//         }
//
//         var heroConfigId = currShowObj.data.configId;
//         var heroTb = TableManager.Instance.GetById<Table.EntityInfo>(heroConfigId);
//
//         currHeroNameText.text = heroTb.Name;
//         ResourceManager.Instance.GetObject<Sprite>(heroTb.AllBodyResId,
//             (sprite) => { currHeroAvatarImg.sprite = sprite; });
//
//         currHeroDesText.text = heroTb.Describe;
//     }
//
//     protected override void OnUnload()
//     {
//         event_ClickConfirmBtn = null;
//         event_ClickCloseBtn = null;
//         this.confirmBtn.onClick.RemoveAllListeners();
//         this.closeBtn.onClick.RemoveAllListeners();
//
//         foreach (var item in heroShowObjList)
//         {
//             item.Release();
//         }
//
//         heroShowObjList = null;
//     }
// }
//
// //-----------------------------------------
//
//
// public class SelectHeroUIArgs : UIArgs
// {
//     public List<HeroCardUIData> heroCardUIDataList;
//     public Action<int> event_ClickConfirmBtn;
//     public Action<int> event_ClickOneHeroOption;
//     public int currSelectHeroGuid;
// }
//
// public class SelectHeroOptionShowObj
// {
//     public HeroAvatar avatar;
//     public GameObject gameObject;
//     public Transform transform;
//
//     public HeroCardUIData data;
//     private Text nameText;
//
//     public void Init(GameObject go)
//     {
//         gameObject = go;
//         transform = gameObject.transform;
//
//         //填充 avatar
//         var avatarRootGo = this.transform.Find("HeroAvatar").gameObject;
//         avatar = new HeroAvatar();
//         avatar.Init(avatarRootGo);
//
//         //自身
//         nameText = this.transform.Find("nameText").GetComponent<Text>();
//     }
//
//     public void AddClickListener(Action<int> eventClickOneHeroOption)
//     {
//         avatar.AddClickListener(eventClickOneHeroOption);
//     }
//
//     public void Refresh(HeroCardUIData data)
//     {
//         this.data = data;
//         avatar.Refresh(data);
//
//         var config = Table.TableManager.Instance.GetById<Table.EntityInfo>(data.configId);
//         nameText.text = config.Name;
//     }
//
//     public void Show()
//     {
//         avatar.Show();
//     }
//
//     public void SetSelectState(bool isShow)
//     {
//         avatar.SetSelectState(isShow);
//     }
//
//     public void Hide()
//     {
//         avatar.Hide();
//     }
//
//     public void Release()
//     {
//         avatar.Release();
//     }
// }