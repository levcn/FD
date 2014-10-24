namespace STComponse.CFG
{
    /// <summary>
    /// 小数配置
    /// </summary>
    public class DecimalConfig
    {
        /// <summary>
        /// 小数位
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 有效长度
        /// </summary>
        public int Length { get; set; }

        public static implicit operator DecimalConfig(string str)
        {
            str = str ?? "";
            var strs = str.Split(',', '，');
            var re =  new DecimalConfig
            {
                Length = strs[0].ToInt(10),
            };
            if (strs.Length>=2)
            {
                re.Precision = strs[1].ToInt(2);
            }
            return re;
        }

        public static implicit operator string(DecimalConfig dc)
        {
            return string.Format("{0},{1}", dc.Length, dc.Precision);
        }
    }
}