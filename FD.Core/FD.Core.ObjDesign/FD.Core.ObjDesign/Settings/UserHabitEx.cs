using System;
using System.Collections.Generic;


namespace ProjectCreater.Settings
{
    public static class UserHabitEx
    {
        /// <summary>
        /// 返回一个Str
        /// </summary>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this UserHabit habit, string key, DateTime defaultValue)
        {
            return habit.DateValues.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 设置一个Str
        /// </summary>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetDateTime(this UserHabit habit, string key, DateTime value)
        {
            habit.DateValues.SetValue(key, value);
        }

        /// <summary>
        /// 返回一个Str
        /// </summary>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetStr(this UserHabit habit, string key, string defaultValue = null)
        {
            var re = habit.StrValues.GetValue(key, null);
            if (re!=null) return re;
            return defaultValue;
        }

        /// <summary>
        /// 设置一个Str
        /// </summary>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetInt(this UserHabit habit, string key, string value = null)
        {
            habit.StrValues.SetValue(key, value);
        }

        /// <summary>
        /// 返回一个Int
        /// </summary>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(this UserHabit habit, string key, int defaultValue = 0)
        {
            return habit.IntValues.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 设置一个Int
        /// </summary>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetInt(this UserHabit habit, string key, int value = 0)
        {
            habit.IntValues.SetValue(key, value);
        }

        /// <summary>
        /// 返回列表中指定位置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string,T> habit, string key, T defaultValue) where T:class 
        {
            if (habit.ContainsKey(key))
            {
                return habit[key];
            }
            return default (T);
        }

        /// <summary>
        /// 设置列表中指定位置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(this Dictionary<string, T> habit, string key, T value) where T : class
        {
            habit[key] = value;
//            if (habit.ContainsKey(key))
//            {
//                habit[index] = value;
//            }
//            else
//            {
//                while(habit.Count<=index)
//                {
//                    habit.Add(default(T));
//                }
//                habit[index] = value;
//            }
        }

        /// <summary>
        /// 返回列表中指定位置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, T?> habit, string key, T defaultValue) where T : struct 
        {
            if (habit.ContainsKey(key))
            {
                var structValue = habit[key];
                if(structValue.HasValue)return structValue.Value;
                return defaultValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置列表中指定位置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="habit"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(this Dictionary<string, T?> habit, string key, T value) where T : struct
        {
            habit[key] = value;
            //            if (habit.Count > index)
            //            {
            //                habit[index] = value;
            //            }
            //            else
            //            {
            //                while (habit.Count <= index)
            //                {
            //                    habit.Add(default(T));
            //                }
            //                habit[index] = value;
            //            }
        }
    }
}