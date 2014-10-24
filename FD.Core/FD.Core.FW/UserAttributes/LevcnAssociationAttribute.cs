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
        /// �����������,�ֵ�������
        /// </summary>
        public string DictionaryKey { get; set; }

        /// <summary>
        /// �Ƿ�ʵʱ������ϵ,���ڹ�������,�Ƿ�ɾ��ʵʱɾ�������������õ�����
        /// </summary>
        public bool SaveRelation { get; set; }

        public bool LeftJoin { get; set; }
    }

    /// <summary>
    /// �������˷�������
    /// </summary>
    public class LevcnMethodAttribute:Attribute
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }
    }
}