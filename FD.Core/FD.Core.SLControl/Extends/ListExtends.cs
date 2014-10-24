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
using SLControls.Generators;


namespace SLControls.Extends
{
    /// <summary>
    ///     列表的扩展方法
    /// </summary>
    public static class ListExtends
    {
        private static readonly Random Random = new Random();

        public static void ForEachIndex<T>(this IList<T> list, Action<T, int> selector)
        {
            int index = 0;
            list.ForEach(w => selector(w,index++));
        }
//        /// <summary>
//        ///     返回搜索内容的字符串
//        /// </summary>
//        /// <param name="content"></param>
//        /// <returns></returns>
//        public static string GetHashString(this List<SearchEntry> content)
//        {
//            return content.Select(w => w.ColumnName + w.Flag + w.value).OrderBy(w => w).Serialize("");
//        }

        /// <summary>
        ///     如果List中不存在item 往List中添加该项
        /// </summary>
        /// <param name="content"></param>
        /// <param name="item"> </param>
        /// <param name="equ"> 比较方法 </param>
        /// <returns></returns>
        public static void AddSingle<T>(this List<T> content, T item, Func<T, T, bool> equ = null)
        {
            if (equ == null)
            {
                equ = (a, b) => Equals(a, b);
            }
            if (!content.Any(w => equ(item, w)))
            {
                content.Add(item);
            }
        }

        public static bool HaveItem<T>(this List<T> content)
        {
            return (content != null && content.Count > 0);
        }

        /// <summary>
        ///     随机选一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> content)
        {
            if (content != null && content.Count > 0)
            {
                return content[Random.Next(0, content.Count)];
            }
            return default(T);
        }

        /// <summary>
        /// 洗牌算法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listtemp"></param>
        public static void Reshuffle<T>(this List<T> listtemp)
        { //随机交换
            var ram = new Random();
            for (var i = 0; i < listtemp.Count; i++)
            {
                var currentIndex = ram.Next(0, listtemp.Count - i);
                var tempValue = listtemp[currentIndex];
                listtemp[currentIndex] = listtemp[listtemp.Count - 1 - i];
                listtemp[listtemp.Count - 1 - i] = tempValue;
            }
        }

        public static int Remove1<T>(this List<T> content, List<T> items)
        {
            int count = 0;
            items.ForEach(w =>
            {
                if (content.Contains(w))
                {
                    content.Remove(w);
                    count++;
                }
            });
            return count;
        }

        public static int Remove1<TSource>(this List<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Remove1(source.Where(predicate).ToList());
        }

        /// <summary>
        ///     转换成关键字字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="list"></param>
        /// <param name="groupBy"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static SerializableDictionary<TKey, List<TKey>> ToSD<T, TKey>(this IEnumerable<T> list, Func<T, TKey> groupBy, Func<T, TKey> selector)
        {
            var r = new SerializableDictionary<TKey, List<TKey>>();
            list.GroupBy(groupBy).ForEach(w => r.Add(w.Key, w.Select(selector).ToList()));
            return r;
        }

        //        public static void ForEachNotNull<T>(this IOrderedEnumerable<T> list, Action<T> action)
        //        {
        //            list.ToList().ForEachNotNull(action);
        //        }
        /// <summary>
        ///     如果不是Null则遍历所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEachNotNull<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list != null)
            {
                List<T> l = list.ToList();
                l.ForEach(action);
            }
        }

        /// <summary>
        ///     如果不是Null则遍历所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="lastIndex"></param>
        public static void InsertLast<T>(this IList<T> list, T item, int lastIndex)
        {
            if (list != null)
            {
                var index = list.Count - lastIndex;
                list.Insert(index, item);
            }
        }
        /// <summary>
        /// 有,则返回指定值,没有,则返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetIfHave<TKey, TValue>(this IDictionary<TKey, TValue> thisList, TKey key, TValue defaultValue = default(TValue))
        {
            if (thisList.ContainsKey(key))
            {
                return thisList[key];
            }
            return defaultValue;
        }

        /// <summary>
        /// 先清除,再添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="newList"></param>
        public static void ClearAdd<T>(this IList<T> thisList, T t)
        {
            ClearAdd(thisList, new List<T> { t });
        }

        /// <summary>
        /// 先清除,再添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisList"></param>
        /// <param name="newList"></param>
        public static void ClearAdd<T>(this IList<T> thisList, IList<T> newList)
        {
            thisList.Clear();
            newList.ForEachNotNull(thisList.Add);
        }
    }
}
