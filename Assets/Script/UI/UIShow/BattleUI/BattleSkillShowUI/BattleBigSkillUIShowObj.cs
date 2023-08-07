using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleBigSkillUIShowObj : BattleSkillUIShowObj
{
   public override void OnInit()
   {
      canUseMaskGo = this.transform.Find("iconMaskBg/cantUse").gameObject;
      cdRootGo = this.transform.Find("iconMaskBg/CDRoot").gameObject;
      cdTimeText = this.transform.Find("iconMaskBg/CDRoot/CDShow/cd_text").GetComponent<Text>();
      cdImg = this.transform.Find("iconMaskBg/CDRoot/CDShow").GetComponent<Image>();
      icon = this.transform.Find("iconMaskBg/icon").GetComponent<Image>();

      evetnTrigger = icon.GetComponent<UIEventTrigger>();

      canUseMaskGo.SetActive(false);
      cdRootGo.SetActive(false);
      
      evetnTrigger.OnPointEnterEvent += OnPointEnter;
      evetnTrigger.OnPointerExitEvent += OnPointExit;
   }
}
