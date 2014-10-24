using System.Collections.Generic;
using System.Text;
using System.Windows.Browser;


namespace StaffTrain.FwClass.DataClientTools
{
    
    /// <summary>
    /// Post数据格式的封装类
    /// </summary>
    public class PostArgs
    {
        Dictionary<string, string> values;
        public PostArgs()
        {
            this.values = new Dictionary<string, string>{};
        }
        public string this[string index]
        {
            get { return this.values[index]; }
            set { this.values[index] = value; }
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            var em = this.values.GetEnumerator();
            bool first = true;
            while (em.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    result.Append('&');
                }
                result.Append(em.Current.Key);
                result.Append('=');
                result.Append(HttpUtility.UrlEncode(em.Current.Value));
            }
            return result.ToString();
        }
    }
}
