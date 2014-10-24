using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace STComponse.StringData
{
//    CData t = new CData();
//            t["属性"]["基本参数"]["长*宽*高"] = "100*50*50(CM)";
//            t["属性"]["基本参数"]["颜色"] = "浅绿";
//            t["属性"]["基本参数"]["重量"] = "130KG";
//            t["属性"]["性能参数"]["功率"] = "100W";
//            t["属性"]["性能参数"]["最大输入电流"] = "260V";
//            t["属性"]["性能参数"]["频率范围"] = "40-80Hz";
//            t["属性"].RemoveKey("性能参数");
//            var der = t.ToJson();
//            var ttttt = der.ToObject<CData>();
//            return;
    public class CData
    {
        public CData()
        {
            Childs = new Dictionary<string, CData>();
        }

        public CData this[string k]
        {
            get
            {
                if (!Childs.ContainsKey(k))
                {
                    var tt = new CData();
                    Childs[k] = tt;
                }
                return Childs[k];
            }
            set
            {
                Childs[k] = value;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 下级列表
        /// </summary>
        public Dictionary<string, CData> Childs { get; set; }

        [XmlIgnore]
        public List<string> Keys
        {
            get
            {
                return Childs.Keys.ToList();
            }
        }

        /// <summary>
        /// 是否有下级
        /// </summary>
        /// <returns></returns>
        public bool HaveChilds()
        {
            return Childs.Count != 0;
        }

        /// <summary>
        /// 返回是否有指定的key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveKey(string key)
        {
            if (HaveKey(key))
            {
                Childs.Remove(key);
                return true;
            }
            return false;
        }
        public bool HaveKey(string key)
        {
            return Childs.ContainsKey(key);
        }
        public static implicit operator string(CData t)
        {
            return t.Value;
        }
        public static implicit operator CData(string t)
        {
            return new CData { Value = t };
        }
    }
}
