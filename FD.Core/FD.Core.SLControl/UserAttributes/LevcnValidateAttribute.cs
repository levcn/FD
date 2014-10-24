using System;
//using StaffTrain.FwClass.Validation;
using STComponse.ObjectConfig;


namespace StaffTrain.FwClass.UserAttributes
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public class LevcnValidateAttribute:Attribute
    {
        ///// <summary>
        ///// 列别名，对应实体类别名
        ///// </summary>
        //public string ColumnName { get; set; }

        public Type CustomValidate { get; set; }

        /// <summary>
        /// 自定义验证时 需追加的条件 |号隔开（指定字段名即可）默认都是=操作 如果需要其他对应关系以后再添加
        /// </summary>
        public string CustomValidateWhere { get; set; }

        /// <summary>
        /// 要验证的字段名，默认当前字段名，一般不需要指定
        /// 指定时需同时指定表名
        /// </summary>
        public string CustomValidateColumn { get; set; }     

        /// <summary>
        /// 验证正则的关键字，可以是多个
        /// </summary>
        public string ReguleKey { get; set; }

        /// <summary>
        /// 是否匹配所有正则
        /// </summary>
        public bool ForAllMatched { get; set; }

        /// <summary>
        /// 验证类型
        /// </summary>
        public ValidateType ValidateType { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 未填的错误信息
        /// </summary>
        public string RequiredErrorMsg { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string TipMsg { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 成功消息
        /// </summary>
        public string PassMsg { get; set; }

        /// <summary>
        /// 输入最大长度
        /// </summary>
        public int MaxLength { get; set; }
        
        /// <summary>
        /// 输入最大值
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// 输入最小值
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// 比较的对象
        /// </summary>
        public string CompeareTo { get; set; }
        ///// <summary>
        ///// 自定义验证
        ///// </summary>
        //internal Func<string, bool> CustomValidate { get; set; }
    }
}
