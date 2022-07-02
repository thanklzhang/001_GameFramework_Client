using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatUI : BaseUI
{
    //CombatNetHandler handler;
    //public override void Init()
    //{
    //    this.resId = UIResIds.CombatUI;
    //}

    //public override void LoadFinish(UIContext context)
    //{
    //    base.LoadFinish(context);

    //    //heroNameText = root.Find("name_title_text/name_text").GetComponent<Text>();

    //    //closeBtn = root.Find("closeBtn").GetComponent<Button>();

    //    //closeBtn.onClick.AddListener(() =>
    //    //{
    //    //    this.Close();
    //    //});

    //    handler = NetHandlerManager.Instance.GetHandler<CombatNetHandler>();

    //    attackBtn = root.Find("attack_btn").GetComponent<Button>();
    //    attackValueText = root.Find("attack_value_title/attack_value").GetComponent<Text>();
    //    resultText = root.Find("result_text").GetComponent<Text>();
    //    skillsRoot = root.Find("skills");
    //    finishOperationBtn = root.Find("finish_operation_btn").GetComponent<Button>();
    //    skillUIObjTemp = skillsRoot.GetChild(0).gameObject;

    //    attackBtn.onClick.AddListener(OnClickAttackBtn);
    //    finishOperationBtn.onClick.AddListener(OnClickFinishOperationBtn);
    //    Debug.Log("show combat ui ");
    //}

    //void RefreshInfo()
    //{

    //}

    //void OnClickAttackBtn()
    //{
    //    //net
    //    //handler.ReqSendNormalAttack(() =>
    //    //{
    //    //    Debug.Log("attack success ");
    //    //});

    //}

    //void OnClickFinishOperationBtn()
    //{
    //    FinishPlayerOperation();
    //}

    //public override void Show()
    //{
    //    base.Show();
    //    EventManager.AddListener<CombatResultData>((int)GameEvent.CombatEnd, OnCombatEnd);
    //    EventManager.AddListener<int>((int)GameEvent.SelectEntity, ShowSkills);
    //    EventManager.AddListener<int>((int)GameEvent.CancelSelectEntity, HideSkills);


    //    //var handler = NetHandlerManager.Instance.GetHandler<HeroListHandler>();
    //    //handler.ReqServerHeroList(() =>
    //    //{
    //    //    RefreshInfo();
    //    //});




    //}


    //public override void Hide()
    //{
    //    base.Hide();
    //    EventManager.RemoveListener<CombatResultData>((int)GameEvent.CombatEnd, OnCombatEnd);
    //    EventManager.RemoveListener<int>((int)GameEvent.SelectEntity, ShowSkills);
    //    EventManager.RemoveListener<int>((int)GameEvent.CancelSelectEntity, HideSkills);
    //}

    ////round action ------------------------------------------------
    //void OnCombatRoundActionStart()
    //{

    //}

    //void FinishHeroOperation(HeroOperation operation)
    //{
    //    CombatManager.Instance.AddHeroOperation(operation);
    //}

    //void FinishPlayerOperation()
    //{
    //    CombatManager.Instance.FinishCurrRoundAction();
    //}

    ////------------------------------------------------
    //void OnCombatRoundShowStart()
    //{

    //}

    //public override void Update(float deltaTime)
    //{
    //    base.Update(deltaTime);


    //}

    //void ShowSkills(int heroGuid)
    //{
       
    //    var entity = CombatManager.Instance.GetEntityByGuid(heroGuid);
    //    //Debug.Log("zxy : show skill : " + entity.guid + " " + entity.configId);
    //    if (entity != null)
    //    {
           
    //        var sills = entity.GetSkills();
    //        //Debug.Log("zxy : sills count : " + sills.Count);
    //        skillsRoot.gameObject.SetActive(true);
    //        int i = 0;
    //        for (; i < sills.Count; i++)
    //        {
    //            var skill = sills[i];
    //            Debug.Log("zxy : sill : " + skill.configId);
    //            GameObject obj = null;
    //            if (i < skillsRoot.childCount)
    //            {
    //                obj = skillsRoot.GetChild(i).gameObject;
    //            }
    //            else
    //            {
    //                obj = GameObject.Instantiate(skillUIObjTemp, skillsRoot, false);
    //            }
    //            obj.SetActive(true);
    //            //之后抽成类
    //            obj.transform.Find("icon/Text").GetComponent<Text>().text = "" + skill.configId;
    //            var btn = obj.transform.Find("icon").GetComponent<Button>();
    //            btn.onClick.RemoveAllListeners();
    //            btn.onClick.AddListener(()=>
    //            {
    //                Debug.Log("zxy : click skill , id : " + skill.configId);
    //                CombatManager.Instance.OnSelectSkill(skill.configId);//之后分离
    //            });

    //        }

    //        for (; i < skillsRoot.childCount; i++)
    //        {
    //            skillsRoot.GetChild(i).gameObject.SetActive(false);
    //        }

    //    }
    //    else
    //    {
    //        Debug.Log("the heroGuid doesnt exist : " + heroGuid);
    //    }
    //}

    //void HideSkills(int heroGuid)
    //{
    //    skillsRoot.gameObject.SetActive(false);
    //}


    //void OnCombatEnd(CombatResultData result)
    //{
    //    Debug.Log("CombatEnd ... ");

    //    bool isWin = result.isWin;
    //    int enemyAttack = result.enemyAttack;

    //    var resultStr = "";
    //    resultStr += (isWin ? "win " : "lost ") + ", the attack of enemy is : ";
    //    resultStr += enemyAttack;

    //    resultText.text = resultStr;

    //    CoroutineManager.Instance.StartCoroutine(ExitCombat());
    //}

    ////模拟战斗结算界面完成
    //IEnumerator ExitCombat()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    this.Close();
    //    GameStateManager.Instance.ChangeState(GameState.Lobby);
    //}

    //Button attackBtn;
    //Text attackValueText;
    //Text resultText;

    //GameObject skillUIObjTemp;
    //Transform skillsRoot;
    //Button finishOperationBtn;


}
public class CombatResultData
{
    public bool isWin;
    public int enemyAttack;
}