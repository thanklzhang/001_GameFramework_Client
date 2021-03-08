using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroInfoCtrlArgs : CtrlArgs
{
    public int heroId;
}

public class GameFunction : Singleton<GameFunction>
{
    public void Init()
    {

    }

    public void EnterHeroList()
    {
        CtrlManager.Instance.GetUICtrl<HeroListCtrl>().Enter(null);
    }
    public void EnterHeroInfo(int heroId)
    {
        HeroInfoCtrlArgs args = new HeroInfoCtrlArgs();
        args.heroId = heroId;
        CtrlManager.Instance.GetUICtrl<HeroInfoCtrl>().Enter(args);
    }
}

