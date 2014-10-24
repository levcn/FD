using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.Collection
{
    public class NameValue<TKey, TValue>
    {
        public NameValue() { }
        public NameValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
