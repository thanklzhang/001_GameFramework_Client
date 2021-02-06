//using Assets.Script.Combat;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum EnityType
//{
//    Hero,
//    Soldier,
//    Monster,
//    Tower,
//    Home
//}
//public class CombatViewEntityManager : Singleton<CombatViewEntityManager>
//{

//    public GameObject heroPrefab;//实体的资源 将要变成根据 id 进行加载
//    public GameObject soldierPrefab;
//    public GameObject towerPrefab;
//    public GameObject homePrefab;
    

//    Transform WorldRoot;

//    public void Init()
//    {
//        WorldRoot = GameObject.Find("WorldRoot").transform;
//        //test
//        heroPrefab = Resources.Load("Entity/Hero_10000") as GameObject;

//        CombatLogicEntityManager.Instance.CreateEntityAction += CreateEntity;
//    }

//    public List<CombatViewEntity> entityCtrlList = new List<CombatViewEntity>();

//    public static int maxGuid = 1;
//    public Action<CombatViewEntity> CreateEntityAction;

//    public void CreateEntity(CombatLogicEntity entity)
//    {
//        GameObject obj = GameObject.Instantiate(heroPrefab, WorldRoot);
//        //test 
//        var ctrl = CombatViewEntity.Create(obj, Vector3.zero + entity.seat * Vector3.one, entity);
//        entityCtrlList.Add(ctrl);

//        CreateEntityAction?.Invoke(ctrl);
//    }

//    public CombatViewEntity FindEntityCtrl(int guid)
//    {
//        var entity = entityCtrlList.Find(ent => ent.entity.guid == guid);
//        return entity;
//    }

//    public void Update(float deltaTime)
//    {
//        foreach (var item in entityCtrlList)
//        {
//            item.Update(deltaTime);
//        }
//    }
//}

