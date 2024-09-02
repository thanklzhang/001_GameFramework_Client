using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public partial class BattleEntity_Client
    {
        public int guid;
        public int configId;
        public int playerIndex;
        public int level;
        public float CurrHealth;
        public int Team => BattleManager.Instance.GetTeamByPlayerIndex(this.playerIndex);
        public float MaxHealth => attr.maxHealth;
        
       
        public BattleEntityState state = BattleEntityState.Idle;
        
        public GameObject gameObject;
        public Collider collider;
        
        public float deadDisappearCurrTimer = 0;
        public float deadDisappearTotalTime = 2.0f;
        
        
        //attr
        public BattleEntityAttr attr = new BattleEntityAttr();
        
        List<BattleSkillInfo> skills;
        List<BattleItemInfo> itemList = new List<BattleItemInfo>();
        List<BattleItemInfo> skillItemList = new List<BattleItemInfo>();
        
        //初始化
        public void Init(int guid, int configId)
        {
            this.guid = guid;
            this.configId = configId;

            InitModel();
            
            isFinishLoad = false;
            
            InitItemList();
        }

        //道具初始化
        void InitItemList()
        {
            itemList = new List<BattleItemInfo>();
            for (int i = 0; i < 6; i++)
            {
                BattleItemInfo item = new BattleItemInfo();
                item.index = i;
                item.ownerGuid = this.guid;


                itemList.Add(item);
            }
        }

       
        internal void SetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        internal void SetItemList(List<BattleItemInfo> itemList)
        {
            this.itemList = itemList;
        }

        public BattleItemInfo FindItem(int index)
        {
            var item = this.itemList.Find((s) => { return s.index == index; });

            return item;
        }

        public BattleItemInfo FindSkillItem(int index)
        {
            var item = this.skillItemList.Find((s) => { return s.index == index; });

            return item;
        }

        internal void UpdateItemInfo(int index, int configId, int count, float currCDTime, float maxCDTime)
        {
            Logx.Log(LogxType.BattleItem, "entity (client) UpdateItemInfo : UpdateItemInfo , index : " + index +
                                          " , configId : " + configId + " , count : " + count);

            var item = this.FindItem(index);
            // if (null == item)
            // {
            //     item = new BattleItemInfo()
            //     {
            //         configId = configId,
            //         index = index,
            //         count = count,
            //         currCDTime = currCDTime,
            //         maxCDTime = maxCDTime
            //     };
            //     
            //     this.itemList.Add(item);
            //     this.itemList.Sort((a,b) =>
            //     {
            //         return a.index.CompareTo(b.index);
            //     });
            // }

            if (item != null)
            {
                item?.UpdateInfo(configId, count, currCDTime, maxCDTime);

                EventDispatcher.Broadcast(EventIDs.OnItemInfoUpdate, item);
            }
        }

        internal void UpdateSkillItemInfo(int index, int configId, int count, float currCDTime, float maxCDTime)
        {
            Logx.Log(LogxType.BattleItem, "entity (client) : UpdateSkillItemInfo , index : " + index +
                                          " , configId : " + configId + " , count : " + count);

            var item = this.FindSkillItem(index);
            // if (null == item)
            // {
            //     item = new BattleItemInfo()
            //     {
            //         configId = configId,
            //         index = index,
            //         count = count,
            //         currCDTime = currCDTime,
            //         maxCDTime = maxCDTime
            //     };
            //     
            //     this.itemList.Add(item);
            //     this.itemList.Sort((a,b) =>
            //     {
            //         return a.index.CompareTo(b.index);
            //     });
            // }

            if (item != null)
            {
                item?.UpdateInfo(configId, count, currCDTime, maxCDTime);
                if (count <= 0)
                {
                    this.skillItemList.Remove(item);
                }
            }
            else
            {
                item = new BattleItemInfo();
                item.configId = configId;
                item.count = count;
                item.currCDTime = currCDTime;
                item.maxCDTime = maxCDTime;
                item.index = index;
                item.ownerGuid = this.guid;

                this.skillItemList.Add(item);
            }

            EventDispatcher.Broadcast(EventIDs.OnSkillItemInfoUpdate, item);
        }

        public static EntityRelationType GetRelation(BattleEntity_Client aEntity, BattleEntity_Client bEntity)
        {
            if (null == aEntity || null == bEntity)
            {
                return EntityRelationType.Me;
            }

            if (aEntity.guid == bEntity.guid)
            {
                return EntityRelationType.Me;
            }
            else
            {
                if (aEntity.Team == bEntity.Team)
                {
                    return EntityRelationType.Friend;
                }
                else
                {
                    return EntityRelationType.Enemy;
                }
            }
        }

        internal void SyncAttr(List<BattleClientMsg_BattleAttr> attrs)
        {
            foreach (var item in attrs)
            {
                var type = (EntityAttrType)item.type;
                if (type == EntityAttrType.Attack)
                {
                    this.attr.attack = (int)item.value;
                }
                else if (type == EntityAttrType.Defence)
                {
                    this.attr.defence = (int)item.value;
                }
                else if (type == EntityAttrType.MaxHealth)
                {
                    this.attr.maxHealth = (int)item.value;
                }
                else if (type == EntityAttrType.MoveSpeed)
                {
                    // /1000
                    this.attr.moveSpeed = item.value;

                    //ani
                    var aniScale = this.attr.moveSpeed / normalAnimationMoveSpeed;
                    SetAnimationSpeed(aniScale);
                }
                else if (type == EntityAttrType.AttackSpeed)
                {
                    // /1000
                    this.attr.attackSpeed = item.value;
                }
                else if (type == EntityAttrType.AttackRange)
                {
                    // /1000
                    this.attr.attackRange = item.value;
                }

                //Logx.Log("sync entity attr : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);

                EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, 0);
            }
        }

        internal void SyncValue(List<BattleClientMsg_BattleValue> values)
        {
            foreach (var item in values)
            {
                var type = (EntityCurrValueType)item.type;
                var value = item.value;
                if (type == EntityCurrValueType.CurrHealth)
                {
                    this.CurrHealth = value;
                }
                //Logx.Log("sync entity curr value : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);

                EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, item.fromEntityGuid);
            }
        }

        public void Update(float timeDelta)
        {
            SetDirTarget(new Vector3(dirTarget.x, 0, dirTarget.z));
            SetTrueDir(timeDelta);

            //Battle._Battle_Log.Log("zxy path test : dir : " + this.gameObject.transform.forward.x + "," + this.gameObject.transform.forward.z + " -> " + dirTarget);
            if (state == BattleEntityState.Move)
            {
                UpdateMove(timeDelta);
            }

            if (triggerNames.Count > 0)
            {
                var str = triggerNames[^1];
                this.animator.SetTrigger(str);
                triggerNames.Clear();
            }

            if (this.state == BattleEntityState.Dead)
            {
                deadDisappearCurrTimer -= timeDelta;
                if (deadDisappearCurrTimer <= 0)
                {
                    this.state = BattleEntityState.Destroy;

                    //这里应该是设置成标志 然后删除
                    // BattleEntityManager.Instance.DestoryEntity(this.guid);
                }
            }
        }

        internal void SetShowState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }
        
       
        public IEnumerator RemoveSelf()
        {
            yield return new WaitForSeconds(2);

            //这里应该是设置成标志 然后删除
            BattleEntityManager.Instance.DestoryEntity(this.guid);
        }

        public void Dead()
        {
            PlayAnimation("die");
            state = BattleEntityState.Dead;

            deadDisappearCurrTimer = deadDisappearTotalTime;

            EventDispatcher.Broadcast(EventIDs.OnEntityDead, this);


            // CoroutineManager.Instance.StartCoroutine(RemoveSelf());
        }
        
        
        public void Destroy()
        {
            this.state = BattleEntityState.Destroy;
            if (isFinishLoad)
            {
                ResourceManager.Instance.ReturnObject(path, model);
                //model.transform.SetParent(GameMain.Instance.gameObjectRoot);
            }

            GameObject.Destroy(gameObject);

            EventDispatcher.Broadcast(EventIDs.OnEntityDestroy, this);
        }

        
    }
}