using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameFunction : Singleton<GameFunction>
{
    public void Init()
    {

    }

    public void EnterHeroList()
    {
        CtrlManager.Instance.GetUICtrl<HeroListCtrl>().OpenUI();
    }
}

