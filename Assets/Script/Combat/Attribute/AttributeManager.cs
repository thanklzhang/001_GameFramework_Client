//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Script.Combat
//{
//    public class AttributeManager
//    {
//        CombatLogicEntity entity;
     
//        public Dictionary<EffectAttributeType, AttributeModel> attributes = new Dictionary<EffectAttributeType, AttributeModel>();

//        public void Init(CombatLogicEntity entity)
//        {
//            this.entity = entity;
//        }

//        public void InitAttribute(EffectAttributeType type, Fix baseValue)
//        {
//            if (!attributes.ContainsKey(type))
//            {
//                var attr = AttributeModel.Create(type, baseValue);
//                attributes.Add(type, attr);
//            }
//            else
//            {
//                Debug.Log("the type is exist : " + type);
//            }
//        }

//        public Fix GetValue(EffectAttributeType type)
//        {
//            if (attributes.ContainsKey(type))
//            {
//                return attributes[type].GetValue();
//            }
//            else
//            {
//                Debug.Log("the type is not exist : " + type);
//            }

//            return 0;
//        }


//        public void ChangeFixedValue(EffectAttributeType type, Fix value)
//        {
//            if (attributes.ContainsKey(type))
//            {
//                attributes[type].ChangeFixedValue(value);
//            }
//            else
//            {
//                Debug.Log("the type is not exist : " + type);
//            }
//        }

//        public void ChangeThousandValue(EffectAttributeType type, Fix value)
//        {
//            if (attributes.ContainsKey(type))
//            {
//                attributes[type].ChangeThousandValue(value);
//            }
//            else
//            {
//                Debug.Log("the type is not exist : " + type);
//            }
//        }
//    }
//}
