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

    //实体状态数据值类型
    public enum EntityStateValueType
    {
        CurrHealth = 1,
        CurrMagic = 2,
        StarLevel = 3,
        StarExp = 4,
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

    public enum PlayerInputType
    {
        Null = 0,
        KeyCode_A = 97,
        KeyCode_Q = 113,
        KeyCode_W = 119,
        KeyCode_E = 101,
        KeyCode_R = 114,
        KeyCode_D = 100,
        KeyCode_F = 102,
    }

    public enum PlayerCommandType
    {
        Null = 0,
        NormalAttack = 10,

        //默认 q
        Skill1 = 101,

        //默认 w
        Skill2 = 102,

        //默认 e
        Skill3 = 103,

        //默认 r
        Skill4 = 104,

        //默认 d
        Skill5 = 105,

        //默认 f
        Skill6 = 106,
        Skill7 = 107,
        
        //打开商店
        OpenShopUI  = 201,
    }


    // public enum RewardQuality
    // {
    //     Green = 0,
    //     Blue = 1,
    //     Purple = 2,
    //     Orange = 3,
    //     Red = 4
    // }
}