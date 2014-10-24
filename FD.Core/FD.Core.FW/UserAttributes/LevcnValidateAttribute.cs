//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Fw.UserAttributes
//{
//    public class LevcnValidateAttribute:Attribute
//    {
//        /// <summary>
//        /// 列别名，对应实体类别名
//        /// </summary>
//        public string ColumnName { get; set; }

//        /// <summary>
//        /// 验证正则的关键字，可以是多个
//        /// </summary>
//        public string ReguleKey { get; set; }

//        /// <summary>
//        /// 是否匹配所有正则
//        /// </summary>
//        public bool ForAllMatched { get; set; }

//        /// <summary>
//        /// 验证类型
//        /// </summary>
//        public ValidateType ValidateType { get; set; }

//        /// <summary>
//        /// 是否必填
//        /// </summary>
//        public bool IsRequired { get; set; }

//        /// <summary>
//        /// 提示消息
//        /// </summary>
//        public string TipMsg { get; set; }

//        /// <summary>
//        /// 错误消息
//        /// </summary>
//        public string ErrorMsg { get; set; }

//        /// <summary>
//        /// 成功消息
//        /// </summary>
//        public string PassMsg { get; set; }

//        /// <summary>
//        /// 自定义验证
//        /// </summary>
//        internal Func<string, bool> CustomValidate { get; set; }
//    }
//}
