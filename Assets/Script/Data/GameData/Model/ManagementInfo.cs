using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ManagementState
{
    Idle,
    Doing,
    Finish,
    Cooldown
}

public class ManagementInfo
{
    public ManagementState state;
    public long lastFinishTime;
    public long lastStartTime;
    public int totalUseBenchNum;
    ManagementInfo()
    {

    }

    public static ManagementInfo Create(ManagementState state, long lastStartTime, long lastFinishTime,int totalUseBenchNum)
    {
        var info = new ManagementInfo()
        {
            state = state,
            lastStartTime = lastStartTime,
            lastFinishTime = lastFinishTime,
            totalUseBenchNum = totalUseBenchNum
        };

        return info;
    }
}

