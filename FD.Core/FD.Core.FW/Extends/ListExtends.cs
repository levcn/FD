using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Fw.Entity;


namespace Fw.Extends
{
    /// <summary>
    /// 列表的扩展方法
    /// </summary>
    public static class ListExtends
    {
        static Random random = new Random();
        /// <summary>
        /// 随机选一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> content)
        {
            if (content != null && content.Count > 0)
            {
                return content[random.Next(0, content.Count)];
            }
            return default(T);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> content)
        {
            return new ObservableCollection<T>(content);
        }
        /// <summary>
        /// 返回搜索内容的字符串
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetHashString(this List<SearchEntry> content)
        {
            return content.Select(w => w.ColumnName + w.Flag + w.value).OrderBy(w => w).Serialize("");
        }

        /// <summary>
        /// 如果List中不存在item 往List中添加该项
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

        public static int Remove<T>(this List<T> content,List<T> items)
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
        public static int Remove<TSource>(this List<TSource> source,
                                                          Func<TSource, bool> predicate)
        {
            return source.Remove(source.Where(predicate).ToList());
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

//        /// <summary>
//        ///     转换成关键字字典
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="TKey"></typeparam>
//        /// <param name="list"></param>
//        /// <param name="groupBy"></param>
//        /// <param name="selector"></param>
//        /// <returns></returns>
//        public static SerializableDictionary<TKey, List<TKey>> ToSD<T, TKey>(this IEnumerable<T> list, Func<T, TKey> groupBy, Func<T, TKey> selector)
//        {
//            var r = new SerializableDictionary<TKey, List<TKey>>();
//            list.GroupBy(groupBy).ForEach(w => r.Add(w.Key, w.Select(selector).ToList()));
//            return r;
//        }

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

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> owner, Func<TKey, bool> getFunc)
        {
            var key = owner.Keys.ToList().FirstOrDefault(getFunc);
            if (!Equals(key, default(TKey)))
            {
                return owner[key];
            }
            return default(TValue);
        }
        public static void RemoveByKey<TKey, TValue>(this Dictionary<TKey, TValue> owner, Func<TKey, bool> getFunc)
        {
            owner.Keys.ToList().Where(getFunc).ToList().ForEach(w =>
            {
                if (owner.ContainsKey(w)) owner.Remove(w);
                
            });
        }
    }
}
