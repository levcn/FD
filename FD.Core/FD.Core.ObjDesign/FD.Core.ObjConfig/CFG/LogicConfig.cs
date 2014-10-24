using System.Collections.Generic;
using System.Linq;


namespace STComponse.CFG
{
    /// <summary>
    /// 字符串配置
    /// </summary>
    public class LogicConfig
    {
        /// <summary>
        /// 有效长度
        /// </summary>
        public List<LogicItem> Items { get; set; }
        public class LogicItem
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }

        public static implicit operator LogicConfig(string str)
        {
            if (str == null) return null;
            return new LogicConfig
            {
                Items = str.Split(',', '，')
                        .ToList()
                        .Select(w => w.Split('=', '＝'))
                        .Where(w => w.Length == 2 && w[0].ToInt(-100) != -100)
                        .Select(w => new LogicItem { Name = w[1], Value = w[0].ToInt() })
                        .ToList()
            };
        }
        public static implicit operator string(LogicConfig str)
        {
            if (str == null) return null;
            return str.Items.Select(w => string.Format("{0}={1}", w.Value, w.Name)).Serialize1(",");
        }
    }
}