namespace SLControls.Editors
{
    /// <summary>
    /// 复合控件数据操作事件参数
    /// </summary>
    public class MultiControlDataEventArgs
    {
        public MultiControlDataEventArgs(string entityName, DataOperatorType operatorType)
        {
            EntityName = entityName;
            OperatorType = operatorType;
        }

        public string EntityName { get; private set; }
        public DataOperatorType OperatorType { get; private set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DataOperatorType
    {
        Update,
        Select,
        Insert,
        Delete,
    }
}