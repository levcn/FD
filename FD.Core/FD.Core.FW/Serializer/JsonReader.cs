using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;


namespace Fw.Serializer
{
    /// <summary>
    /// Json字符串解析类
    /// </summary>
    public class JsonReader
    {
        public bool IsArray { get; private set; }
        public bool HaveError { get; private set; }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        public JsonReader(string jsonStr)
        {
            InitData(jsonStr);
        }

        public Dictionary<string, object> JsonObj { get; private set; }
        public ArrayList JsonArr { get; private set; }
        private void InitData(string jsonStr)
        {
            IsArray = CheckIsArray(jsonStr);
            if (!HaveError)
            {
                if (IsArray)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    JsonArr = serializer.Deserialize<ArrayList>(jsonStr);
                }
                else
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    JsonObj = serializer.Deserialize<Dictionary<string, object>>(jsonStr);
                }
            }
        }
        /// <summary>
        /// 返回对象的属性
        /// </summary>
        /// <returns></returns>
        public List<string> GetRootKeys()
        {
            if (IsArray) return new List<string>();
            return JsonObj.Keys.ToList();
        }
        bool CheckIsArray(string str)
        {
            var q = str.FirstOrDefault(w => w == '{' || w == '[');
            if (q == '{' || q == '[')
            {
                return q == '[';
            }
            HaveError = true;
            return false;
        }
    }
}
