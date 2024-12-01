using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;

public class AttrHelper_Client
{
    public static float GetValue(EntityAttrType attrType, int value)
    {
        if (attrType == EntityAttrType.MoveSpeed ||
            attrType == EntityAttrType.AttackSpeed ||
            attrType == EntityAttrType.AttackRange ||
            attrType == EntityAttrType.InputDamageRate ||
            attrType == EntityAttrType.OutputDamageRate)
        {
            return value / 1000.0f;
        }

        return (float)value;
    }

    public static void GetBattleRewardContent(BattleReward_Client battleReward, bool isMakeSureReward,
        out string nameStr, out string desStr)
    {
        var configId = battleReward.configId;
        var rewardConfig = ConfigManager.Instance.GetById<Config.BattleReward>(configId);

        var type = (BattleRewardType)rewardConfig.Type;

        nameStr = rewardConfig.Name;
        desStr = rewardConfig.Describe;
        // var intValueList = this.data.intValueList;
        if (type == BattleRewardType.GainSkill_FixedRand)
        {
            var skillConfigId = battleReward.intArg1;
            nameStr = "获得技能";
            if (isMakeSureReward)
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
                desStr = $"获得技能:{skillConfig.Name}\n{skillConfig.Describe}";
            }
            else
            {
                desStr = "获得一个随机技能";
            }
        }
        else if (type == BattleRewardType.TeamMember_Gain)
        {
            nameStr = "获得队友";
            if (isMakeSureReward)
            {
                var entityConfigId = battleReward.intArg1;
                var entityConfig = ConfigManager.Instance.GetById<EntityInfo>(entityConfigId);
                desStr = $"获得队友：{entityConfig.Name}";
            }
            else
            {
                desStr = "获得一个队友";
            }

         
        }
        else if (type == BattleRewardType.TeamMember_RandAttr)
        {
            nameStr = "增加属性";
            if (isMakeSureReward)
            {
                desStr = "增加随机一个队友属性";
                var attrStr = GetAttrContent(battleReward.intArg1,
                    battleReward.intListArg1);
                desStr += attrStr;
            }
            else
            {
                desStr = "增加随机一个队友属性";
            }
        }
        else if (type == BattleRewardType.TeamMember_AllRandAttr)
        {
            nameStr = "增加属性";
            if (isMakeSureReward)
            {
                desStr = "所有队友增加属性：";

                var attrStr = GetAttrContent(battleReward.intArg1,
                    battleReward.intListArg1);
                desStr += attrStr;
            }
            else
            {
                desStr = "所有队友增加随机属性";
            }
        }
        else if (type == BattleRewardType.Leader_RandAttr)
        {
            nameStr = "增加属性";
            if (isMakeSureReward)
            {
                desStr = "增加队长属性：";

                var attrStr = GetAttrContent(battleReward.intArg1,
                    battleReward.intListArg1);
                desStr += attrStr;
            }
            else
            {
                desStr = "增加队长随机属性";
            }
        }
    }

    public static string GetAttrContent(int attrGroupConfigId, List<int> intValueList)
    {
        var attrGroupConfig = ConfigManager.Instance.GetById<BattleAttributeGroup>(attrGroupConfigId);

        var attrStr = "";
        for (int i = 0; i < attrGroupConfig.AddedAttrGroup.Count; i++)
        {
            var str = "";
            var attrType = (EntityAttrType)attrGroupConfig.AddedAttrGroup[i];
            var paramsList = attrGroupConfig.AddedValueGroup[i];
            var randList = attrGroupConfig.AddedValueRand[i];
            var attrValueType = (AddedValueType)paramsList[0];
            var attrValue = intValueList[i];

            var showAttrType = attrType;
            if (attrType >= EntityAttrType.Attack_Permillage)
            {
                showAttrType = EntityAttrType.Attack_Permillage - 1000;
            }

            var attrInfo = AttrInfoHelper.Instance.GetAttrInfo(showAttrType);
            if (attrValueType == AddedValueType.Fixed)
            {
                var resultValue = AttrHelper_Client.GetValue(attrType, attrValue);

                if ((int)attrType < (int)EntityAttrType.Attack_Permillage)
                {
                    var randMin = randList[0];
                    var randMax = randList[1];
                    str += $"{resultValue} 点 {attrInfo.name} ({randMin} ~ {randMax})";
                }
                else
                {
                    resultValue *= 100.0f;
                    var randMin = randList[0] * 100.0f;
                    var randMax = randList[1] * 100.0f;
                    str += $"{resultValue}% {attrInfo.name} ({randMin}% ~ {randMax}%)";
                }

                attrStr += str;
            }
            else
            {
                //其他的目前没有需求展示
            }
        }

        return attrStr;
    }
}