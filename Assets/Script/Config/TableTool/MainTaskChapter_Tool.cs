using System.Collections.Generic;

namespace Table
{
    public static class MainTaskChapter_Tool
    {
        public static List<int> GetStageIdsByChapterId(int chapterId)
        {
            List<int> list = new List<int>();
            var chapter = Table.TableManager.Instance.GetById<Table.MainTaskChapter>(chapterId);
            if (chapter != null)
            {
                var strs = chapter.StageList.Split(',');
                foreach (var idStr in strs)
                {
                    var id = int.Parse(idStr);
                    list.Add(id);
                }
            }
            return list;
        }
    }
}