using System.Collections.Generic;
using Aspose.Words.Lists;


namespace Fw.Entity
{
    /// <summary>
    /// ���صĽ��
    /// </summary>
    public class ResultData
    {
        public ResultData()
        {
            OutputValues = new List<OutputValue>();
        }
        public bool HaveError = false;
        public string ErrorMsg; //������Ϣ
        public string DetailErrorMsg;
        public string FriendlyErrorMsg;//�Ѻô�����Ϣ
        public int AllRecord; //���м�¼��
        public int Record; //��Ӱ���¼��
        public string ObjectEntryStr; //ʵ���������
        public List<OutputValue> OutputValues { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class OutputValue
    {
        /// <summary>
        /// ֵ������
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string Name { get; set; }

        public string Value { get; set; }
    }
}