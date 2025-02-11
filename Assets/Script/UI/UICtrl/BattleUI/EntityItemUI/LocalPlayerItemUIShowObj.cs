using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//本地玩家实体的装备栏显示
public class LocalPlayerItemUIShowObj : ItemUIShowObj
{
    private DragScript dragScript;
    private DropScript dropScript;

    public override void Init(GameObject gameObject, BattleUI battleUI)
    {
        base.Init(gameObject, battleUI);
        dragScript = itemTran.gameObject.GetComponent<DragScript>();
        if (null == dragScript)
        {
            dragScript = itemTran.gameObject.AddComponent<DragScript>();
        }

        dragScript.target = itemTran;

        dragScript.onBeginDragBeforeAction += OnBeginDrag_Before;
        dragScript.onBeginDragAction += OnBeginDrag;
        dragScript.onDragAction += OnDrag;
        dragScript.onEndDragAction += OnEndDrag;


        dropScript = this.gameObject.GetComponent<DropScript>();
        dropScript.OnDropAction += OnDropAction;
    }


    //作为拖动开始点------------------
    public void OnBeginDrag_Before(PointerEventData eventData)
    {
        dragScript.transferData = this;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       

    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //this.parentUI.OnItemEndDrag(this,eventData);

       
    }


    //作为拖动结束点-------------------------
    public void OnDropAction(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        var dragScript = dropped.GetComponent<DragScript>();

        //TODO 可以通用
        if (dragScript.transferData is ItemUIShowObj)
        {
            //从实体道具栏拖过来的
            var showObj = dragScript.transferData as ItemUIShowObj;

            ItemMoveArg srcMoveArg = new ItemMoveArg();
            srcMoveArg.locationType = ItemLocationType.EntityItemBar;
            srcMoveArg.itemIndex = showObj.index;
            //这里如果还是用的 ItemUIShowObj 那么需要传入 entityGuid
            srcMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

            ItemMoveArg desMoveArg = new ItemMoveArg();
            desMoveArg.locationType = ItemLocationType.EntityItemBar;
            desMoveArg.itemIndex = this.index;
            desMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
            

            BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg, desMoveArg);
        }
        else if (dragScript.transferData is WarehouseItemUIShowObj)
        {
            //从玩家仓库拖过来的
            var showObj = dragScript.transferData as WarehouseItemUIShowObj;

            ItemMoveArg srcMoveArg = new ItemMoveArg();
            srcMoveArg.locationType = ItemLocationType.Warehouse;
            srcMoveArg.itemIndex = showObj.index;
            
            ItemMoveArg desMoveArg = new ItemMoveArg();
            desMoveArg.locationType = ItemLocationType.EntityItemBar;
            desMoveArg.itemIndex = this.index;
            desMoveArg.entityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

            BattleManager.Instance.MsgSender.Send_MoveItemTo(srcMoveArg, desMoveArg);
        }

        // GameObject.Destroy(dropped);
    }


    public override void Release()
    {
        base.Release();

        dragScript.onBeginDragBeforeAction -= OnBeginDrag_Before;
        dragScript.onBeginDragAction -= OnBeginDrag;
        dragScript.onDragAction -= OnDrag;
        dragScript.onEndDragAction -= OnEndDrag;

        dropScript.OnDropAction -= OnDropAction;
    }
}