using System;
using System.Collections.Generic;
using System.Linq;
using Table;

namespace Battle
{
    public class LocalBattleLogic_BattleConfig : IBattleConfig
    {
        public T GetById<T>(int id) where T : BaseTable
        {
            return Table.TableManager.Instance.GetById<T>(id);
        }

        public List<T> GetList<T>() where T : BaseTable
        {
            return Table.TableManager.Instance.GetList<T>();
        }

        public Dictionary<int, T> GetDic<T>() where T : BaseTable
        {
            return Table.TableManager.Instance.GetDic<T>();
        }
    }
}