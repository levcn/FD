using System;
using System.Collections.Generic;


namespace ServerFw.TMath
{
    /// <summary>
    ///     大于10进制的任意进制
    /// </summary>
    public struct CustomNumber : IEquatable<CustomNumber>, IComparable<CustomNumber>
    {
        private const string Bits = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_:";

        #region 字典

        private static readonly Dictionary<char, int> Dic = new Dictionary<char, int> {
                {'0', 0},
                {'1', 1},
                {'2', 2},
                {'3', 3},
                {'4', 4},
                {'5', 5},
                {'6', 6},
                {'7', 7},
                {'8', 8},
                {'9', 9},
                {'a', 10},
                {'b', 11},
                {'c', 12},
                {'d', 13},
                {'e', 14},
                {'f', 15},
                {'g', 16},
                {'h', 17},
                {'i', 18},
                {'j', 19},
                {'k', 20},
                {'l', 21},
                {'m', 22},
                {'n', 23},
                {'o', 24},
                {'p', 25},
                {'q', 26},
                {'r', 27},
                {'s', 28},
                {'t', 29},
                {'u', 30},
                {'v', 31},
                {'w', 32},
                {'x', 33},
                {'y', 34},
                {'z', 35},
                {'A', 36},
                {'B', 37},
                {'C', 38},
                {'D', 39},
                {'E', 40},
                {'F', 41},
                {'G', 42},
                {'H', 43},
                {'I', 44},
                {'J', 45},
                {'K', 46},
                {'L', 47},
                {'M', 48},
                {'N', 49},
                {'O', 50},
                {'P', 51},
                {'Q', 52},
                {'R', 53},
                {'S', 54},
                {'T', 55},
                {'U', 56},
                {'V', 57},
                {'W', 58},
                {'X', 59},
                {'Y', 60},
                {'Z', 61},
                {'_', 62},
                {':', 63},
        };

        #endregion


        /// <summary>
        ///     多少进制
        /// </summary>
        public readonly int BitCount;

        private long _longValue;
        private string _strValue;

        public CustomNumber(long l, int bitCount = 64)
                : this()
        {
            BitCount = bitCount;
            _longValue = l;
            _strValue = GetCustomNumber(l);
        }

//        public CustomNumber(string l)
        public CustomNumber(string l,int bitCount = 64)
                : this()
        {
            BitCount = bitCount;
            try
            {
                _longValue = GetNumber(l);
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("字符串不是正确的数字类型:" + l);
            }
            _strValue = l;
        }

        /// <summary>
        ///     数据值
        /// </summary>
        public long LongValue
        {
            get
            {
                return _longValue;
            }
            set
            {
                _longValue = value;
                _strValue = GetCustomNumber(value);
            }
        }

        /// <summary>
        ///     字符串值
        /// </summary>
        public string StrValue
        {
            get
            {
                return _strValue;
            }
            set
            {
                _strValue = value;
                _longValue = GetNumber(value);
            }
        }

        public bool Equals(CustomNumber other)
        {
            return BitCount == other.BitCount && _longValue == other._longValue && string.Equals(_strValue, other._strValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CustomNumber && Equals((CustomNumber) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = BitCount;
                hashCode = (hashCode*397) ^ _longValue.GetHashCode();
                hashCode = (hashCode*397) ^ (_strValue != null ? _strValue.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     数据转64进制
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string GetCustomNumber(long number)
        {
            long yushu = number;
            bool fu = yushu < 0;
            if (fu) yushu = -(yushu);
            int bitCount = BitCount;
            string r = "";
            while(yushu > 0)
            {
                long yu = yushu%bitCount;
                yushu /= bitCount;
                r = Bits[(int) yu] + r;
                if (yu == 0 && yushu == 0) break;
            }
            if (fu) r = "-" + r;
            return r;
        }

        private long GetNumber(string number)
        {
            bool fu = false;
            if (number.StartsWith("-"))
            {
                fu = true;
                number = number.TrimStart('-');
            }
            int d = BitCount;
            long f = 0L;
            for (int i = 0; i < number.Length; i++)
            {
                char c = number[number.Length - i - 1];
                int a = Dic[c];
                var current = (long) Math.Pow(d, i);
                f += a*current;
            }
            if (fu) f = -f;
            return f;
        }

        public override string ToString()
        {
            return StrValue;
        }

        #region 操作符重载

        public static bool operator ==(CustomNumber x, CustomNumber y)
        {
            //            var xNull = object.Equals(x, null);
            //            var yNull = object.Equals(y, null);
            //            return xNull && yNull || (!xNull && !yNull) && x.LongValue == y.LongValue;
            return x.LongValue == y.LongValue;
        }

        public static bool operator !=(CustomNumber x, CustomNumber y)
        {
            //            var xNull = object.Equals(x, null);
            //            var yNull = object.Equals(y, null);
            //            return xNull && yNull || (!xNull && !yNull) && x.LongValue != y.LongValue;
            return x.LongValue != y.LongValue;
        }

        public static CustomNumber operator +(CustomNumber x, CustomNumber y)
        {
            return new CustomNumber(x.LongValue + y.LongValue);
        }

        public static CustomNumber operator -(CustomNumber x, CustomNumber y)
        {
            return new CustomNumber(x.LongValue - y.LongValue);
        }

        public static CustomNumber operator *(CustomNumber x, CustomNumber y)
        {
            return new CustomNumber(x.LongValue*y.LongValue);
        }

        public static implicit operator string(CustomNumber x)
        {
            return x.StrValue;
        }

        public static implicit operator CustomNumber(string x)
        {
            return new CustomNumber(x ?? "");
        }

        public static implicit operator long(CustomNumber x)
        {
            return x.LongValue;
        }

        public static implicit operator CustomNumber(long x)
        {
            return new CustomNumber(x);
        }

        public static CustomNumber operator /(CustomNumber x, CustomNumber y)
        {
            return new CustomNumber(x.LongValue/y.LongValue);
        }

        #endregion

        public int CompareTo(CustomNumber other)
        {
            return this.LongValue.CompareTo(other);
        }
    }
}