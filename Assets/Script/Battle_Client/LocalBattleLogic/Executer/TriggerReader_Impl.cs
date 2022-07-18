using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Battle.BattleTrigger;
using System.IO;
using Battle.BattleTrigger.Runtime;
using Battle;
namespace Battle_Client
{

    public class TriggerReader_Impl : ITriggerReader
    {
        Battle.Battle battle;

        public TriggerReader_Impl(Battle.Battle battle)
        {
            this.battle = battle;
        }

        //override
        public List<Trigger> GetTriggers(int battleConfigId)
        {
            Table.Battle battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleConfigId);
            var triggerId = battleTb.TriggerId;
            Table.BattleTrigger striggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(triggerId);
            var jsonFileRootPath = striggerTb.ScriptPath;

            //这里文件应该是一开始都加载好 这里先这么读取
            string Battle_Config_Path = Const.buildPath + "/BattleTriggerConfig";
            string triggerPath = Battle_Config_Path + "/" + jsonFileRootPath;
            string[] files = System.IO.Directory.GetFiles(triggerPath);

            List<string> triggerJsonStrs = new List<string>();
            files.ToList().ForEach(file =>
            {
                //string jsonStr = battle.FileReader.GetTextFromFile(file);
                string jsonStr = FileOperate.GetTextFromFile(file);
                var ext = Path.GetExtension(file);
                if (ext == ".json")
                {
                    triggerJsonStrs.Add(jsonStr);
                }
            });

            List<Trigger> triggers = new List<Trigger>();
            foreach (var jsonStr in triggerJsonStrs)
            {
                var trigger = GetTrigger(jsonStr);
                triggers.Add(trigger);
            }

            return triggers;
        }

        //public Trigger GetTrigger(string json)
        //{
        //    Trigger trigger = new Trigger();

        //    var triggerNode = LitJson.JsonMapper.ToObject(json);
        //    var eventJsonData = triggerNode["triggerEvent"];
        //    var actionJsonData = triggerNode["executeGroup"];

        //    //事件
        //    var triggerEvent = ParseTriggerEvent(eventJsonData);

        //    trigger.triggerEvent = triggerEvent;

        //    //动作
        //    TriggerAction action = new TriggerAction();
        //    var executeGroup = ParseTriggerActionExecuteGroup(actionJsonData, trigger);
        //    executeGroup.SetTrigger(trigger);
        //    action.Init(executeGroup, trigger);

        //    trigger.triggerAction = action;

        //    return trigger;
        //}

