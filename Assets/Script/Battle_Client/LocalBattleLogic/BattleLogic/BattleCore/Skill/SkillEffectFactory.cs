namespace Battle
{
    public enum SkillEffectType
    {
        ProjectileEffect = 3,
        CalculateEffect = 4,
        AreaEffect = 13,
        BuffEffect = 14,
        MoveEffect = 16,
        CollisionGroupEffect = 17,
        PassiveEffect = 18,
    }
    public class SkillEffectFactory
    {
        //public static SkillEffect GenByType(SkillEffectType type)
        //{
        //    SkillEffect effect = null;
        //    if (type == SkillEffectType.ProjectileEffect)
        //    {
        //        effect = new ProjectileEffect();
        //    }

        //    return effect;
        //}

        public static SkillEffectType GetTypeByConfigId(int configId)
        {
            //����Ҳ���Ը��ݱ����ǰ������ Ȼ������Ƿ�����ֵ���
            var type = (SkillEffectType)(configId / 1000000);
            return type;
        }

        //public static SkillEffect GenByConfigId(int configId)
        //{
        //    var type = GetTypeByConfigId(configId);

        //    return GenByType(type);
        //}
    }
}