/*
 * generate by tool
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using FixedPointy;
using System;
using System.IO;

public class HeroInfoTableStore : BaseTableStore<Table.HeroInfo>
{
    internal override void Load()
    {
        
        List = JsonMapper.ToObject<List<Table.HeroInfo>>(File.ReadAllText("HeroInfo.json"));

        Dic = List.ToDictionary(info => { return info.Id; });
    }
}
