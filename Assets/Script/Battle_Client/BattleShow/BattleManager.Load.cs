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
using Vector3 = Battle.Vector3;

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
            else if (this.battleType == BattleType.PureLocal)
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
            Logx.Log(LogxType.Game, "StartLoad_PureLocal : start laod");

            GameDataManager.Instance.UserStore.Uid = 1;
            var uid = GameDataManager.Instance.UserStore.Uid;


            //地图数据由本地加载
            EventSender.SendLoadingProgress(0.0f, "开始加载地图数据");
            // List<List<int>> mapList = null;
            MapSaveData mapSaveData = null;
            // yield return LoadMapData(battleConfigId, (list) =>
            // {
            //     mapList = list;
            // });
            yield return LoadMapData(battleConfigId, (map) => { mapSaveData = map; });

            Logx.Log(LogxType.Game, "StartLoad_PureLocal : load map config finish");

            var applyArg = ApplyBattleUtil.MakePureLocalApplyBattleArg(battleConfigId, (int)uid);

            //触发器配置由本地加载
            EventSender.SendLoadingProgress(0.1f, "开始加载触发器数据");
            TriggerSourceResData sourceData = null;
            yield return LoadTriggerResource(battleConfigId, (source) => { sourceData = source; });

            Logx.Log(LogxType.Game, "StartLoad_PureLocal : load trigger config finish");


            MapInitArg mapInitData = new MapInitArg();
            mapInitData.mapList = mapSaveData.mapList;
            mapInitData.posList = ToVector3s(mapSaveData.posList);
            mapInitData.playerInitPosList = ToVector3s(mapSaveData.playerInitPosList);

            //创建本地战斗数据
            SetLocalBattle(applyArg, sourceData, mapInitData, true);

            //开启本地战斗流程
            EventSender.SendLoadingProgress(0.2f, "开始启动本地战斗");
            yield return StartLocalBattle(battleClientArgs);

            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }

        List<Battle.Vector3> ToVector3s(List<float[]> _posList)
        {
            List<Battle.Vector3> pList = new List<Vector3>();
            ;
            for (int i = 0; i < _posList.Count; i++)
            {
                var p = _posList[i];

                pList.Add(ToVector3(p));
            }

            return pList;
        }

        Battle.Vector3 ToVector3(float[] _pos)
        {
            Vector3 pos = new Vector3(_pos[0], 0, _pos[1]);
            return pos;
        }

        public IEnumerator StartLoad_Common()
        {
            Logx.Log(LogxType.Game, "StartLoad_Common : load start");


            //读取战斗相关配置数据
            var battleTableId = BattleManager.Instance.battleConfigId;
            var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleTableId);
            var mapConfig = Table.TableManager.Instance.GetById<Table.BattleMap>(battleTb.MapId);
            var battleTriggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(battleTb.TriggerId);

            var sceneResTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(mapConfig.ResId);

            //加载场景
            EventSender.SendLoadingProgress(0.3f, "加载 场景 中");
            yield return SceneLoadManager.Instance.LoadRequest(sceneResTb.Name);
            SetCameraInfo();
            Logx.Log(LogxType.Game, "StartLoad_Common : scene load finish");

            // var sceneResId = mapConfig.ResId;
            // var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(sceneResId);
            // scenePath = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;

            //加载 UI 并打开
            EventSender.SendLoadingProgress(0.4f, "加载 战斗界面 中");
            yield return UICtrlManager.Instance.EnterRequest<BattleUICtrl>();
            Logx.Log(LogxType.Game, "StartLoad_Common : BattleUICtrl load finish");
            //battle ui
            // objsRequestList.Add(new LoadUIRequest<BattleUIPre>() { selfFinishCallback = OnUILoadFinish });

            //战斗实体资源
            EventSender.SendLoadingProgress(0.5f, "加载 战斗实体 中");
            yield return BattleEntityManager.Instance.LoadInitEntities();
            Logx.Log(LogxType.Game, "StartLoad_Common : entities load finish");
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