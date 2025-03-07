using System.Collections.Generic;
using Battle;

namespace Config
{
    public static class Skill_Tool
    {
        // public static List<int> GetSkilConfigIdsByLevels(List<int> skillLevels,int count = 1)
        // {
        //     List<int> list = new List<int>();
        //
        //     var allSkill = Config.ConfigManager.Instance.GetList<Config.Skill>();
        //     
        //     
        //     for (int i = 0; i < allSkill.Count; i++)
        //     {
        //         var fromSkillConfig = allSkill[i];
        //         if(skillLevels.Contains(fromSkillConfig.level))
        //     }
        //     
        //     var chapter = Config.ConfigManager.Instance.GetById<Config.MainTaskChapter>(chapterId);
        //     if (chapter != null)
        //     {
        //         var strs = chapter.StageList.Split(',');
        //         foreach (var idStr in strs)
        //         {
        //             var id = int.Parse(idStr);
        //             list.Add(id);
        //         }
        //     }
        //     return list;
        // }

        //
        public static int GetKeyAreaEft(int skillId)
        {
            var skillConfig = BattleConfigManager.Instance.GetById<ISkill>(skillId);
            if (skillConfig != null)
            {
                if (skillConfig.EffectList.Count > 0)
                {
                    for (int i = 0; i < skillConfig.EffectList.Count; i++)
                    {
                        var eftId = skillConfig.EffectList[i];
                        var type = SkillEffectFactory.GetTypeByConfigId(eftId);
                        if (type == SkillEffectType.AreaEffect)
                        {
                            return eftId;
                        }
                    }
                }
            }

            return -1;
        }
    }
}