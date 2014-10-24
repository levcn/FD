using System;


namespace Fw.Entity
{
    /// <summary>
    /// ��������
    /// </summary>
    public class SearchEntry
    {
        /// <summary>
        /// ��ѯ����(������ͬ��ʹ��Or,��ͬ��ʹ��And,δ��������Ĭ��Ϊ����һ��)
        /// </summary>
        public string GroupName;
        /// <summary>
        /// ����
        /// </summary>
        public string ColumnName; //����
        /// <summary>
        /// �߼�����
        /// </summary>
        public string Flag = " = "; //�߼�����
        public string value;
        public string GetSearchValue()
        {
            if (Flag.IndexOf("in", StringComparison.OrdinalIgnoreCase) == -1)
            {
                var v = value;
                bool removed = false;
                if (value.Length > 2 && value.StartsWith("'") && value.EndsWith("'"))
                {
                    v = value.Substring(1, value.Length - 2);
                    removed = true;
                }
                v = v.Replace("'", "''");
                if (removed)
                {
                    v = string.Format("'{0}'",v);
                }
                return v;
            }
            return value;
        }
    }
}