using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFw.Tool
{
    /// <summary>
    /// 自动过期处理类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Overdue<T>
    {
        T _value;
        public T Value
        {
            get
            {
                if (_value == null || (DateTime.Now - LastTime).TotalSeconds > Second)
                {
                    _value = GetObject();
                    LastTime = DateTime.Now;
                }
                return _value;
            }
        }
        private Func<T> GetObject;
        private int Second;
        private DateTime LastTime = DateTime.MinValue;
        public Overdue(Func<T> getObject,int second)
        {
            GetObject = getObject;
            Second = second;
        }

        public void Refresh()
        {
            LastTime = DateTime.MinValue;
        }
    }
}
