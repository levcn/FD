using System.Collections.Generic;


namespace ServerFw.IO
{
    public class NameValue<TKey, TValue>
    {
        protected bool Equals(NameValue<TKey, TValue> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Key, other.Key) && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TKey>.Default.GetHashCode(Key)*397) ^ EqualityComparer<TValue>.Default.GetHashCode(Value);
            }
        }

        public NameValue() { }
        public NameValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NameValue<TKey, TValue>) obj);
        }
    }
}