//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace GameModel
//{
//    public class Item
//    {
//        public int SN;
//        public int id;
//        public int quality;
//        public int starLevel;
//        public Config.ItemConfig config;

//        //这些属性之后会根据表正常计算
//        public int finalAttack
//        {
//            get { return (int)config.baseAttack + (int)quality + starLevel; }
//        }

//        public int finalDefence
//        {
//            get { return (int)config.baseDefence + (int)quality + starLevel; }
//        }
//        public int finalMaxHealth
//        {
//            get { return (int)config.baseMaxHealth + (int)quality + starLevel; }
//        }
//        public int finalPrice
//        {
//            get { return (int)config.basePrice + (int)quality + starLevel; }
//        }

//        Item()
//        {

//        }

//        public static Item Create(int SN, int id)
//        {
//            var info = new Item()
//            {
//                SN = SN,
//                id = id,
//                config = Config.ConfigManager.Instance.GetBySN<Config.ItemConfig>(SN)
//            };

//            return info;
//        }

//        public static Item Create(GS2GC.Item item)
//        {

//            var info = Item.Create(item.SN, item.Id);
//            info.quality = item.Quality;
//            info.starLevel = item.StarLevel;
//            return info;
//        }

//    }

//}

