//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Script.Combat
//{
//    public class SkillModel
//    {
//        //public int SN;
//        public Config.SkillConfig config;

//        List<int> nextSkillEffects = new List<int>();

//        public Fix currCD;
//        public Fix maxCD;

//        CombatLogicEntity releaser;

//        public void Init()
//        {
//            //maxCD = 
//            ResetCD();
//        }

//        public void EnterCD()
//        {
//            currCD = maxCD;
//        }

//        public void ResetCD()
//        {
//            currCD = 0;
//        }

//        public void Update(Fix timeDelta)
//        {
//            if (currCD <= 0)
//            {
//                return;
//            }

//            currCD -= timeDelta;
//        }

//        public void Start(CombatLogicEntity target, FixVec3 targetPos)
//        {
//            var skills = SkillEffectLogicManager.Instance.Create(nextSkillEffects, releaser, target, targetPos);
//            skills.ForEach(s =>
//            {
//                s.Start();
//            });
//            EnterCD();
//        }

//        public void Finish()
//        {

//        }

//        public void StartNextEffect()
//        {

//        }
//    }
//}
