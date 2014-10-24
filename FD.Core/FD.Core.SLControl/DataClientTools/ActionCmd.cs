using System.Collections.Generic;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// ����
    /// </summary>
    public class ActionCommand
    {
        public ActionCommand()
        {
            Entity = new Entity();
            Operator = new Operator();
            Parameter = new Parameter();
        }
        /// <summary>
        /// ʵ��
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public Operator Operator { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public Parameter Parameter { get; set; }
    }

    /// <summary>
    /// ����ʵ��
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// ������������л��ַ���
        /// </summary>
        public string ActionObjectEntryStr { get; set; } // 


        /// <summary>
        /// ���������ʵ��������
        /// </summary>
        public string ActionObjectName { get; set; } // 
    }

    public class Operator
    {
        public Operator()
        {
            Version = 2;
        }
        /// <summary>
        /// �Ƿ����Զ�������
        /// </summary>
        public bool IsCustomCmd { get; set; }

        /// <summary>
        /// �Զ�������
        /// </summary>
        public CustomCmd CustomCmd { get; set; }

        /// <summary>
        /// ��������(��ѯ��ɾ�����޸�)select,update,delete,insert
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// ���ݲ����İ汾,1:ԭʼ�汾,2:ʹ�������ļ��İ汾
        /// </summary>
        public int Version { get; set; }
    }

    public class Parameter
    {
        /// <summary>
        /// �洢���̲���
        /// </summary>
        public StoredProcedureParams StoredProcedureParams { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public SelectAcionParams SelectAcionParams { get; set; } //

        
        /// <summary>
        /// �ֶι���
        /// </summary>
        public FieldFilter FieldFilter { get; set; }
    }
//    /// <summary>
//    /// ���տͻ��˷���������
//    /// </summary>
//    public class ActionCmd
//    {
//        /// <summary>
//        /// �Ƿ����Զ�������
//        /// </summary>
//        public bool IsCustomCmd { get; set; }
//
//        /// <summary>
//        /// �Զ�������
//        /// </summary>
//        public CustomCmd CustomCmd { get; set; }
//
//        /// <summary>
//        /// ��������(��ѯ��ɾ�����޸�)select,update,delete,insert
//        /// </summary>
//        public string ActionType { get; set; }
//
//        /// <summary>
//        /// �洢���̲���
//        /// </summary>
//        public StoredProcedureParams StoredProcedureParams { get; set; }
//
//        /// <summary>
//        /// ��������
//        /// </summary>
//        public SelectAcionParams SelectAcionParams { get; set; } //
//
//        /// <summary>
//        /// ���������ʵ��������
//        /// </summary>
//        public string ActionObjectName { get; set; } // 
//
//        /// <summary>
//        /// ������������л��ַ���
//        /// </summary>
//        public string ActionObjectEntryStr { get; set; } // 
//
//        /// <summary>
//        /// �ֶι���
//        /// </summary>
//        public FieldFilter FieldFilter { get; set; }
//    }

    public class StoredProcedureParams
    {
        public string StoredProcedureName { get; set; }
//        public List<string> ParamsName { get; set; }
//        public List<string> ParamsValue { get; set; }
        public List<STParamete> Params { get; set; }
        public List<OutputValue> OutputValues { get; set; }
    }

    /// <summary>
    /// ѡ���ʱ��ָ����
    /// </summary>
    public class FieldFilter
    {
        /// <summary>
        /// �������������,�����������ֶβ�����
        /// </summary>
        public string[] BlackNames { get; set; }

        /// <summary>
        /// �������������,��ֻ�����������ֶ�,���һ���ֶ�(x)ͬʱ�����ڰ������ͺ�������,���Ժ�����Ϊ׼���������ֶ�
        /// </summary>
        public string[] WhiteNames { get; set; }
    }
}
