using System.Collections.Generic;
namespace Battle
{
    //init
    public class EntityInit
    {
        public int configId;
        public int level;
        //public int team;
        public int playerIndex;
        public Vector3 position;

        public List<SkillInit> skillInitList = new List<SkillInit>();
        internal bool isPlayerCtrl;
    }

    public class SkillInit
    {
        public int configId;
        public int level;
    }

    public class BattleEntityInitArg
    {
        public List<EntityInit> entityInitList = new List<EntityInit>();
    }


    ////////////// runtime

    public class BattleEntityMgr
    {
        Dictionary<int, BattleEntity> entityDic = new Dictionary<int, BattleEntity>();

        Battle battle;
        //public void SetBattle(Battle battle)
        //{
        //    this.battle = battle;
        //}

        public void Init(BattleEntityInitArg arg, Battle battle)
        {
            this.battle = battle;

            //填充 entityDic
            for (int i = 0; i < arg.entityInitList.Count; i++)
            {
                //注意 这里是初始化实体 不会发送创建实体事件
                var entityInit = arg.entityInitList[i];
                BattleEntity entity = AddEntity(entityInit);

                //设置是否是受玩家控制的英雄(目前一个玩家只能操控一个英雄实体)
                if (entityInit.isPlayerCtrl)
                {
                    battle.SetPlayerCtrlEntity(entity.playerIndex, entity.guid);
                }
            }
        }

        //添加实体
        private BattleEntity AddEntity(EntityInit entityInit)
        {
            BattleEntity entity = new BattleEntity();
            entity.configId = entityInit.configId;
            entity.playerIndex = entityInit.playerIndex;
            entity.position = entityInit.position;
            entity.guid = this.GenGuid();

            entityDic.Add(entity.guid, entity);
            entity.SetBattle(battle);
            entity.Init(entityInit);

            battle.RegisterEntityAI(entity);

            return entity;
        }


        ////添加实体并且发送创建实体事件
        //public BattleEntity CreateEntity(EntityInit entityInit)
        //{
        //    var entity = this.AddEntity(entityInit);
        //    List<BattleEntity> list = new List<BattleEntity>();
        //    list.Add(entity);
        //    this.battle.OnCreateEntities(list);

        //    
        //    entity.NotifyBattleData();

        //    return entity;

        //}

        public List<BattleEntity> CreateEntities(List<EntityInit> entityInitList)
        {
            List<BattleEntity> list = new List<BattleEntity>();
            foreach (var entityInit in entityInitList)
            {
                var entity = this.AddEntity(entityInit);
                list.Add(entity);
            }

            this.battle.OnCreateEntities(list);

            //同步战斗数据
            foreach (var item in list)
            {
                var entity = item;
                entity.NotifyBattleData();
            }


            return list;
        }

        int maxGuid = 1;
        private int GenGuid()
        {
            return maxGuid++;
        }

        internal BattleEntity FindEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                return entityDic[guid];
            }
            return null;
        }



        public void Update(float timeDelta)
        {

            //移除 entity
            List<BattleEntity> delList = new List<BattleEntity>();
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                var state = entity.EntityState;

                //死亡目前先移除 之后如果有对尸体做操作在进行更改
                if (state == EntityState.Dead)
                {
                    delList.Add(entity);
                }
            }

            for (int i = delList.Count - 1; i >= 0; i--)
            {
                var delEntity = delList[i];
                this.RemoveEntity(delEntity.guid);
            }
            //

            //update
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Update(timeDelta);
            }
        }

        internal Dictionary<int, BattleEntity> GetAllEntity(bool isIncludeDeath = false)
        {
            return entityDic;
        }

        public void RemoveEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                var entity = entityDic[guid];
                entity.Clear();
                entityDic.Remove(guid);
            }
            else
            {
                //_G.LogWarning("BattleEntityMgr RemoveEntity : the guid is not found : " + guid);
            }

        }

        internal void SetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            //List<BattleEntity> list = new List<BattleEntity>();
            foreach (var guid in entityGuids)
            {
                var entity = this.FindEntity(guid);
                if (entity != null)
                {
                    entity.SetShowState(isShow);
                    //list.Add(entity);
                }
                else
                {
                    //_G.LogWarning("BattleEntityMgr SetEntitiesShowState : the guid is not found : " + guid);

                }
            }

            this.battle.OnSetEntitiesShowState(entityGuids, isShow);
        }
    }


}
