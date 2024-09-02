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

        public float GetValue(EntityAttrType type)
        {
            if (type == EntityAttrType.Attack)
            {
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