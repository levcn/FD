using System;
//using StaffTrain.FwClass.Validation;
using STComponse.ObjectConfig;


namespace StaffTrain.FwClass.UserAttributes
{
    /// <summary>
    /// ������֤
    /// </summary>
    public class LevcnValidateAttribute:Attribute
    {
        ///// <summary>
        ///// �б�������Ӧʵ�������
        ///// </summary>
        //public string ColumnName { get; set; }

        public Type CustomValidate { get; set; }

        /// <summary>
        /// �Զ�����֤ʱ ��׷�ӵ����� |�Ÿ�����ָ���ֶ������ɣ�Ĭ�϶���=���� �����Ҫ������Ӧ��ϵ�Ժ������
        /// </summary>
        public string CustomValidateWhere { get; set; }

        /// <summary>
        /// Ҫ��֤���ֶ�����Ĭ�ϵ�ǰ�ֶ�����һ�㲻��Ҫָ��
        /// ָ��ʱ��ͬʱָ������
        /// </summary>
        public string CustomValidateColumn { get; set; }     

        /// <summary>
        /// ��֤����Ĺؼ��֣������Ƕ��
        /// </summary>
        public string ReguleKey { get; set; }

        /// <summary>
        /// �Ƿ�ƥ����������
        /// </summary>
        public bool ForAllMatched { get; set; }

        /// <summary>
        /// ��֤����
        /// </summary>
        public ValidateType ValidateType { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// δ��Ĵ�����Ϣ
        /// </summary>
        public string RequiredErrorMsg { get; set; }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        public string TipMsg { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// �ɹ���Ϣ
        /// </summary>
        public string PassMsg { get; set; }

        /// <summary>
        /// ������󳤶�
        /// </summary>
        public int MaxLength { get; set; }
        
        /// <summary>
        /// �������ֵ
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// ������Сֵ
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// �ȽϵĶ���
        /// </summary>
        public string CompeareTo { get; set; }
        ///// <summary>
        ///// �Զ�����֤
        ///// </summary>
        //internal Func<string, bool> CustomValidate { get; set; }
    }
}
