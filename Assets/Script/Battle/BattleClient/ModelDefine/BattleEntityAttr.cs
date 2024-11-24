using Battle;

namespace Battle_Client
{
    public class BattleEntityAttr
    {
        public float attack;
        public float defence;
        public float maxHealth;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;

        //Ç§·Ö±È
        public float attack_Permillage;
        public float defence_Permillage;
        public float maxHealth_Permillage;
        public float attackSpeed_Permillage;
        public float attackRange_Permillage;
        public float moveSpeed_Permillage;

        public float GetValue(EntityAttrType type)
        {
            if (type == EntityAttrType.Attack)
            {
                // return this.attack * (1 + attack_Permillage / 1000.0f);
                return this.attack;
            }
            else if (type == EntityAttrType.Defence)
            {
                return this.defence;
            }
            else if (type == EntityAttrType.MaxHealth)
            {
                return this.maxHealth;
            }
            else if (type == EntityAttrType.AttackSpeed)
            {
                return this.attackSpeed;
            }
            else if (type == EntityAttrType.AttackRange)
            {
                return this.attackRange;
            }
            else if (type == EntityAttrType.MoveSpeed)
            {
                return this.moveSpeed;
            }
            else
            {
                return 0;
            }
        }

    }
}