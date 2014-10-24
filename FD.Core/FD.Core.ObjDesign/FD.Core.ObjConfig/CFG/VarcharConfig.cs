namespace STComponse.CFG
{
    /// <summary>
    /// 字符串配置
    /// </summary>
    public class VarcharConfig
    {
        /// <summary>
        /// 有效长度(默认100)
        /// </summary>
        public int Length { get; set; }

        public static implicit operator VarcharConfig(string str)
        {
            return new VarcharConfig { Length = str.ToInt(100) };
        }

        public static implicit operator string(VarcharConfig vc)
        {
            return vc.Length.ToString();
        }
    }
}