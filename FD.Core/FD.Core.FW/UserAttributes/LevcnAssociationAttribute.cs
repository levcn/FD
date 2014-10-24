using System;


namespace Fw.UserAttributes
{
    public class LevcnAssociationAttribute:Attribute
    {
        //public LevcnAssociationAttribute(string thisKey,string otherKey,RelationType relation)
        //{
            
        //}
        public string ThisKey { get; set; }

        public string OtherKey { get; set; }

        public RelationType Relation { get; set; }

        /// <summary>
        /// 定义关联表中,字典表的属性
        /// </summary>
        public string DictionaryKey { get; set; }

        /// <summary>
        /// 是否实时保存表关系,用于关联表中,是否删除实时删除关联表中无用的数据
        /// </summary>
        public bool SaveRelation { get; set; }

        public bool LeftJoin { get; set; }
    }

    /// <summary>
    /// 服务器端方法定义
    /// </summary>
    public class LevcnMethodAttribute:Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}