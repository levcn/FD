using System;
using System.Windows.Forms;


namespace ProjectCreater.Settings
{
    public class BasePageHabitConfig
    {
        public BasePageHabitConfig(UserHabit mainHabit)
        {
            MainHabit = mainHabit;
        }

        protected UserHabit MainHabit { get; set; }

        public int GetInt(string key,int defaultValue=0)
        {
            return MainHabit.IntValues.GetValue(key, defaultValue);
        }
        public void SetInt(string key, int value = 0)
        {
            MainHabit.IntValues.SetValue(key, value);
        }
        public string GetStr(string key, string defaultValue = "")
        {
            return MainHabit.StrValues.GetValue(key, defaultValue);
        }
        public void SetStr(string key, string value = "")
        {
            MainHabit.StrValues.SetValue(key, value);
        }
        public DateTime GetDateTime(string key, DateTime defaultValue )
        {
            return MainHabit.DateValues.GetValue(key, defaultValue);
        }
        public void SetDateTime(string key, DateTime value )
        {
            MainHabit.DateValues.SetValue(key, value);
        }
        public double GetDouble(string key, double defaultValue = 0)
        {
            return MainHabit.DoubleValues.GetValue(key, defaultValue);
        }
        public void SetInt(string key, double value = 0)
        {
            MainHabit.DoubleValues.SetValue(key, value);
        }
        public long GetLong(string key, long defaultValue = 0)
        {
            return MainHabit.LongValues.GetValue(key, defaultValue);
        }
        public void SetLong(string key, long value = 0)
        {
            MainHabit.LongValues.SetValue(key, value);
        }
    }
}