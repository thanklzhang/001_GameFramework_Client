// using Battle_Client;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Table;
// using UnityEngine;
// using UnityEngine.UI;
//
// public enum EntityRelationType
// {
//     Self = 0,
//     Friend = 1,
//     Enemy = 2
// }
//
// public class BattleUIPre : BaseUI
// {
//     public Action onCloseBtnClick;
//     public Action onReadyStartBtnClick;
//     public Action onAttrBtnClick;
//
//     Button closeBtn;
//     Button readyStartBtn;
//     Button hasReadyBtn;
//     Button attrBtn;
//
//     public GameObject readyBgGo;
//     public Text stateText;
//
//     public GameObject bossComingRootGo;
//
//     //血条
//     HpUIMgr hpUIMgr;
//
//     //飘字
//     FloatWordMgr floatWordMgr;
//
//     //属性面板
//     BattleAttrUI attrUI;
//
//     //技能显示面板
//     BattleSkillUI skillUI;
//
//     //队友英雄信息面板
//     BattleHeroInfoUIPre _heroInfoUIPre;
//
//     //buff 显示面板
//     BattleBuffUI buffUI;
//
//     //通用描述面板
//     DescribeUI describeUI;
//
//     //关卡信息界面
//     BattleStageInfoUI stageInfoUI;
//
//
//     protected override void OnInit()
//     {
//         closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//         readyBgGo = this.transform.Find("readyBg").gameObject;
//         readyStartBtn = this.transform.Find("readyBg/readyStartBtn").GetComponent<Button>();
//         hasReadyBtn = this.transform.Find("readyBg/hasReadyBtn").GetComponent<Button>();
//         stateText = this.transform.Find("stateText").GetComponent<Text>();
//         attrBtn = this.transform.Find("attrBtn").GetComponent<Button>();
//
//         bossComingRootGo = this.transform.Find("bossComingRoot").gameObject;
//
//         closeBtn.onClick.AddListener(() => { onCloseBtnClick?.Invoke(); });
//         readyStartBtn.onClick.AddListener(() =>
//         {
//             onReadyStartBtnClick?.Invoke();
//             AudioManager.Instance.PlaySound((int)ResIds.audio_click_001);
//         });
//         attrBtn.onClick.AddListener(() => { onAttrBtnClick?.Invoke(); });
//
//         //血条管理
//         this.hpUIMgr = new HpUIMgr();
//         var hpUIRoot = this.transform.Find("HpShow");
//         this.hpUIMgr.Init(hpUIRoot.gameObject, this);
//
//         //飘字管理
//         floatWordMgr = new FloatWordMgr();
//         var floatRoot = this.transform.Find("float_word_root");
//         floatWordMgr.Init(floatRoot);
//
//         //属性面板
//         attrUI = new BattleAttrUI();
//         var attrUIRoot = this.transform.Find("attrBar");
//         attrUI.Init(attrUIRoot.gameObject, this);
//         // attrUI.Hide();
//
//         //技能面板
//         var skillUIRoot = this.transform.Find("skillBar");
//         skillUI = new BattleSkillUI();
//         skillUI.Init(skillUIRoot.gameObject, this);
//
//         //队友英雄信息面板
//         var heroInfoUIRoot = this.transform.Find("all_player_info");
//         _heroInfoUIPre = new BattleHeroInfoUIPre();
//         _heroInfoUIPre.Init(heroInfoUIRoot.gameObject, this);
//
//         //buff 面板
//         var buffUIRoot = this.transform.Find("buffBar");
//         buffUI = new BattleBuffUI();
//         buffUI.Init(buffUIRoot.gameObject, this);
//
//         //通用描述面板
//         var describeUIRoot = this.transform.Find("DescribeBar");
//         describeUI = new DescribeUI();
//         describeUI.Init(describeUIRoot.gameObject, this);
//         describeUI.Hide();
//
//         //关卡信息界面
//         var stageUIRoot = this.transform.Find("stageInfo");
//         stageInfoUI = new BattleStageInfoUI();
//         stageInfoUI.Init(stageUIRoot.gameObject, this);
//     }
//
//     protected override void OnUpdate(float timeDelta)
//     {
//         this.hpUIMgr.Update(timeDelta);
//
//         this.floatWordMgr.Update(timeDelta);
//
//         this.skillUI.Update(timeDelta);
//
//         this.buffUI.Update(timeDelta);
//
//         this.describeUI.Update(timeDelta);
//
//         this._heroInfoUIPre.Update(timeDelta);
//
//         this.stageInfoUI.Update(timeDelta);
//     }
//
//
//     public void SetReadyShowState(bool isShow)
//     {
//         readyBgGo.SetActive(isShow);
//         // readyStartBtn.gameObject.SetActive(isShow);
//         if (isShow)
//         {
//             SetReadyBtnShowState(false);
//         }
//     }
//
//     public void SetReadyBtnShowState(bool isHasReady = false)
//     {
//         if (!isHasReady)
//         {
//             readyStartBtn.gameObject.SetActive(true);
//             hasReadyBtn.gameObject.SetActive(false);
//         }
//         else
//         {
//             readyStartBtn.gameObject.SetActive(false);
//             hasReadyBtn.gameObject.SetActive(true);
//         }
//     }
//
//     public void SetStateText(string stateStr)
//     {
//         stateText.text = stateStr;
//     }
//
//
//     #region 血条相关
//
//     public void RefreshHpShow(UIArgs args)
//     {
//         this.hpUIMgr.RefreshHpShow(args);
//     }
//
//     public void DestoryHpUI(int guid)
//     {
//         this.hpUIMgr.DestoryHpUI(guid);
//     }
//
//     public void SetHpShowState(int entityGuid, bool isShow)
//     {
//         this.hpUIMgr.SetHpShowState(entityGuid, isShow);
//     }
//
//     #endregion
//
//     #region 飘字相关
//
//     internal void ShowFloatWord(string word, GameObject go, int floatStyle, Color color)
//     {
//         floatWordMgr.ShowFloatWord(word, go, floatStyle, color);
//     }
//
//     #endregion
//
//     #region 属性面板相关
//
//     public void OpenAttrUI()
//     {
//         this.attrUI.Show();
//     }
//
//     public void CloseAttrUI()
//     {
//         this.attrUI.Hide();
//     }
//
//     public void RefreshBattleAttrUI(UIArgs args)
//     {
//         this.attrUI.Refresh(args);
//     }
//
//     #endregion
//
//     #region 技能面板相关
//
//     internal void RefreshBattleSkillUI(UIArgs args)
//     {
//         this.skillUI.Refresh(args);
//     }
//
//     public void RefreshSkillInfo(int skillConfigId, float currCDTime)
//     {
//         this.skillUI.UpdateSkillInfo(skillConfigId, currCDTime);
//     }
//
//     public void ShowSkillTipText(string str)
//     {
//         this.skillUI.SetSkillTipText(str);
//     }
//
//     #endregion
//
//     #region 队友英雄信息, boss 信息相关
//
//     internal void RefreshHeroInfoUI(UIArgs args)
//     {
//         this._heroInfoUIPre.Refresh(args);
//     }
//
//     internal void RefreshSingleHeroInfo(BattleHeroInfoUIData info, int fromEntityGuid)
//     {
//         this._heroInfoUIPre.RefreshSingleHeroInfo(info, fromEntityGuid);
//     }
//
//     public void StartBossLimitCountdown()
//     {
//         this._heroInfoUIPre.StartBossLimitCountdown();
//     }
//     
//     #endregion
//     
//     #region boss 强敌来袭的动画效果
//     public void StartBossComingAni()
//     {
//         bossComingRootGo.gameObject.SetActive(true);
//     }
//     
//     public void HideBossComing()
//     {
//         bossComingRootGo.gameObject.SetActive(false);
//     }
//     
//     #endregion
//
//     #region buff 面板相关
//
//     internal void RefreshBattleBuffUI(UIArgs args)
//     {
//         this.buffUI.Refresh(args);
//     }
//
//     public void RefreshBuffInfo(BattleBuffUIData buffInfo)
//     {
//         this.buffUI.UpdateBuffInfo(buffInfo);
//     }
//
//     #endregion
//
//     #region 描述面板相关
//
//     public void ShowDescribeUI(UIArgs arg)
//     {
//         this.describeUI.Refresh(arg);
//         this.describeUI.Show();
//     }
//
//     public void HideDescribeUI()
//     {
//         this.describeUI.Hide();
//     }
//
//     #endregion
//
//
//     #region 关卡信息相关
//
//     public void ShowStageInfoUI(UIArgs arg)
//     {
//         this.stageInfoUI.Refresh(arg);
//         this.stageInfoUI.Show();
//     }
//
//
//     public void HideStageInfoUI()
//     {
//         this.stageInfoUI.Hide();
//     }
//
//     #endregion
//
//     protected override void OnUnload()
//     {
//         onCloseBtnClick = null;
//         onReadyStartBtnClick = null;
//
//         this.attrUI.Release();
//         this.skillUI.Release();
//         this.buffUI.Release();
//         this.describeUI.Release();
//         this.floatWordMgr.Release();
//     }
// }