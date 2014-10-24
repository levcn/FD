using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Collection
{
    public class TList<T> : List<T>
    {
        public TList()
        {

        }
        public TList(IEnumerable<T> list)
            : base(list ?? new List<T>())
        {

        }
        object flag = new object();
        public new void Add(T t)
        {
            lock (flag)
            {
                base.Add(t);
            }
        }
        public new void AddRange(IEnumerable<T> collection)
        {
            lock (flag)
            {
                base.AddRange(collection);
            }
        }
        public new bool Contains(T t)
        {
            lock (flag)
            {
                return base.Contains(t);
            }
        }
        public new T this[int index]
        {
            get
            {
                lock (flag)
                {
                    return base[index];
                }
            }
        }
        public new void Clear()
        {
            lock (flag)
            {
                base.Clear();
            }
        }
        public new void Remove(T t)
        {
            lock (flag)
            {
                if (base.Contains(t)) base.Remove(t);
            }
        }

        /// <summary>
        /// 线程安全的删除
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public new int RemoveAll(Predicate<T> match)
        {
            lock (flag)
            {
                return base.RemoveAll(match);
            }
        }
        /// <summary>
        /// 如果返回fase,退出循环
        /// </summary>
        /// <param name="func"></param>
        public void LockEach(Func<T, bool> func)
        {
            LockEach((item, index) => func(item));
        }

        public void LockBlock(Action<TList<T>> action)
        {
            lock (flag)
            {
                action(this);
            }
        }
        /// <summary>
        /// 如果返回fase,退出循环
        /// </summary>
        /// <param name="func"></param>
        public void LockEach(Func<T, int, bool> func)
        {
            lock (flag)
            {
                int index = 0;
                foreach (var item in this)
                {
                    if (!func(item, index++))
                    {
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 如果返回fase,退出循环
        /// </summary>
        /// <param name="func"></param>
        public List<T> ToLockList()
        {
            lock (flag)
            {
                return base.ToArray().ToList();
            }
        }

        /// <summary>
        /// 线程安全的
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Func<T, bool> predicate)
        {
            lock (flag)
            {
                foreach (T source1 in this)
                {
                    if (predicate(source1))
                        return source1;
                }
                return default(T);
            }
        }
    }
}
