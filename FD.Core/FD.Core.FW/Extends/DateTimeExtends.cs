using System;


namespace Fw.Extends
{
    public static class DateTimeExtends
    {
        public static DateTime? ToDate(this string str)
        {
            try
            {
                return DateTime.Parse(str);
            }
            catch
            {
                return null;
            }
        }
    }
}
