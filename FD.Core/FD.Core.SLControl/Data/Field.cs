namespace StaffTrain.SVFw.Data
{
    public class Field
    {
        //名称
        public string DisplayName { get; set; }
        //数据类型
        public DataType DataType { get; set; }
        //页面上的控件名称
        public string ControlName { get; set; }
        //最大长度
        public int? MaxLen { get; set; }
        //最小长度
        public int? MinLen { get; set; }
        //最大值
        public double? MaxValue { get; set; }
        //最小值
        public double? MinValue { get; set; }
        //正则
        public string RegexStr { get; set; }
        //必填
        public bool? Required { get; set; }
        //值
        public string Value { get; set; }
    }
}