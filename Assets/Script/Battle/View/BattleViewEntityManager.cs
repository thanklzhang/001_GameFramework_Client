//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class BattleViewEntityManager : Singleton<BattleViewEntityManager>
//{
//    Dictionary<int, BattleViewEntity> viewEntityDic;

//    public void Init()
//    {
//        EventDispatcher.AddListener<BattleEntity>(EventIDs.OnCreateEntity, OnCreateEntity);

//        viewEntityDic = new Dictionary<int, BattleViewEntity>();
//    }

//    //event
//    void OnCreateEntity(BattleEntity entity)
//    {
//        CreateViewEntity(entity);
//    }
//    //

//    public BattleViewEntity FindViewEntity(int guid)
//    {
//        if (viewEntityDic.ContainsKey(guid))
//        {
//            return viewEntityDic[guid];
//        }
//        else
//        {
//            Logx.LogWarning("the guid is not found : " + guid);
//        }
//        return null;
//    }



//    public void CreateViewEntity(BattleEntity entity)
//    {
//        if (viewEntityDic.ContainsKey(entity.guid))
//        {
//            Logx.LogWarning("the guid is exist : " + entity.guid);
//            return;
//        }

//        BattleViewEntity viewEntity = new BattleViewEntity();
//        viewEntity.Init(entity);

//        viewEntityDic.Add(entity.guid, viewEntity);

//    }
//}
