using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace Fw.Serializer
{
    public class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            Type type = o.GetType();
            XmlSerializer serializer = new XmlSerializer(type); 

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }
        /// <summary>
        /// 返回对T的XML序列化结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetXmlSerialize(object o)
        {
            if (o == null) return "";
            var encoding = Encoding.UTF8;
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
//            string re = "";
//            try
//            {
//                var ser = new XmlSerializer(t.GetType());
//                using (var ms = new MemoryStream())
//                using (var sw = new StreamWriter(ms))
//                using (var sr = new StreamReader(ms))
//                {
//                    ser.Serialize(sw, t);
//                    ms.Position = 0;
//                    re = sr.ReadToEnd();
//                }
//            }
//            catch (Exception e)
//            {
//                var r = e.Message;
//            }
//            return re;
        }
        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }
        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(path, encoding);
            return XmlHelper.GetXmlDeserialize<T>(xml, encoding);
        }
        /// <summary>
        /// 返回对T的XML序列化结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetXmlSerialize<T>(T t)
        {
            return GetXmlSerialize((object)t);
        }

        /// <summary>
        /// 对XML反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static object GetXmlDeserialize(string xmlString,Type type)
        {
            return GetXmlDeserialize(xmlString,type, Encoding.UTF8);
        }

        /// <summary>
        /// 对XML反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static object GetXmlDeserialize(string xmlString,Type type, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlString)) return null;
            MemoryStream ms = new MemoryStream(encoding.GetBytes(xmlString));
            XmlSerializer xml = new XmlSerializer(type);
            object re = null;
            try
            {
                re = xml.Deserialize(ms); //反序列化为ChatMessage对象
            }
            catch (Exception ex)
            {
                re = null;
            }
            return re;
        }
        //public static T GetXmlDeserialize<T>(string xmlString) where T : class
        //{
        //    return GetXmlDeserialize<T>(xmlString, Encoding.UTF8);
        //}
        public static T GetXmlDeserialize<T>(string xmlString, Encoding encoding)
        {
            return (T)GetXmlDeserialize(xmlString, typeof(T), encoding);
        }
        public static void SaveToFile<T>(string path, T t) where T : class
        {
            File.WriteAllText(path, SaveToStr(t),Encoding.UTF8);
        }

        public static string SaveToStr<T>(T t)
        {
            return GetXmlSerialize<T>(t);
        }


        public static T LoadFromStr<T>(string str) where T : class
        {
            return GetXmlDeserialize<T>(str);
        }

        public static T GetXmlDeserialize<T>(string str) where T : class
        {
            return GetXmlDeserialize<T>(str, Encoding.UTF8);
        }

        public static T LoadFromFile<T>(string path) where T : class
        {
            T t = null;
            if (File.Exists(path))
            {
                return LoadFromStr<T>(File.ReadAllText(path,Encoding.UTF8));
            }
            else
            {
                return t;
            }


        }
    }
}
