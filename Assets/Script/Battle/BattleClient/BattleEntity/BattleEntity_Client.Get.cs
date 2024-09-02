using System.Collections.Generic;

namespace Battle_Client
{
    public partial class BattleEntity_Client
    {
        public List<BattleSkillInfo> GetSkills()
        {
            return skills;
        }

        public List<BattleItemInfo> GetItems()
        {
            return itemList;
        }

        public List<BattleItemInfo> GetSkillItems()
        {
            return skillItemList;
        }
    }
}