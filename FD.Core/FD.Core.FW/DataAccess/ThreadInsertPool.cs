using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerFw.Collection;


namespace ServerFw.DataAccess
{
    /// <summary>
    /// 数据查询缓冲池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class InsertThreadPool<T>
    {
        static TList<T> list = new TList<T>();
        public static void Add(T t)
        {
            list.Add(t);
        }
        public static void Remove(T t)
        {
            list.Remove(t);
        }
        public static List<T> ToList()
        {
            return list.ToLockList();
        }
    }
    /// <summary>
    /// 数据查询缓冲池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class UpdateDBThreadPool<T>
    {
        static TList<T> list = new TList<T>();
        public static void Add(T t)
        {
            list.Add(t);
        }
        public static void Remove(T t)
        {
            list.Remove(t);
        }
        public static List<T> ToList()
        {
            return list.ToLockList();
        }
    }
}
