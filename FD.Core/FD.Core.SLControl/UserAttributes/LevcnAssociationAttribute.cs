using System;


namespace StaffTrain.FwClass.UserAttributes
{
    public class LevcnAssociationAttribute : Attribute
    {
        //public LevcnAssociationAttribute(string thisKey, string otherKey, RelationType relation)
        //{

        //}
        /// <summary>
        /// 当前实体的关联属性
        /// </summary>
        public string ThisKey { get; set; }

        /// <summary>
        /// 字典表的关联属性
        /// </summary>
        public string OtherKey { get; set; }

        /// <summary>
        /// 返回两个两的关系是否为关联表,还是字典表
        /// </summary>
        public RelationType Relation { get; set; }

        /// <summary>
        /// 定义关联表中,字典表的属性
        /// </summary>
        public string DictionaryKey { get; set; }

        /// <summary>
        /// 是否实时保存表关系,用于关联表中,是否删除实时删除关联表中无用的数据
        /// </summary>
        public bool SaveRelation { get; set; }
    }
}