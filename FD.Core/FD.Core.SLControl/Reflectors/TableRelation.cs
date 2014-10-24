namespace StaffTrain.FwClass
{
    /// <summary>
    /// 记录表于表的关系
    /// </summary>
    public class TableRelation
    {
        public string TableName1 { get; set; }
        public string TableName2 { get; set; }

        public string Column1 { get; set; }
        public string Column2 { get; set; }
    }
}
