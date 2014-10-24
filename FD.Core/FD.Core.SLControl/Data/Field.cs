namespace StaffTrain.SVFw.Data
{
    public class Field
    {
        //����
        public string DisplayName { get; set; }
        //��������
        public DataType DataType { get; set; }
        //ҳ���ϵĿؼ�����
        public string ControlName { get; set; }
        //��󳤶�
        public int? MaxLen { get; set; }
        //��С����
        public int? MinLen { get; set; }
        //���ֵ
        public double? MaxValue { get; set; }
        //��Сֵ
        public double? MinValue { get; set; }
        //����
        public string RegexStr { get; set; }
        //����
        public bool? Required { get; set; }
        //ֵ
        public string Value { get; set; }
    }
}