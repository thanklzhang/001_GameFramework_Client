

using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class HpUIShowObj
{
    GameObject gameObject;
    Transform transform;

    public Transform bgRoot;
    public Transform hp;
    public Text valueText;

    HpUIMgr hpUIMgr;
    RectTransform parentTranRect;
    EntityHpColorSelector colorSelector;
    
    public Image hpBg;
    
    Color normalDamageColor = new Color(1, 0.744f, 0.108f);
    Color addHpColor = new Color(0, 1, 0, 1);

    //实体信息
    private float preCurrHp;
    private float nowCurrHp;
    private GameObject entityObj;
    private EntityRelationType relationType;
    private int fromEntityGuid;
    private float maxHp;

    public void Init(GameObject gameObject,BattleEntity_Client entity, HpUIMgr hpUIMgr)
    {
        this.gameObject = gameObject;
        this.hpUIMgr = hpUIMgr;
        parentTranRect = hpUIMgr.transform.GetComponent<RectTransform>();
        this.transform = this.gameObject.transform;

        bgRoot = this.transform.Find("hpMain/bg");
        hp = this.transform.Find("hpMain/bg/hpFill/hp");
        hpBg = hp.GetComponent<Image>();
        valueText = this.transform.Find("hpMain/valueText").GetComponent<Text>();
        colorSelector = hp.GetComponent<EntityHpColorSelector>();

        preCurrHp = entity.CurrHealth;

    }

    public void Refresh(BattleEntity_Client entity,int fromEntityGuid)
    {
        RefreshData(entity,fromEntityGuid);

        RefreshShow();
    }

    //刷新数据
    private void RefreshData(BattleEntity_Client entity,int fromEntityGuid)
    {
        //preCurrHp = entity.CurrHealth;
        nowCurrHp = entity.CurrHealth;
        entityObj = entity.gameObject;
        maxHp = entity.MaxHealth;
        this.fromEntityGuid = fromEntityGuid;
            
        var selfPlayerIndex = BattleManager.Instance.GetLocalPlayer().playerIndex;
        bool isSelf = selfPlayerIndex == entity.playerIndex;
        bool isSameTeam = BattleManager.Instance.IsSameTeam(selfPlayerIndex,entity.playerIndex);
        EntityRelationType relationType = EntityRelationType.Friend;
        if (isSelf)
        {
            relationType = EntityRelationType.Self;
        }
        else if (isSameTeam)
        {
            relationType = EntityRelationType.Friend;
        }
        else
        {
            relationType = EntityRelationType.Enemy;
        }
        
        this.relationType = relationType;
    }

    //刷新显示
    public void RefreshShow()
    {
        var currHp = this.nowCurrHp;
        var maxHp = this.maxHp;

        if (0 == maxHp)
        {
            maxHp = 1;
        }

        var ratio = currHp / maxHp;

        if (preCurrHp >= 0)
        {
            //飘字
            var changeHp = currHp - preCurrHp;

            var word = "" + changeHp;
            if (changeHp > 0)
            {
                word = "+" + changeHp;
            }
            var go = this.entityObj;
            FloatWordShowStyle floatStyle = FloatWordShowStyle.Left;
            var fromEntityGuid = this.fromEntityGuid;
            if (fromEntityGuid > 0)
            {
                var currEntityPos = go.transform.position;
                var fromEntity = BattleEntityManager.Instance.FindEntity(this.fromEntityGuid);
                if (fromEntity != null)
                {
                    var fromEntityPos = fromEntity.gameObject.transform.position;
                    var dir = (fromEntityPos - currEntityPos).normalized;
                    if (dir.x > 0)
                    {
                        floatStyle = FloatWordShowStyle.Left;
                    }
                    else
                    {
                        floatStyle = FloatWordShowStyle.Right;
                    }
                }

                Color color = normalDamageColor;
                if (changeHp > 0)
                {
                    color = addHpColor;
                }

                FloatWordBean bean = new FloatWordBean();
                bean.wordStr = word;
                bean.followGo = go;
                bean.color = color;
                bean.showStyle = floatStyle;
                bean.stateType = changeHp > 0 ? EntityAbnormalStateType.CurrHp_Add
                        : EntityAbnormalStateType.CurrHp_Sub;
                bean.triggerType = AbnormalStateTriggerType.Start;
                
                hpUIMgr.ShowFloatWord(bean);
            }
        }

        preCurrHp = nowCurrHp;

        var width = bgRoot.GetComponent<RectTransform>().rect.width;
        var currLen = width * ratio;

        hpBg.fillAmount = ratio;
        valueText.text = "" + currHp + "/" + maxHp;

        //背景颜色和字体颜色
        var relationType = this.relationType;

        if (relationType == EntityRelationType.Self)
        {
            hpBg.sprite = this.colorSelector.selfSprite;
        
        }
        else if (relationType == EntityRelationType.Friend)
        {
            hpBg.sprite = this.colorSelector.friendSprite;
        }
        else if (relationType == EntityRelationType.Enemy)
        {
            hpBg.sprite = this.colorSelector.enemySprite;
        }
    }

    public void Update(float timeDelta)
    {
        //血条跟随实体
        var entityObj = this.entityObj;
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        var screenPos = RectTransformUtility.WorldToScreenPoint(camera3D.camera, entityObj.transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTranRect, screenPos, cameraUI.camera, out uiPos);

        //这里可以换成实体上的血条挂点
        this.transform.localPosition = uiPos + Vector2.up * 100;
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    internal void SetShowState(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}

