using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;


namespace StaffTrain.FwClass.Serializer
{
    public class XmlHelper 
    {
        /// <summary>
        /// 返回对T的XML序列化结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetXmlSerialize<T>(T t)
        {
            string re = "";
            try
            {
                var ser = new XmlSerializer(typeof(T));
                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms))
                using (var sr = new StreamReader(ms))
                {
                    ser.Serialize(sw, t);
                    ms.Position = 0;
                    re = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                var r = e.Message;
            }
            return re;
        }
        /// <summary>
        /// 返回对T的XML序列化结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetXmlSerialize(object t)
        {
            if (t == null) return "";
            string re = "";
            try
            {
                var type = t.GetType();
                var ser = new XmlSerializer(type);
                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms))
                using (var sr = new StreamReader(ms))
                {
                    ser.Serialize(sw, t);
                    ms.Position = 0;
                    re = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                var r = e.Message;
            }
            return re;
        }
        /// <summary>
        /// 对XML反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T GetXmlDeserialize<T>(string xmlString) where T : class
        {
            return GetXmlDeserialize<T>(xmlString, Encoding.UTF8);
        }

        /// <summary>
        /// 对XML反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T GetXmlDeserialize<T>(string xmlString, Encoding encoding) where T : class
        {
            if (string.IsNullOrEmpty(xmlString)) return default(T);
            MemoryStream ms = new MemoryStream(encoding.GetBytes(xmlString));
            XmlSerializer xml = new XmlSerializer(typeof(T));
            T re = null;
            try
            {
                re = xml.Deserialize(ms) as T; //反序列化为ChatMessage对象
            }
            catch (Exception ex)
            {
                re = null;
            }
            return re;
        }

        public static void SaveToFile<T>(string path, T t) where T : class
        {
            File.WriteAllText(path, SaveToStr(t));
        }

        public static string SaveToStr<T>(T t)
        {
            return GetXmlSerialize<T>(t);
        }


        public static T LoadFromStr<T>(string str) where T : class
        {
            return GetXmlDeserialize<T>(str);
        }

        public static T LoadFromFile<T>(string path) where T : class
        {
            T t = null;
            if (File.Exists(path))
            {
                return LoadFromStr<T>(File.ReadAllText(path));
            }
            else
            {
                return t;
            }


        }
    }
}
