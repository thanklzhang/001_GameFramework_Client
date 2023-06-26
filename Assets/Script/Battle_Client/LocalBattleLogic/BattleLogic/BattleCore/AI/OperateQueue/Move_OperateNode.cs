using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class Move_OperateNode : OperateNode
    {
        //move param
        public Vector3 moveTargetPos;
        public int moveFollowTargetGuid;

        //如果目标是单位 那么指和目标差多少距离算作完成
        public float finishDistance;
      

        protected override void OnExecute()
        {
            if (moveFollowTargetGuid > 0)
            {
                var target = battle.FindEntity(moveFollowTargetGuid);
                if (target != null)
                {
                    ai.FindPathAndMoveToPos(target.position);
                }
                else
                {
                    //move 完成
                    //this.entity.ChangeToIdle();
                    this.operateModule.OnNodeExecuteFinish(this.key);
                }
            }
            else
            {
                ai.FindPathAndMoveToPos(moveTargetPos);
            }
        }

        protected override void OnUpdate()
        {
            if (moveFollowTargetGuid > 0)
            {
                var target = battle.FindEntity(moveFollowTargetGuid);

                var sqrDis = (this.entity.position - target.position).sqrMagnitude;
                if (sqrDis <= finishDistance * finishDistance)
                {
                    //move 完成
                    this.entity.ChangeToIdle();
                    this.operateModule.OnNodeExecuteFinish(this.key);
                }
                else
                {
                    //这里要判断 ai 寻路的点是否一致(包括) 一致的话不用再次寻路（可有一个路径缓冲池）
                    ai.FindPathAndMoveToPos(target.position);
                }


            }
            else
            {
                var sqrDis = (this.entity.position - moveTargetPos).sqrMagnitude;
                if (sqrDis <= finishDistance * finishDistance)
                {
                    //move 完成
                    this.entity.ChangeToIdle();
                    this.operateModule.OnNodeExecuteFinish(this.key);
                }
                else
                {
                    //这里要判断 ai 寻路的点是否一致(包括) 一致的话不用再次寻路（可有一个路径缓冲池）
                    ai.FindPathAndMoveToPos(moveTargetPos);
                }
            }

        }

        public override int GenKey()
        {
            return (int)OperateKey.Move;
        }
    }

    public enum OperateKey
    {
        Move = 100
    }



}
