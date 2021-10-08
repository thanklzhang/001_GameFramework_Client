using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BattleEntityManager : Singleton<BattleEntityManager>
{
    Dictionary<int, BattleEntity> entityDic;

    public void Init()
    {
        entityDic = new Dictionary<int, BattleEntity>();
    }

    public BattleEntity FindEntity(int guid)
    {
        if (entityDic.ContainsKey(guid))
        {
            return entityDic[guid];
        }
        else
        {
            Logx.LogWarning("the guid is not found : " + guid);
        }
        return null;
    }

    //public void CreateEntity(BattleEntity battleEntity)
    //{
    //    if (entityDic.ContainsKey(battleEntity.guid))
    //    {
    //        Logx.LogWarning("the guid is exist : " + battleEntity.guid);
    //        return;
    //    }

    //    entityDic.Add(battleEntity.guid, battleEntity);
    //}

    public BattleEntity CreateEntity(int guid, int configId)
    {
        if (entityDic.ContainsKey(guid))
        {
            Logx.LogWarning("the guid is exist : " + guid);
            return null;
        }

        BattleEntity entity = new BattleEntity();
        entity.Init(guid, configId);
        entityDic.Add(guid, entity);

        return entity;

        //EventDispatcher.Broadcast(EventIDs.OnCreateEntity,guid);
    }

    public void Update(float timeDelta)
    {
        foreach (var item in entityDic)
        {
            var entity = item.Value;
            entity.Update(timeDelta);
        }
    }

    public void DestoryEntity(int guid)
    {
        if (entityDic.ContainsKey(guid))
        {
            BattleEntity entity = entityDic[guid];
            entity.Destroy();
            entityDic.Remove(guid);
        }
        else
        {
            Logx.LogWarning("the guid is not found : " + guid);
        }
    }
}
