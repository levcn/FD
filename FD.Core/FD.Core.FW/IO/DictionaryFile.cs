using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fw.IO;
using Fw.Serializer;
using ServerFw.Collection;


namespace ServerFw.IO
{
    /// <summary>
    /// 字典文件
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryFile<TKey,TValue>
    {
        public DictionaryFile(SerializableDictionary<TKey, TValue> content, string filePath)
        {
            Content = content;
            FilePath = filePath;
            Load();
        }

        public SerializableDictionary<TKey, TValue> Content { get; set; }
        public DateTime LastSaveTime { get; set; }
        public string FilePath { get; set; }
        public void Load()
        {
            if (File.Exists(FilePath))
            {
                var txt = File.ReadAllText(FilePath);
                var obj = XmlHelper.GetXmlDeserialize<SerializableDictionary<TKey, TValue>>(txt) ?? new SerializableDictionary<TKey, TValue>();
                Content = obj;
            }
        }
        static object LockObject = new object();
        public void Save()
        {
            lock (LockObject)
            {
                LastSaveTime = DateTime.Now;
                var str = XmlHelper.GetXmlSerialize(Content);
                FileHelper.CreateFolderByFilePath(FilePath);
                File.WriteAllText(FilePath,str,Encoding.UTF8);
            }
        }

        public void DeleteFile()
        {
            if (File.Exists(FilePath))
            {
                FileHelper.TryDeleteFile(FilePath);
            }
        }
    }
}