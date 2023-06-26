using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Table;
using LitJson;
using Battle.BattleTrigger.Runtime;

namespace Battle_Impl
{
    public class JsonDataProxy : ITriggerDataNode
    {
        JsonData jsonData;

        bool ITriggerDataNode.IsArray => jsonData.IsArray;

        public int Count => jsonData.Count;

        public ITriggerDataNode this[string key]
        {
            get
            {
                var jd = jsonData[key];
                return new JsonDataProxy(jd);
            }
            set
            {
                var jdEx = (JsonDataProxy)value;
                jsonData[key] = jdEx.jsonData;
            }
        }

        public ITriggerDataNode this[int idx]
        {
            get
            {
                var jd = jsonData[idx];
                return new JsonDataProxy(jd);
            }
            set
            {
                var jdEx = (JsonDataProxy)value;
                jsonData[idx] = jdEx.jsonData;
            }
        }

        public JsonDataProxy(JsonData jsonData)
        {
            this.jsonData = jsonData;
        }

        public Type GetDataType()
        {
            throw new NotImplementedException();
        }

        public string GetString(string key)
        {
            return jsonData[key].ToString();
        }

        public int GetInt(string key)
        {
            return int.Parse(jsonData[key].ToString());
        }

        public float GetFloat(string key)
        {
            return float.Parse(jsonData[key].ToString());
        }

        public ITriggerDataNode GetValueObj(string key)
        {
            var s = jsonData[key];
            JsonDataProxy jdEx = new JsonDataProxy(s);

            return jdEx;
        }

        public bool ContainsKey(string key)
        {
            return jsonData.ContainsKey(key);
        }

        public override string ToString()
        {
            return this.jsonData.ToString();
        }
    }
}
