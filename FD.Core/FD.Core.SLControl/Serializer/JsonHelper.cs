using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;


namespace StaffTrain.FwClass.Serializer
{
    public static class JsonHelper
    {
        public static string JsonSerializer<T>(T t)
        {
            return JsonSerializer(typeof (T), t);
        }

        public static string JsonSerializerByMs<T>(T t)
        {
            return JsonSerializerByMs(typeof(T), t);
        }
        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer(Type type,object t)
        {
            try
            {
                return fastJSON.JSON.Instance.ToJSON(t);
            }
            catch (Exception e)
            {
                
            }
            //so
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            byte[] bytes = ms.ToArray();
            string jsonString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            ms.Close();
            //替换Json的Date字符串
            const string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = ConvertJsonDateToDateString;
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }

        public static string JsonSerializerByMs(Type type, object t)
        {            
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            byte[] bytes = ms.ToArray();
            string jsonString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            ms.Close();
            //替换Json的Date字符串
            const string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = ConvertJsonDateToDateString;
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }
        

        ///// <summary>
        ///// 从一个对象信息生成Json串
        ///// </summary>
        ///// <param name="obj">转换对象</param>
        ///// <typeparam name="T">对象类型</typeparam>
        ///// <returns></returns>
        //public static string ToJson<T>(this T obj)
        //{
        //    var fullName = typeof(T).FullName;
        //    if (fullName != null && fullName.Contains("<>f__AnonymousType"))
        //    {
        //        //匿名类型
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        return serializer.Serialize(obj);
        //    }
        //    else
        //    {
        //        var serializer = new DataContractJsonSerializer(typeof(T));
        //        string json;
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            serializer.WriteObject(stream, obj);
        //            json = Encoding.UTF8.GetString(stream.ToArray());
        //            stream.Close();
        //        }
        //        return json;
        //    }
        //}
        public static object JsonDeserialize(Type type, string jsonString)
        {
            try
            {
                if (type == typeof (string)) return jsonString.Replace(@"\""", @"""").Trim('"');
                return fastJSON.JSON.Instance.ToObject(jsonString, type);
            }
            catch (Exception e)
            {
                
            }
            if (string.IsNullOrEmpty(jsonString)) return null;
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            const string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = ConvertDateStringToJsonDate;
            //Regex reg = new Regex(p);
            //jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);

            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return ser.ReadObject(ms);
            }
            catch
            {
                return null;
            }
        }

        public static object JsonDeserializeByMs(Type type, string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString)) return null;
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            const string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = ConvertDateStringToJsonDate;
            //Regex reg = new Regex(p);
            //jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);

            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return ser.ReadObject(ms);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T TryDeserialize<T>(string jsonString)
        {
            try
            {
                return (T) JsonDeserialize(typeof (T), jsonString);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            return (T)JsonDeserialize(typeof(T), jsonString);
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserializeByMs<T>(string jsonString)
        {
            return (T)JsonDeserializeByMs(typeof(T), jsonString);
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T TryJsonDeserialize<T>(string jsonString)
        {
            try
            {
                return (T) JsonDeserialize(typeof (T), jsonString);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            string result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            string result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
        public static object Clone(object t)
        {
            return JsonDeserialize(t.GetType(), JsonSerializer(t.GetType(), t));
        }
        //public static T Clone<T>(T t)
        //{
        //    return JsonDeserialize<T>(JsonSerializer(t));
        //}
    }
}
