//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Script.Combat
//{
//    public class AttributeModel
//    {
//        public EffectAttributeType type;
//        public Fix baseValue;
//        public Fix addedValue;
//        public Fix thousandValue;

//        //public void Init(EffectAttributeType type, Fix baseValue)
//        //{
//        //    this.type = type;
//        //    this.baseValue = baseValue;
//        //}

//        public void ChangeFixedValue(Fix value)
//        {
//            addedValue += value;
//        }

//        public void ChangeThousandValue(Fix value)
//        {
//            thousandValue += value;
//        }

//        public Fix GetValue()
//        {
//            return (baseValue + addedValue) * (1 + thousandValue / 1000);
//        }

//        public static AttributeModel Create(EffectAttributeType type, Fix baseValue)
//        {
//            AttributeModel model = new AttributeModel();
//            model.type = type;
//            model.baseValue = baseValue;

//            return model;
//        }
//    }
//}
