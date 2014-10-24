using System.Text.RegularExpressions;


namespace STComponse.CFG
{
    public class RelConfig
    {
        private const string RegexStr = @"(?<RelTableName>.+?)\((?<RelMasertKey>.+?),(?<RelDictKey>.+?)\),(?<DictTableName>.+?)\((?<DictKey>.+?),(?<DictShowName>.+?)\)";
        private const string RegexStr1 = @"(?<DictTableName>.+?)\((?<DictKey>.+?),(?<DictShowName>.+?)\)";

        
        public static implicit operator RelConfig(string str)
        {
            Regex r = new Regex(RegexStr);
            str = str ?? "";
            var m = r.Match(str);
            if (!m.Success)
            {
                r = new Regex(RegexStr1);
                m = r.Match(str);
            }
            if (m.Success)
            {
                return new RelConfig {
                    RelTableName = m.Groups["RelTableName"].Value,
                    RelMasertKey = m.Groups["RelMasertKey"].Value,
                    RelDictKey = m.Groups["RelDictKey"].Value,
                    DictTableName = m.Groups["DictTableName"].Value,
                    DictKey = m.Groups["DictKey"].Value,
                    DictShowName = m.Groups["DictShowName"].Value,
                };
            }
            return new RelConfig();
        }
        public static implicit operator string(RelConfig str)
        {
            //            Regex r = new Regex(RegexStr);
            return string.Format("{0}({1},{2}),{3}({4},{5})", str.RelTableName, str.RelMasertKey, str.RelDictKey, str.DictTableName, str.DictKey, str.DictShowName);
        }
        /// <summary>
        /// 关联表名(SYS_RoleUserRel)
        /// </summary>
        public string RelTableName { get; set; }

        /// <summary>
        /// 关联表中主表的外键(UserID)
        /// </summary>
        public string RelMasertKey { get; set; }

        /// <summary>
        /// 关联表中字典表的外键(RoleID)
        /// </summary>
        public string RelDictKey { get; set; }

        /// <summary>
        /// 字典表名(Sys_Role)
        /// </summary>
        public string DictTableName { get; set; }

        /// <summary>
        /// 字典表的外键(ID)
        /// </summary>
        public string DictKey { get; set; }

        /// <summary>
        /// 字典表的显示名字段(Name)
        /// </summary>
        public string DictShowName { get; set; }

        public bool IsDict
        {
            get
            {
                return RelDictKey == "" && RelMasertKey == "" && RelTableName == "";
            }
        }
    }
}