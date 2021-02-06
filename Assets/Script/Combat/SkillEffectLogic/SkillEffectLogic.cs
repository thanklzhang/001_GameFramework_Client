//using Assets.Script.Combat;
//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class SkillEffectLogic
//{
//    public FixVec3 posistion;
//    public FixVec3 dir;

//    public CombatLogicEntity releaser;//释放者
//    public CombatLogicEntity target;//目标者
//    public FixVec3 targetPos;//目标点

//    int skillEffectSN;

//    public bool isWillDestroy;

//    public virtual void Init(int skillEffectSN, CombatLogicEntity releaser, CombatLogicEntity target, FixVec3 targetPos)
//    {
//        this.skillEffectSN = skillEffectSN;
//        this.releaser = releaser;
//        this.target = target;
//        this.targetPos = targetPos;
//    }

//    public virtual void Update()
//    {

//    }



//    public virtual void Start()
//    {

//    }

//    public void DestroySelf()
//    {
//        isWillDestroy = true;
//    }
//}

