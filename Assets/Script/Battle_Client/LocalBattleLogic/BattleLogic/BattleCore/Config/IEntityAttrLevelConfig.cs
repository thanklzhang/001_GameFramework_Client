namespace Battle
{
    public interface IEntityAttrLevelConfig : IConfig
    {
       int TemplateId { get; }

       int Level { get; }

       float Attack { get; }
       float Defence { get; }
       float Health { get; }
       float AttackSpeed { get; }
       float MoveSpeed { get; }
       float AttackRange { get; }


    }

}
