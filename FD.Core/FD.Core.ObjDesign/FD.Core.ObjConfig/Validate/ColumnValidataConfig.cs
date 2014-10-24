using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;


namespace SLComponse.Validate
{
    public class ColumnValidataConfig
    {
        public ColumnValidataConfig(string name)
        {
            Name = name;
            DataType = -1;
        }

        public ColumnValidataConfig()
        {}

        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Required { get; set; }
        /// <summary>
        /// 字符串类型 -1:无 0:正则,1:数字,2:小数,3:日期,4:信箱,5:邮编,6:身份证,7:表中唯一验证
        /// </summary>
        public int DataType { get; set; }
        public bool RequiredError { get; set; }
        public string RegexPattern { get; set; }
        public string RegexPatternError { get; set; }
        public int? TextMinLenth { get; set; }
        public int? TextMaxLenth { get; set; }
        public double? NumberMin { get; set; }
        public double? NumberMax { get; set; }

        [XmlIgnore]
        public static List<ValidType> ValidTypes
        {
            get { return new List<ValidType> {
                new ValidType{Code = -1,Name = "无"},
                new ValidType{Code = 0,Name = "正则"},
                new ValidType{Code = 1,Name = "数字"},
                new ValidType{Code = 2,Name = "小数"},
                new ValidType{Code = 3,Name = "日期"},
                new ValidType{Code = 4,Name = "信箱"},
                new ValidType{Code = 5,Name = "邮编"},
                new ValidType{Code = 6,Name = "身份证"},
                new ValidType{Code = 7,Name = "字段唯一"},
            };}
        }
    }

    public class ValidType
    {
        public ValidType()
        {
            Code = -1;
        }

        public string Name { get; set; }
        public int Code { get; set; }
    }
}