        public Trigger GetTrigger(string json)
        {
            Trigger trigger = new Trigger();
            trigger.SetBattle(this.battle);


            var triggerNode = LitJson.JsonMapper.ToObject(json);
            JsonDataProxy jdEx = new JsonDataProxy(triggerNode);

            var eventJsonData = jdEx["triggerEvent"];
            var actionJsonData = jdEx["executeGroup"];

            //事件
            var triggerEvent = ParseTriggerEvent(eventJsonData);

            trigger.triggerEvent = triggerEvent;

            //动作
            TriggerAction action = new TriggerAction();
            var executeGroup = ParseTriggerActionExecuteGroup(actionJsonData, trigger);
            executeGroup.SetTrigger(trigger);
            action.Init(executeGroup, trigger);

            trigger.triggerAction = action;

            return trigger;
        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        //public TriggerEvent ParseTriggerEvent(LitJson.JsonData eventJsonData)
        //{
        //    TriggerEvent trigger = null;
        //    var eventJsonType = eventJsonData["__TYPE__"];
        //    var typeStr = eventJsonType.ToString();
        //    var strs = typeStr.Split('.');
        //    var str = strs[strs.Length - 1];

        //    var type = Type.GetType(NameSpaceName + "." + str);
        //    if (type != null)
        //    {
        //        if (type.IsSubclassOf(typeof(TriggerEvent)))
        //        {
        //            trigger = Activator.CreateInstance(type) as TriggerEvent;
        //            trigger.Parse(eventJsonData);
        //        }
        //        else
        //        {
        //            Logx.LogError("the type is not subClass of TriggerEvent : " + type.ToString());
        //        }
        //    }
        //    else
        //    {
        //        Logx.LogError("the type of event is not found : " + str);
        //    }

        //    return trigger;
        //}

        public TriggerEvent ParseTriggerEvent(ITriggerDataNode eventJsonData)
        {
            TriggerEvent trigger = null;
            var typeStr = eventJsonData.GetString("__TYPE__");
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var type = Type.GetType(NameSpaceName + "." + str);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(TriggerEvent)))
                {
                    trigger = Activator.CreateInstance(type) as TriggerEvent;
                    trigger.Parse(eventJsonData);
                }
                else
                {
                    Logx.LogError("the type is not subClass of TriggerEvent : " + type.ToString());
                }
            }
            else
            {
                Logx.LogError("the type of event is not found : " + str);
            }

            return trigger;
        }

        //public ExecuteGroup ParseTriggerActionExecuteGroup(LitJson.JsonData nodeJsonData, Trigger trigger, TriggerNode parent = null)
        //{
        //    ExecuteGroup group = new ExecuteGroup();

        //    var nodeList = ParseTriggerActionNodeList(nodeJsonData, group, trigger);
        //    group.SetActionNodeList(nodeList);
        //    group.SetTrigger(trigger);
        //    group.SetParent(parent);
        //    return group;
        //}

        public ExecuteGroup ParseTriggerActionExecuteGroup(ITriggerDataNode nodeJsonData, Trigger trigger, TriggerNode parent = null)
        {
            ExecuteGroup group = new ExecuteGroup();

            var nodeList = ParseTriggerActionNodeList(nodeJsonData, group, trigger);
            group.SetActionNodeList(nodeList);
            group.SetTrigger(trigger);
            group.SetParent(parent);
            return group;
        }

        ///// <summary>
        ///// 解析 action node list 
        ///// </summary>
        ///// <param name="nodeJsonData"></param>
        ///// <returns></returns>
        //public List<TriggerNode> ParseTriggerActionNodeList(LitJson.JsonData nodeJsonData, ExecuteGroup executeGroup, Trigger trigger)
        //{
        //    List<TriggerNode> actionNodeList = new List<TriggerNode>();
        //    var isArray = nodeJsonData.IsArray;
        //    if (!isArray)
        //    {
        //        return null;
        //    }

        //    for (int i = 0; i < nodeJsonData.Count; i++)
        //    {
        //        var nodeJson = nodeJsonData[i];
        //        var triggerNode = ParseTriggerActionNode(nodeJson, trigger);
        //        if (null == triggerNode)
        //        {
        //            continue;
        //        }
        //        triggerNode.Init(executeGroup);
        //        actionNodeList.Add(triggerNode);
        //    }
        //    return actionNodeList;
        //}

        /// <summary>
        /// 解析 action node list 
        /// </summary>
        /// <param name="nodeJsonData"></param>
        /// <returns></returns>
        public List<TriggerNode> ParseTriggerActionNodeList(ITriggerDataNode nodeJsonData, ExecuteGroup executeGroup, Trigger trigger)
        {
            List<TriggerNode> actionNodeList = new List<TriggerNode>();
            var isArray = nodeJsonData.IsArray;
            if (!isArray)
            {
                return null;
            }

            for (int i = 0; i < nodeJsonData.Count; i++)
            {
                var nodeJson = nodeJsonData[i];
                var triggerNode = ParseTriggerActionNode(nodeJson, trigger);
                if (null == triggerNode)
                {
                    continue;
                }
                triggerNode.Init(executeGroup);
                actionNodeList.Add(triggerNode);
            }
            return actionNodeList;
        }

        ///// <summary>
        ///// 解析 单个 action node
        ///// </summary>
        ///// <param name="nodeJsonData"></param>
        ///// <returns></returns>
        //public TriggerNode ParseTriggerActionNode(LitJson.JsonData nodeJsonData, Trigger trigger)
        //{
        //    if (null == nodeJsonData)
        //    {
        //        return null;
        //    }
        //    TriggerNode node = null;

        //    if (!nodeJsonData.ContainsKey("__TYPE__"))
        //    {
        //        return null;
        //    }
        //    var nodeJsonType = nodeJsonData["__TYPE__"];
        //    var typeStr = nodeJsonType.ToString();
        //    var strs = typeStr.Split('.');
        //    var str = strs[strs.Length - 1];

        //    var fullName = NameSpaceName + "." + str;
        //    Logx.Log("ParseTriggerActionNode fullName : " + fullName);
        //    var type = Type.GetType(fullName);
        //    if (type != null)
        //    {
        //        if (type.IsSubclassOf(typeof(ParentNode)))
        //        {
        //            node = Activator.CreateInstance(type) as ParentNode;
        //            node.SetTrigger(trigger);
        //            node.Parse(nodeJsonData);


        //        }
        //        else if (type.IsSubclassOf(typeof(ExecuteNode)))
        //        {
        //            node = Activator.CreateInstance(type) as ExecuteNode;
        //            node.SetTrigger(trigger);
        //            node.Parse(nodeJsonData);

        //        }
        //        else
        //        {
        //            Logx.LogError("the type is not subClass of ParentNode or ExecuteNode : " + type.ToString());
        //        }
        //    }
        //    else
        //    {
        //        Logx.LogError("the type of node is not found : " + str);
        //    }


        //    return node;
        //}


        /// <summary>
        /// 解析 单个 action node
        /// </summary>
        /// <param name="nodeJsonData"></param>
        /// <returns></returns>
        public TriggerNode ParseTriggerActionNode(ITriggerDataNode nodeJsonData, Trigger trigger)
        {
            if (null == nodeJsonData)
            {
                return null;
            }
            TriggerNode node = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            Logx.Log("ParseTriggerActionNode fullName : " + fullName);
            var type = Type.GetType(fullName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(ParentNode)))
                {
                    node = Activator.CreateInstance(type) as ParentNode;
                    node.SetTrigger(trigger);
                    node.Parse(nodeJsonData);


                }
                else if (type.IsSubclassOf(typeof(ExecuteNode)))
                {
                    node = Activator.CreateInstance(type) as ExecuteNode;
                    node.SetTrigger(trigger);
                    node.Parse(nodeJsonData);

                }
                else
                {
                    Logx.LogError("the type is not subClass of ParentNode or ExecuteNode : " + type.ToString());
                }
            }
            else
            {
                Logx.LogError("the type of node is not found : " + str);
            }


            return node;
        }



    }
}

