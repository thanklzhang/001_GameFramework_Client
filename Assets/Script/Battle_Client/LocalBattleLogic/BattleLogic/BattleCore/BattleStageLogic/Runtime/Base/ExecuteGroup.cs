using System.Collections.Generic;

namespace Battle.BattleTrigger.Runtime
{
    public class ExecuteGroup
    {
        Trigger trigger;

        TriggerNode parent;

        public List<TriggerNode> actionNodeList;
        public int currExecuteIndex = -1;

        //public static ExecuteGroup Create(List<TriggerNode> nodeList, Trigger trigger)
        //{
        //    ExecuteGroup group = new ExecuteGroup();
        //    group.Init(nodeList, trigger);
        //    return group;
        //}

        //public void Init(List<TriggerNode> nodeList,Trigger trigger)
        //{
        //    this.trigger = trigger;
        //    this.actionNodeList = nodeList;
        //}

        public void SetTrigger(Trigger trigger)
        {
            this.trigger = trigger;
        }

        public Trigger GetTrigger()
        {
            return this.trigger;
        }

        internal void StartExecute(ActionContext context)
        {
            //_Battle_Log.Log("ExecuteGroup : StartExecute ");
            //triggerRoot.Execute(context);
            currExecuteIndex = 0;
            ExecuteNode(currExecuteIndex, context);
        }

        public void ExecuteNext(ActionContext context)
        {
            currExecuteIndex += 1;
            ExecuteNode(currExecuteIndex, context);
        }

        void ExecuteNode(int index, ActionContext context)
        {
            if (index < actionNodeList.Count)
            {
                var node = actionNodeList[index];
                node.Execute(context, index);
            }
            else
            {
                this.OnAllNodeFinish(context);
            }

        }

        internal void Update(float deltaTime, ActionContext context)
        {
            for (int i = 0; i < actionNodeList.Count; i++)
            {
                var actionNode = actionNodeList[i];
                if (actionNode.state == TriggerNodeState.Execute)
                {
                    //_G.Log("update : type : " + actionNode.GetType());
                    actionNode.Update(deltaTime, context);
                }
            }
        }

        public void OnNodeFinish(int index, ActionContext context)
        {
            //_Battle_Log.Log("ExecuteGroup : OnNodeFinish , currExecuteIndex : " + currExecuteIndex + " index : " + index);

            if (currExecuteIndex == index)
            {
                ExecuteNext(context);
            }
            else
            {
                _Battle_Log.LogError(string.Format("the currExecuteIndex {0} is not the same as index {1} : ", currExecuteIndex, index));
            }
        }


        public void OnAllNodeFinish(ActionContext context)
        {
            //_Battle_Log.Log("ExecuteGroup : OnAllNodeFinish");
            this.OnFinish(context);
        }

        //执行结束(包括没走完所有节点就退出的情况 如 return)
        public void OnFinish(ActionContext context)
        {
            if (parent != null)
            {
                if (parent is ParentNode)
                {
                    var pn = (ParentNode)parent;
                    //_Battle_Log.Log("OnAllNodeFinish : ChildExecuteFinish");
                    pn.ChildExecuteFinish(context);
                }
            }
            else
            {
                //trigger 结束了
                //_Battle_Log.Log("OnAllNodeFinish :  trigger.Finish");
                trigger.Finish();
            }

        }

        internal void SetActionNodeList(List<TriggerNode> nodeList)
        {
            this.actionNodeList = nodeList;
        }

        internal void SetParent(TriggerNode parent)
        {
            this.parent = parent;
        }
    }

}
