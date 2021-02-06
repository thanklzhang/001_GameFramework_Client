//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Script.Combat
//{
//    //目前不用
//    public class CombatLogicEntityManager : Singleton<CombatLogicEntityManager>
//    {
//        public Action<CombatLogicEntity> CreateEntityAction;

//        public void Init()
//        {

//        }

//        List<CombatLogicEntity> entityList = new List<CombatLogicEntity>();

//        public static int maxGuid = 1;
//        public static int GetGuid()
//        {
//            return maxGuid++;
//        }

//        public void CreateEntity(CombatLogicEntity entity)
//        {
//            var guid = GetGuid();
//            entity.guid = guid;
//            entityList.Add(entity);

//            CreateEntityAction?.Invoke(entity);
//        }

//        public CombatLogicEntity FindEntity(int guid)
//        {
//            var entity = entityList.Find(ent => ent.guid == guid);
//            return entity;
//        }

//        public void Update()
//        {
//            for (int i = entityList.Count - 1; i >= 0; --i)
//            {
//                var entity = entityList[i];
//                entity.Update();
//            }
//        }

//        public List<CombatLogicEntity> GetEntities()
//        {
//            return entityList;
//        }
//    }
//}
