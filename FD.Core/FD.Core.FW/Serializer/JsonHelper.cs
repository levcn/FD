using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
//using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using fastJSON;


namespace Fw.Serializer
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonSerilizerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Culture = CultureInfo.GetCultureInfo("en-us")
        };

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            return ToJson(t);
            //so
            //            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            //            MemoryStream ms = new MemoryStream();
            //            ser.WriteObject(ms, t);
            //            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            //            ms.Close();
            //            //替换Json的Date字符串
            //            const string p = @"\\/Date\((\d+)\+\d+\)\\/";
            //            MatchEvaluator matchEvaluator = ConvertJsonDateToDateString;
            //            Regex reg = new Regex(p);
            //            jsonString = reg.Replace(jsonString, matchEvaluator);
            //            return jsonString;
        }
        public static string JsonSerializer(object t)
        {
            return JsonSerializer(t, t.GetType());
        }
        public static string JsonSerializer(object t, Type type)
        {

            return ToJson(t);
            //            var ty = t.GetType();
            //            DataContractJsonSerializer ser = new DataContractJsonSerializer(ty);
            //            MemoryStream ms = new MemoryStream();
            //            try
            //            {
            //                ser.WriteObject(ms, t);
            //            }
            //            catch (Exception e)
            //            {
            //                throw;
            //            }
            //            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            //            ms.Close();
            //            //替换Json的Date字符串
            //            const string p = @"\\/Date\((\d+)\+\d+\)\\/";
            //            MatchEvaluator matchEvaluator = ConvertJsonDateToDateString;
            //            Regex reg = new Regex(p);
            //            jsonString = reg.Replace(jsonString, matchEvaluator);
            //            return jsonString;
        }
        /// <summary>
        /// 从一个对象信息生成Json串
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        static string ToJson(object obj)
        {
            return FastJsonSerializer(obj);
            var fullName = obj.GetType().FullName;
            if (fullName.Contains("<>f__AnonymousType"))
            {
                //匿名类型
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                return serializer.Serialize(obj);
            }
            else
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                string json;
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, obj);
                    json = Encoding.UTF8.GetString(stream.ToArray());
                    stream.Close();
                }
                return json;
            }
        }
        public static T JsonDeserialize<T>(string jsonString)
        {
            return (T)JsonDeserialize(jsonString, typeof(T));
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserializeByMs<T>(string jsonString)
        {
            return (T)JsonDeserializeByMs(typeof(T), jsonString);
        }
        public static string JsonSerializerByMs<T>(T t)
        {
            return JsonSerializerByMs(typeof(T), t);
        }
        public static object JsonDeserializeByMs(Type type, string jsonString)
        {
            return FastJsonDeserialize(jsonString,type);
            if (string.IsNullOrEmpty(jsonString)) return null;
            //Regex reg = new Regex(p);
            //jsonString = reg.Replace(jsonString, matchEvaluator);
            var ser = new DataContractJsonSerializer(type);

            try
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return ser.ReadObject(ms);
            }
            catch
            {
                return null;
            }
        }
        public static string JsonSerializerByMs(Type type, object t)
        {
            var ser = new DataContractJsonSerializer(type);
            var ms = new MemoryStream();
            ser.WriteObject(ms, t);
            var bytes = ms.ToArray();
            var jsonString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            ms.Close();
            //替换Json的Date字符串
            const string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = ConvertJsonDateToDateString;
            var reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static object JsonDeserialize(string jsonString, Type type)
        {
            try
            {
                return FastJsonDeserialize(jsonString, type);
                //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
                const string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
                MatchEvaluator matchEvaluator = ConvertDateStringToJsonDate;
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return ser.ReadObject(ms);
            }
            catch (Exception c)
            {
                return null;
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


        public static T Clone<T>(T t)
        {
            return JsonDeserialize<T>(JsonSerializer(t));
        }

        #region fast

        public static string FastJsonSerializer(object t)
        {
            if (t is String)
            {
                return t as String;
            }
            return JSON.Instance.ToJSON(t, FastParameters);
//            return JsonConvert.SerializeObject(t);
        }

        private static JSONParameters fastParameters;
        static JSONParameters FastParameters
        {
            get
            {
                if (fastParameters == null)
                {
                    fastParameters = new JSONParameters
                    {
                        UsingGlobalTypes = false,
                        EnableAnonymousTypes = true,
                        IgnoreCaseOnDeserialize = false,
                        SerializeNullValues= false,
                        ShowReadOnlyProperties = false,
                        UseExtensions = true,
                        UseFastGuid = false,
                        UseOptimizedDatasetSchema = false,
                        UseUTCDateTime = DateTimeFormat.Default,
                    };
                }
                return fastParameters;
            }
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static object FastJsonDeserialize(string jsonString, Type type)
        {
            try
            {
                if (type == typeof (string)) return jsonString.Trim('"');
                if (type == typeof (decimal) ||
                    type == typeof (decimal?) ||
                    type == typeof (byte) ||
                    type == typeof (short) ||
                    type == typeof (float) ||
                    type == typeof (double) ||
                    type == typeof (int) ||
                    type == typeof (byte?) ||
                    type == typeof (short?) ||
                    type == typeof (float?) ||
                    type == typeof (double?) ||
                    type == typeof (int?))
                {
                    return Convert.ChangeType(jsonString.Trim('"'),type);
                };
                return JSON.Instance.ToObject(jsonString, type);
//                return JsonConvert.DeserializeObject(jsonString, type);
            }
            catch (Exception e)
            {
                return null;
            }

        }
        #endregion

        public static T FastJsonDeserialize<T>(string str) where T : class
        {
            return FastJsonDeserialize(str, typeof(T)) as T;
        }
    }
}
