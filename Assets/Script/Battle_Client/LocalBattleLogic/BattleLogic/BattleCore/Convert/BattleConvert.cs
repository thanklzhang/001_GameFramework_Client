namespace Battle
{
    public class BattleConvert
    {
        //public static BattleInitArg ConvertToClientBattleProto(Battle battle)
        //{
        //    NetProto.BattleInitArg netBattleInitArg = new BattleInitArg();
        //    netBattleInitArg.Guid = battle.Guid;
        //    netBattleInitArg.RoomId = battle.RoomId;
        //    netBattleInitArg.TableId = battle.tableId;
        //    //netBattleInitArg.MapInitArg

        //    //玩家信息
        //    netBattleInitArg.BattlePlayerInitArg = new NetProto.BattlePlayerInitArg();
        //    var battlePlayers = battle.GetAllPlayers();
        //    foreach (var player in battlePlayers)
        //    {
        //        var netPlayer = new NetProto.BattlePlayerProto();
        //        netPlayer.Uid = (int)player.uid;
        //        netPlayer.PlayerIndex = player.playerIndex;
        //        netPlayer.Team = player.team;
        //        netPlayer.CtrlHeroGuid = player.ctrlHeroGuid;
        //        netBattleInitArg.BattlePlayerInitArg.PlayerList.Add(netPlayer);
        //    }

        //    //实体信息
        //    netBattleInitArg.EntityInitArg = new NetProto.BattleEntityInitArg();
        //    var entities = battle.GetAllEntities();
        //    foreach (var keyV in entities)
        //    {
        //        var entity = keyV.Value;

        //        var netEntity = new NetProto.BattleEntityProto();
        //        netEntity.Guid = entity.guid;
        //        netEntity.ConfigId = entity.configId;
        //        netEntity.PlayerIndex = entity.playerIndex;

        //        netEntity.Position = BattleConvert.ConvertToVector3Proto(entity.position);

        //        netEntity.MaxHp = (int)entity.MaxHealth;
        //        netEntity.CurrHp = netEntity.MaxHp;

        //        //技能
        //        var skills = entity.GetAllSkills();
        //        foreach (var skillKV in skills)
        //        {
        //            var skill = skillKV.Value;

        //            NetProto.BattleSkillProto netSkill = new BattleSkillProto();
        //            netSkill.ConfigId = skill.configId;
        //            netSkill.Level = skill.level;
        //            netEntity.SkillInitList.Add(netSkill);
        //        }
        //        netBattleInitArg.EntityInitArg.BattleEntityInitList.Add(netEntity);
        //    }

        //    return netBattleInitArg;
        //}

        //public static BattleInitArg ConvertToBattleProto(Battle.BattleArg battleArg)
        //{
        //    NetProto.BattleInitArg netBattleInitArg = new BattleInitArg();
        //    netBattleInitArg.Id = battleArg.id;
        //    netBattleInitArg.RoomId = battleArg.roomId;
        //    //netBattleInitArg.MapInitArg


        //    //player
        //    netBattleInitArg.BattlePlayerInitArg = new NetProto.BattlePlayerInitArg();
        //    foreach (var item in battleArg.battlePlayerInitArg.battlePlayerInitList)
        //    {
        //        var player = item;
        //        NetProto.BattlePlayerInit battlePlayerInit = new BattlePlayerInit();
        //        battlePlayerInit.PlayerIndex = player.playerIndex;
        //        battlePlayerInit.Team = player.team;
        //        battlePlayerInit.Uid = (int)player.uid;
        //        netBattleInitArg.BattlePlayerInitArg.PlayerList.Add(battlePlayerInit);
        //    }

        //    //entity
        //    netBattleInitArg.EntityInitArg = new NetProto.BattleEntityInitArg();
        //    foreach (var item in battleArg.entityInitArg.entityInitList)
        //    {
        //        var entityInit = item;
        //        NetProto.BattleEntityInit netEntity = new BattleEntityInit();
        //        netEntity.ConfigId = entityInit.configId;
        //        netEntity.Level = entityInit.level;
        //        netEntity.MaxHp = 100;
        //        netEntity.CurrHp = 100;
        //        netEntity.PlayerIndex = entityInit.playerIndex;
        //        netEntity.Position = ConvertToVector3Proto(entityInit.position);
        //        foreach (var skill in entityInit.skillInitList)
        //        {
        //            NetProto.BattleSkillInitArg netSkill = new BattleSkillInitArg();
        //            netSkill.ConfigId = skill.configId;
        //            netSkill.Level = skill.level;
        //            netEntity.SkillInitList.Add(netSkill);
        //        }
        //        netBattleInitArg.EntityInitArg.BattleEntityInitList.Add(netEntity);
        //    }

        //    return netBattleInitArg;

        //}


        //public static Vector3Proto ConvertToVector3Proto(Vector3 position)
        //{
        //    Vector3Proto v3Proto = new Vector3Proto();
        //    v3Proto.X = ToValue(position.x);
        //    v3Proto.Y = ToValue(position.y);
        //    v3Proto.Z = ToValue(position.z);
        //    return v3Proto;

        //}

        //public static Vector3 ConvertToVector3(Vector3Proto protoPos)
        //{
        //    Vector3 v3 = new Vector3();
        //    v3.x = GetValue(protoPos.X);
        //    v3.y = GetValue(protoPos.Y);
        //    v3.z = GetValue(protoPos.Z);
        //    return v3;

        //}

        public static int ToValue(float value)
        {
            int v = (int)(value * 1000);
            return v;
        }

        public static float GetValue(int value)
        {
            return value / 1000.0f;
        }
    }

}