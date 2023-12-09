// using Battle_Client;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class HpData
// {
//     public BattleEntity_Client entity;
//
//     internal void Init()
//     {
//
//     }
//
//     internal void Refresh(BattleEntity_Client entity)
//     {
//         this.entity = entity;
//     }
//
// }
//
// public class HpModule
// {
//     BattleUIPre _battleUIPre;
//     Dictionary<int, HpData> hpDic = new Dictionary<int, HpData>();
//
//     public void Init(BattleUIPre battleUIPre)
//     {
//         this._battleUIPre = battleUIPre;
//     }
//
//     public void Refresh()
//     {
//
//     }
//
//     public void RefreshEntityData(BattleEntity_Client entity, int fromEntityGuid = 0)
//     {
//         this.RefreshEntityHp(entity, fromEntityGuid);
//     }
//
//     public void RefreshEntityHp(BattleEntity_Client entity, int fromEntityGuid)
//     {
//         HpData hpData = null;
//         if (hpDic.ContainsKey(entity.guid))
//         {
//             hpData = hpDic[entity.guid];
//         }
//         else
//         {
//             hpData = new HpData();
//             hpData.Init();
//             hpDic.Add(entity.guid, hpData);
//         }
//
//         hpData.Refresh(entity);
//
//         var selfPlayerIndex = BattleManager.Instance.GetLocalPlayer().playerIndex;
//         var currEntityPlayerIndex = entity.playerIndex;
//         bool isSelf = selfPlayerIndex == currEntityPlayerIndex;
//         bool isEnemy = currEntityPlayerIndex < 0;
//         EntityRelationType relationType = EntityRelationType.Friend;
//         if (isSelf)
//         {
//             relationType = EntityRelationType.Self;
//         }
//         else if (isEnemy)
//         {
//             relationType = EntityRelationType.Enemy;
//         }
//         else
//         {
//             relationType = EntityRelationType.Friend;
//         }
//
//         //Logx.Log("lll : " + selfPlayerIndex + " " + currEntityPlayerIndex + " " + relationType);
//
//         HpUIData args = new HpUIData()
//         {
//             entityGuid = entity.guid,
//             preCurrHp = entity.CurrHealth,
//             nowCurrHp = entity.CurrHealth,
//             maxHp = entity.MaxHealth,
//             entityObj = entity.gameObject,
//             relationType = relationType,
//             valueFromEntityGuid = fromEntityGuid
//
//         };
//
//         _battleUIPre?.RefreshHpShow(args);
//     }
//
//     public void DestroyEntityHp(BattleEntity_Client entity)
//     {
//         if (hpDic.ContainsKey(entity.guid))
//         {
//             var hpData = hpDic[entity.guid];
//             _battleUIPre.DestoryHpUI(entity.guid);
//             hpDic.Remove(entity.guid);
//         }
//         else
//         {
//             Logx.LogWarning("HpModule DestroyEntityHp : the guid is not found : " + entity.guid);
//         }
//     }
//
//     public HpData FindHpData(BattleEntity_Client entity)
//     {
//         HpData hpData = null;
//         if (hpDic.ContainsKey(entity.guid))
//         {
//             hpData = hpDic[entity.guid];
//         }
//         else
//         {
//             Logx.LogWarning("HpModule FindHp : the guid is not found : " + entity.guid);
//         }
//         return hpData;
//     }
//
//     public void Release()
//     {
//
//     }
//
//     internal void ChangeShowState(BattleEntity_Client entity, bool isShow)
//     {
//
//     }
// }
