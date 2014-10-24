using System.Collections.Generic;


namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// �Զ�������
    /// </summary>
    public class CustomCmd
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public List<string> Params { get; set; }
        public List<string> ParamTypes { get; set; }

        /// <summary>
        /// �����Ƿ�ʹ����������ռ�
        /// </summary>
        public bool IsRelativePath { get; set; }
    }
}