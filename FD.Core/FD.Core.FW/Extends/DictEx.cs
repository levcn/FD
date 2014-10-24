using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFw.Extends
{
    public static class DictEx
    {
        /// <summary>
        /// 根据键删除内容
        /// </summary>
        /// <param name="func"></param>
        public static void RemoveByKey<TKey, TValue>(this Dictionary<TKey, TValue> owner, Func<TKey, bool> func)
        {
            owner.Keys.ToList().ForEach(w =>
            {
                if (func(w))
                {
                    owner.Remove(w);
                }
            });
        }
    }
}
