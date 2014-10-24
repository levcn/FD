using System;
using System.Collections.Generic;
using System.Linq;
using Fw.Threads;
using NPOI.SS.Formula;


namespace Fw.Caches
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public static class CacheHelper
    {
        static CacheHelper()
        {
            ThreadHelper.StartThread(CheckOverDate,1000);
        }
        public static object cacheFlag = new object();
        public static void DeleteCacheWithKey(Func<string, bool> func)
        {
            
        }
        /// <summary>
        /// 心跳执行线程,触发缓存过期事件,移除过期的内容
        /// </summary>
        private static void CheckOverDate()
        {
            lock (cacheFlag)
            {
                Cache.Keys
                        .ToList()
                        .ForEach(w =>
                        {
                            var item = Cache[w];
                            if (item != null)
                            {
                                if (item.CheckOverDateWithEvent())
                                {
                                    if (Cache.ContainsKey(w)) Cache.Remove(w);
                                }
                            }
                        });
            }
        }

        private static readonly Dictionary<string, CacheItem> Cache = new Dictionary<string, CacheItem>();

        /// <summary>
        /// 返回是否有指定的键存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            lock (cacheFlag)
            {
                return Cache.ContainsKey(key);
            }
        }

        /// <summary>
        /// 添加一个缓存条目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        public static void Add(string key,object o)
        {
            Add(key, o, TimeSpan.MaxValue);
        }

        /// <summary>
        /// 添加一个缓存条目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="overDate"></param>
        public static void Add(string key,object o,TimeSpan overDate)
        {
            Add(key, o, overDate, null);
        }

        /// <summary>
        /// 添加一个缓存条目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="overDate"></param>
        /// <param name="overdateBeforeHandler"></param>
        public static void Add(string key,object o,TimeSpan overDate ,Action<OverDateEventArgs> overdateBeforeHandler)
        {
            var ci = new CacheItem { Content = o, OverDate =overDate == TimeSpan.MaxValue?DateTime.MaxValue: DateTime.Now.AddSeconds(overDate.TotalSeconds) };
            if(overdateBeforeHandler!=null)
            {
                ci.OverDateBefore += (s, e) =>
                                         {
                                             overdateBeforeHandler(e);
                                             if(e.Cancel)
                                             {
                                                 ci.OverDate = e.NewOverDate;
                                             }
                                         };
            }
            lock (cacheFlag)
            {
                Cache[key] = ci;
            }
        }
        public static T Get<T>(string key) where T : class
        {
            lock (cacheFlag)
            {
                if (ContainsKey(key))
                {
                    var o = Cache[key];
                    return o.Content as T;
                }
                return null;
            }
        }

        /// <summary>
        /// 返回指定的缓存,如果缓存不存在,使用指定的方法获取数据并缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T GetAdd<T>(string key, Func<T> func) where T : class
        {
            return GetAdd<T>(key, func, TimeSpan.MaxValue);
        }

        /// <summary>
        /// 返回指定的缓存,如果缓存不存在,使用指定的方法获取数据并缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="overDate"></param>
        /// <returns></returns>
        public static T GetAdd<T>(string key, Func<T> func, TimeSpan overDate) where T : class
        {
            if (ContainsKey(key))
            {
                return Get<T>(key);
            }
            else
            {
                T re = func();
                Add(key, re, overDate);
                return re;
            }
        }

        public static void Remove(string key)
        {
            lock (key)
            {
                if (ContainsKey(key))
                {
                    Cache.Remove(key);
                }
            }
        }
        public static void Clear()
        {
            lock (cacheFlag)
            {
                Cache.Clear();
            }
        }
    }
}
