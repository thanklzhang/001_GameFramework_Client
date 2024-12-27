using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    //玩家偏好设置相关
    public partial class ClientPlayer
    {
        public Dictionary<PlayerInputType, PlayerInputCommandModel> inputCommandDic;

        //TODO 技能初始化之后调用
        public void InitPreference()
        {
            inputCommandDic = new Dictionary<PlayerInputType, PlayerInputCommandModel>();

            AddInputCommand(PlayerInputType.KeyCode_A,new SkillCommandModel
            {
                commandType = PlayerCommandType.NormalAttack,
                skillConfigId = 1
            });
        }

        public void AddInputCommand(PlayerInputType inputType, PlayerInputCommandModel model)
        {
            model.inputType = inputType;
            if (!this.inputCommandDic.ContainsKey(inputType))
            {
                this.inputCommandDic.Add(inputType, model);
            }
            else
            {
                Logx.LogWarning(LogxType.Game, "has exist inputType : " + inputType);
            }
        }

        public PlayerInputCommandModel GetInputCommand(PlayerInputType inputType)
        {
            if (this.inputCommandDic.ContainsKey(inputType))
            {
                return this.inputCommandDic[inputType];
            }

            return null;
        }

        public bool TryToAddInputCommand(PlayerInputType inputType, PlayerInputCommandModel model)
        {
            if (!this.inputCommandDic.ContainsKey(inputType))
            {
                AddInputCommand(inputType, model);
                return true;
            }

            return false;
        }
    }

    public class PlayerInputCommandModel
    {
        public PlayerInputType inputType;
        public PlayerCommandType commandType;
    }

    public class SkillCommandModel : PlayerInputCommandModel
    {
        public int skillConfigId;
    }
}