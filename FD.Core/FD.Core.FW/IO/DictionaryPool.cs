using System;
using System.Collections.Generic;
using System.Linq;
using Fw.IO;
using Fw.Threads;
using ServerFw.Collection;


namespace ServerFw.IO
{
    /// <summary>
    /// 字典管理器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryPool<TKey,TValue>
    {
        static object LockObject = new object();
        /// <summary>
        /// 一个数据类型一个字典
        /// </summary>
        static SerializableDictionary<string, DictionaryFile<TKey, TValue>> Current = new SerializableDictionary<string, DictionaryFile<TKey, TValue>>();
        /// <summary>
        /// 根据键,返回一个字典,如果没有该字典,则添加一个返回
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> GetDictionary(string key,string filePath)
        {
            lock (LockObject)
            {
                return GetDictionary(key, () =>
                {
                    return new DictionaryFile<TKey, TValue>(new SerializableDictionary<TKey, TValue>(), filePath);
                });
            }
            
        }

        /// <summary>
        /// 延迟5秒保存文件
        /// </summary>
        /// <param name="key"></param>
        public static void DelaySaveToFile(string key)
        {
            lock (LockObject)
            {
                if (Current.ContainsKey(key))
                {
                    var item = Current[key];
                    if (item != null)
                    {
                        ThreadHelper.DelayRun(item.Save, 1000, "DictionaryPool_" + key);
                    }
                }
            }
        }
        /// <summary>
        /// 延迟5秒保存文件
        /// </summary>
        /// <param name="key"></param>
        public static void SaveToFile(string key)
        {
            lock (LockObject)
            {
                if (Current.ContainsKey(key))
                {
                    var item = Current[key];
                    if (item != null)
                    {
                        item.Save();
                    }
                  }
            }
        }
        /// <summary>
        /// 延迟5秒保存文件
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            lock (LockObject)
            {
                if (Current.ContainsKey(key))
                {
                    var v = Current[key];
                    Current.Remove(key);
                    v.DeleteFile();
                }
            }
        }
        /// <summary>
        /// 返回指定类型,指定key的字典
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getFunc"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> GetDictionary(string key, Func<DictionaryFile<TKey, TValue>> getFunc)
        {
            lock (LockObject)
            {
                if (Current.ContainsKey(key))
                {
                    var item = Current[key];
                    if (item == null)
                    {
                        item = getFunc();
                        Current.Add(key, item);
                    }
                    return item.Content;
                }
                else
                {
                    var item = getFunc();
                    Current.Add(key, item);
                    return item.Content;
                }
            }
        }
    }
}