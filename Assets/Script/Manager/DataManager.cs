using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DataManager : Singleton<DataManager>
{
    public HeroData heroData;

    public void Init()
    {
        heroData = new HeroData();


        heroData.Init();
    }
}
