using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine;

namespace Battle
{
    public class OperateModule
    {
        List<OperateNode> nodeList;
        BattleEntity entity;
        public void Init(BattleEntity entity)
        {
            this.entity = entity;
            nodeList = new List<OperateNode>();
        }

        public void AddOperate(OperateNode node)
        {
            AddOperate(new List<OperateNode>() { node });
        }

        public void AddOperate(List<OperateNode> addedNodeList)
        {
            foreach (var item in addedNodeList)
            {
                item.Init(this.entity, this);
            }

            if (0 == this.nodeList.Count)
            {
                this.nodeList.AddRange(addedNodeList);
            }
            else
            {
                var currExecuteNode = this.nodeList[0];
                if (currExecuteNode.type == OperateType.Move)
                {
                    this.nodeList.Clear();
                    this.nodeList.AddRange(addedNodeList);
                }
                else if (currExecuteNode.type == OperateType.ReleaseSkill)
                {
                    if (!currExecuteNode.IsCanBeBreak())
                    {
                        //从第二个操作开始全部移除 加入新的操作
                        for (int i = this.nodeList.Count - 1; i >= 1; i--)
                        {
                            this.nodeList.RemoveAt(i);
                        }

                        this.nodeList.AddRange(addedNodeList);
                    }
                    else
                    {
                        this.nodeList.Clear();

                        this.nodeList.AddRange(addedNodeList);
                    }
                }

            }
        }

        public void Update()
        {
            Handle();
        }

        OperateNode currNode;
        public void Handle()
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                //Debug.Log("zxy : test : " + nodeList[i].type);
            }
            if (nodeList.Count > 0)
            {
                currNode = nodeList[0];
                if (currNode.state == ExecuteState.Ready)
                {
                    currNode.Execute();
                }
                else if (currNode.state == ExecuteState.Doing)
                {
                    currNode.Update();
                }
            }
            else
            {
                //没有任何操作的话 那么检测自动攻击
            }
        }

        public void OnNodeExecuteFinish(int operateKey)
        {
            if (nodeList.Count > 0)
            {
                var currNode = nodeList[0];
                if (currNode.key == operateKey)
                {
                    //Debug.Log("zxy : test : Finish " + currNode.key);

                    currNode.Finish();
                    nodeList.RemoveAt(0);

                    //Handle();
                }

            }

        }

        internal bool IsHaveOperate()
        {
            return this.nodeList.Count > 0;
        }
    }

}
