using System;
using System.Collections.Generic;


namespace ServerFw.Reflection
{
    /// <summary>
    /// 反射数据缓存,永久缓存,没有过期时间
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class RCache<TKey,TValue>
    {
        static Dictionary<TKey, Dictionary<string, TValue>> All = new Dictionary<TKey, Dictionary<string, TValue>>();
        static readonly object LockObject = new object();

        /// <summary>
        /// 返回指定的缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="getvalue">值</param>
        /// <returns></returns>
        public static TValue GetValue(TKey key, Func<TValue> getvalue)
        {
            return GetValue(key, "", getvalue);
        }

        /// <summary>
        /// 返回指定的缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="subKey">子键(默认为"")</param>
        /// <param name="getvalue">值</param>
        /// <returns></returns>
        public static TValue GetValue(TKey key,string subKey, Func<TValue> getvalue)
        {
            if (subKey == null) subKey = "";
            lock (LockObject)
            {
                TValue v;
                if (!All.ContainsKey(key))
                {
                    v = getvalue();
                    var v1 = new Dictionary<string, TValue> {{subKey, v}};
                    if (!All.ContainsKey(key))//有可能getvalue方法中会再次访问该方法,这时会把key添加到All中(这种情况在同一线程的发生)
                    {
                        All.Add(key, v1);
                    }
                }
                else
                {
                    var valueDic = All[key];
                    if (!valueDic.ContainsKey(subKey))
                    {
                        v = getvalue();
                        valueDic.Add(subKey, v);
                    }
                    else
                    {
                        v = valueDic[subKey];
                    }
                }
                return v;

            }
        }
    }
}
