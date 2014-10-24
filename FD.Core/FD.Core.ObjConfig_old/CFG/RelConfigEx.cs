namespace STComponse.CFG
{
    public static class RelConfigEx
    {
        public static string ToDictStr(this RelConfig str)
        {
            return string.Format("{0}({1},{2})", str.DictTableName, str.DictKey, str.DictShowName);
        }
    }
}