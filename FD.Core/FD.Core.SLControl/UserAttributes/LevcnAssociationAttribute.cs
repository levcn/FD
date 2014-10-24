using System;


namespace StaffTrain.FwClass.UserAttributes
{
    public class LevcnAssociationAttribute : Attribute
    {
        //public LevcnAssociationAttribute(string thisKey, string otherKey, RelationType relation)
        //{

        //}
        /// <summary>
        /// ��ǰʵ��Ĺ�������
        /// </summary>
        public string ThisKey { get; set; }

        /// <summary>
        /// �ֵ��Ĺ�������
        /// </summary>
        public string OtherKey { get; set; }

        /// <summary>
        /// �����������Ĺ�ϵ�Ƿ�Ϊ������,�����ֵ��
        /// </summary>
        public RelationType Relation { get; set; }

        /// <summary>
        /// �����������,�ֵ�������
        /// </summary>
        public string DictionaryKey { get; set; }

        /// <summary>
        /// �Ƿ�ʵʱ������ϵ,���ڹ�������,�Ƿ�ɾ��ʵʱɾ�������������õ�����
        /// </summary>
        public bool SaveRelation { get; set; }
    }
}