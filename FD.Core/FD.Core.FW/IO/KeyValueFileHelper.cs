using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fw.Serializer;


namespace Fw.IO
{
    /// <summary>
    /// 键值文件管理类
    /// </summary>
    public class KeyValueFileHelper
    {
        /// <summary>
        /// 保存文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ht"></param>
        static void SetFileContent(string path,Hashtable ht)
        {
            FileHelper.CreateFolderByFilePath(path);
            File.WriteAllText(path,JsonHelper.JsonSerializer(ht),Encoding.UTF8);
        }
        static Hashtable GetFileContent(string path)
        {
            if (!File.Exists(path)) return new Hashtable();
            var text = File.ReadAllText(path);
            try
            {
                return JsonHelper.JsonDeserialize<Hashtable>(text);
            }
            catch
            {
                return new Hashtable();
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keys"></param>
        public static void DeleteValue(string path, List<string> keys)
        {
            var ht = GetFileContent(path);
            keys.ToList().ForEach(
                    w =>
                        {
                            if (ht.ContainsKey(w))
                            {
                                ht.Remove(w);
                            }
                        });
            SetFileContent(path, ht);
        }

        /// <summary>
        /// 返回人员信息的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string path, string key)
        {
            var ht = GetFileContent(path);
            if (ht.ContainsKey(key))
            {
                return ht[key].ToString();
            }
            return null;
        }

        /// <summary>
        /// 设置人员信息的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetValue(string path, string key, string value)
        {
            FileHelper.CreateFolderByFilePath(path);
            var ht = GetFileContent(path);
            ht[key] = value;
            SetFileContent(path, ht);
            return null;
        }

        /// <summary>
        /// 返回人员信息的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Hashtable GetValues(string path, List<string> key)
        {
            var re = new Hashtable();
            var ht = GetFileContent(path);
            key.ForEach(z =>
                            {
                                if (ht.ContainsKey(z))
                                {
                                    re[z] = ht[z];
                                }
                            });
            return re;
        }

        /// <summary>
        /// 设置人员信息的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string SetUserValues(string path, Hashtable table)
        {
            FileHelper.CreateFolderByFilePath(path);
            var ht = GetFileContent(path);
            table.Keys.Cast<string>().ToList().ForEach(
                    w =>
                        {
                            ht[w] = table[w];
                        });
            SetFileContent(path, ht);
            return null;
        }
    }
}
