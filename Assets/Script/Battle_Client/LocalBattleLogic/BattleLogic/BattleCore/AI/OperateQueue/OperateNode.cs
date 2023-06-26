using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{

    public enum OperateType
    {
        Move = 1,
        ReleaseSkill = 2
    }

    public enum ExecuteState
    {
        Ready = 0,
        Doing = 1,
        Finish = 2
    }

    public class OperateNode
    {
        public OperateModule operateModule;
        public Battle battle;
        public BattleEntity entity;
        public BaseAI ai;
        public OperateType type;
        public ExecuteState state;

        public int key;
        public void Init(BattleEntity entity, OperateModule root)
        {
            this.entity = entity;
            this.battle = entity.GetBattle();
            this.ai = battle.FindAI(entity.guid);
            this.operateModule = root;
            this.state = ExecuteState.Ready;
            this.key = this.GenKey();
            this.OnInit();
        }

        protected virtual void OnInit()
        {

        }

        public void Execute()
        {
            if (state == ExecuteState.Ready)
            {
                this.state = ExecuteState.Doing;
                this.OnExecute();

            }
        }

        protected virtual void OnExecute()
        {

        }

        public void Update()
        {
            this.OnUpdate();
        }

        protected virtual void OnUpdate()
        {

        }

        //子类不要调用
        public void Finish()
        {
            this.state = ExecuteState.Finish;
        }


        public virtual int GenKey()
        {
            return -1;
        }

        public virtual bool IsCanBeBreak()
        {
            return false;
        }


    }

}
