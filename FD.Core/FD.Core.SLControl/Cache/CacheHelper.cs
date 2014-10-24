using System;
using System.Collections.Generic;
using System.Linq;
using Fw.Caches;
using SLControls.Threads;


namespace SLControls.Cache
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public static class CacheHelper
    {
        static CacheHelper()
        {
            ThreadHelper.StartThread(CheckOverDate, 1000);
        }
        public static object cacheFlag = new object();

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="func"></param>
        public static void DeleteCacheWithKey(Func<string, bool> func)
        {
            lock (cacheFlag)
            {
                var keys = Cache.Keys.ToList();
                keys.Where(func).ToList().ForEach(w => Cache.Remove(w));
            }
        }
        //APP缓存键
        private const string AppStart = "APP_";
        //导航缓存键
        private const string NavigatorStart = "Navigator_";
        //页面缓存键
        private const string PageStart = "Page_";

        public static object GetAppCache(string key)
        {
            return Get(AppStart + key);
        }
        public static object GetNavigatorCache(string key)
        {
            return Get(NavigatorStart + key);
        }
        public static object GetPageCache(string key)
        {
            return Get(PageStart + key);
        }
        /// <summary>
        /// 添加APP缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppCache(string key, object value)
        {
            key = AppStart + key;
            Add(key,value);
        }

        /// <summary>
        /// 删除指定的APP缓存
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAppCache(string key)
        {
            key = AppStart + key;
            Remove(key);
        }

        /// <summary>
        /// 添加导航缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddNavigatorCache(string key, object value)
        {
            key = NavigatorStart + key;
            Add(key, value);
        }

        /// <summary>
        /// 删除指定的导航缓存
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveNavigatorCache(string key)
        {
            key = NavigatorStart + key;
            Remove(key);
        }

        /// <summary>
        /// 添加页面缓存
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddPageCache(string pageName,string key, object value)
        {
            key = PageStart + pageName + key;
            Add(key, value);
        }

        /// <summary>
        /// 删除指定的页面缓存
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="key"></param>
        public static void RemovePageCache(string pageName, string key)
        {
            key = PageStart + pageName+ key;
            Remove(key);
        }

        /// <summary>
        /// 清除页面缓存数据
        /// </summary>
        public static void ClearPageCache(string pageName)
        {
            var value = PageStart + pageName;
            DeleteCacheWithKey(w => w.StartsWith(value));
        }

        /// <summary>
        /// 清除导航缓存数据
        /// </summary>
        public static void ClearNavigatorCache()
        {
            DeleteCacheWithKey(w => w.StartsWith(NavigatorStart));
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
        public static void Add(string key, object o)
        {
            Add(key, o, TimeSpan.MaxValue);
        }

        /// <summary>
        /// 添加一个缓存条目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="overDate"></param>
        public static void Add(string key, object o, TimeSpan overDate)
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
        public static void Add(string key, object o, TimeSpan overDate, Action<OverDateEventArgs> overdateBeforeHandler)
        {
            var ci = new CacheItem { Content = o, OverDate = overDate == TimeSpan.MaxValue ? DateTime.MaxValue : DateTime.Now.AddSeconds(overDate.TotalSeconds) };
            if (overdateBeforeHandler != null)
            {
                ci.OverDateBefore += (s, e) =>
                {
                    overdateBeforeHandler(e);
                    if (e.Cancel)
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
//            lock (cacheFlag)
//            {
//                if (ContainsKey(key))
//                {
//                    var o = Cache[key];
//                    return o.Content as T;
//                }
//                return null;
//            }
            return Get<T>(key);
        }
        public static object Get(string key)
        {
            lock (cacheFlag)
            {
                if (ContainsKey(key))
                {
                    var o = Cache[key];
                    return o.Content;
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
            if (ContainsKey(key))
            {
                return Get<T>(key);
            }
            else
            {
                T re = func();
                Add(key, re);
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
