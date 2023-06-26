namespace Battle
{
    public interface IEntityAttrBaseConfig:IConfig
    {
        int Attack { get; }
        int Defence { get; }
        int Health { get; }
        int AttackSpeed { get; }
        int MoveSpeed { get; }
        int AttackRange { get; }
    }

}
