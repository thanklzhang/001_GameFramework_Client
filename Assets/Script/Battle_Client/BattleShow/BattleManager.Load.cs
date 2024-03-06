using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle.BattleTrigger.Runtime;
using Battle_Client;
using GameData;
using NetProto;
using Table;
using UnityEditor;
using UnityEngine;

namespace Battle_Client
{
    //战斗中的摄像机相关
    public partial class BattleManager
    {
        // public void StartLoad()
        // {
        //     
        // }

        public IEnumerator StartLoad()
        {
            if (this.battleType == BattleType.Remote)
            {
                yield return StartLoad_Remote();
            }
            else  if (this.battleType == BattleType.PureLocal)
            {
                yield return StartLoad_PureLocal();
            }

            OnLoadFinish();
        }

        public void OnLoadFinish()
        {
            skillDirectModule = new SkillDirectorModule();
            skillDirectModule.Init();

            skillTrackModule = new SkillTrackModule();
            skillTrackModule.Init();
        }

        /// <summary>
        /// 远端战斗加载
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_Remote()
        {
            
            //填充客户端所需组件
            msgSender = new BattleClient_MsgSender_Remote();
            msgReceiver = new BattleClient_MsgReceiver_Impl();

            //创建战斗数据
            CreateBattleData(battleClientArgs);
            
            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }
        
        /// <summary>
        /// 纯本地战斗加载
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_PureLocal()
        {
            Logx.Log(LogxType.Game,"StartLoad_PureLocal : start laod");
            
            GameDataManager.Instance.UserStore.Uid = 1;
            var uid = GameDataManager.Instance.UserStore.Uid;
            
            //地图数据由本地加载
            List<List<int>> mapList = null;
            yield return LoadMapData(battleConfigId, (list) =>
            {
                mapList = list;
            });
            
            Logx.Log(LogxType.Game,"StartLoad_PureLocal : load map config finish");
            
            var applyArg = ApplyBattleUtil.MakePureLocalApplyBattleArg(battleConfigId, (int)uid);

            //触发器配置由本地加载
            TriggerSourceResData sourceData = null;
            yield return LoadTriggerResource(battleConfigId, (source) =>
            {
                sourceData = source;
            });
            
            Logx.Log(LogxType.Game,"StartLoad_PureLocal : load trigger config finish");
            
            
            MapInitArg mapInitData = new MapInitArg();
            mapInitData.mapList = mapList;
            
            //创建本地战斗数据
            SetLocalBattle(applyArg, sourceData, mapInitData, true);

            //开启本地战斗流程
            yield return StartLocalBattle(battleClientArgs);
            
            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
            
        }
        
        public IEnumerator StartLoad_Common()
        {
            Logx.Log(LogxType.Game,"StartLoad_Common : load start");
            
            //读取战斗相关配置数据
            var battleTableId = BattleManager.Instance.battleConfigId;
            var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleTableId);
            var mapConfig = Table.TableManager.Instance.GetById<Table.BattleMap>(battleTb.MapId);
            var battleTriggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(battleTb.TriggerId);

            var sceneResTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(mapConfig.ResId);
            
            //加载场景
            yield return SceneLoadManager.Instance.LoadRequest(sceneResTb.Name);
            SetCameraInfo();
            Logx.Log(LogxType.Game,"StartLoad_Common : scene load finish");
            
            // var sceneResId = mapConfig.ResId;
            // var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(sceneResId);
            // scenePath = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;
        
            //加载 UI 并打开
            yield return UICtrlManager.Instance.EnterRequest<BattleUICtrl>();
            Logx.Log(LogxType.Game,"StartLoad_Common : BattleUICtrl load finish");
            //battle ui
            // objsRequestList.Add(new LoadUIRequest<BattleUIPre>() { selfFinishCallback = OnUILoadFinish });
     
            //战斗实体资源
            yield return BattleEntityManager.Instance.LoadInitEntities();
            Logx.Log(LogxType.Game,"StartLoad_Common : entities load finish");
            // var path = 
            // ResourceManager.Instance.GetObject<GameObject>(path, (gameObject) =>
            // {
            //     //viewEntity.OnLoadModelFinish(gameObject);
            //     this.OnFinishLoadEntityObj(battleEntity, gameObject);
            // });   
            // entityLoadReqs = BattleEntityManager.Instance.MakeCurrBattleAllEntityLoadRequests(OnEntityLoadFinish);
            // objsRequestList.AddRange(entityLoadReqs);
        }
        
    }


}