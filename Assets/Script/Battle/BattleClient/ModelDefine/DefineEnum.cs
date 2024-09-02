namespace Battle_Client
{
    //战斗状态
    public enum BattleState
    {
        Null = 0,
        Loading = 1,
        Running = 2,
        End = 3
    }
    
    //实体状态
    public enum BattleEntityState
    {
        Idle = 0,
        Move = 1,
        ReleasingSkill = 2,
        Dead = 3,
        Destroy = 4
    }

    //实体当前数据值类型
    public enum EntityCurrValueType
    {
        CurrHealth = 1,
        CurrMagic = 2,
    }

    //实体间的关系
    public enum EntityRelationType
    {
        //自己
        Me = 0,
        //友军
        Friend = 1,
        //敌人
        Enemy = 2
    }
    
    public enum BattleSkillEffectState
    {
        Idle = 0,
        Move = 1,
        WillDestroy = 2,
        Destroy = 3
    }
}