using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic
{
    /// <summary>
    /// 使用字典的方式来存储数据，并输出标准JSON
    /// </summary>
    public class DictResponse
    {
        public bool _state { get; set; }
        public Dictionary<string, object> _data { get; set; }
        public string _message { get; set; }

        public DictResponse()
        {
            _state = false;
            _message = "Error";
            _data = new Dictionary<string, object>();
        }
        public string ToJson()
        {
            Newtonsoft.Json.Linq.JObject res = new Newtonsoft.Json.Linq.JObject();
            res.Add("state", _state);
            if (_state)
            {
                if (_data.Count > 0)
                {
                    Newtonsoft.Json.Linq.JObject data = new Newtonsoft.Json.Linq.JObject();
                    Newtonsoft.Json.Linq.JProperty propData = new Newtonsoft.Json.Linq.JProperty("data", data);
                    res.Add(propData);
                    foreach (string key in _data.Keys)
                    {
                        data.Add(key, _data[key].ToString());
                    }
                }
            }
            else
            {
                res.Add("message", _message ?? "错误");
            }
            return res.ToString();
        }
    }
}
