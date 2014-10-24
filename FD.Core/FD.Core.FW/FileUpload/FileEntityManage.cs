using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fw.FileUpload;
using Fw.Serializer;


namespace Fw.FileUpload
{
    /// <summary>
    /// 上传文件信息的管理
    /// </summary>
    public class FileEntityManage
    {
        public FileEntityManage(string uploadRootPath,string configFileName)
        {
            UploadRootPath = uploadRootPath;
            ConfigFileName = configFileName;
        }
        public string UploadRootPath { get; set; }
        public string ConfigFileName { get; set; }
        public string FullFilePath
        {
            get
            {
                return Path.Combine(UploadRootPath, ConfigFileName);
            }
        }
        private List<FileEntity> fileEntities;

        public List<FileEntity> FileEntities
        {
            get
            {
                if (fileEntities == null)
                {
                    fileEntities = new List<FileEntity>();
                    string fullFilePath = FullFilePath;
                    if (File.Exists(fullFilePath))
                    {
                        var text = File.ReadAllText(fullFilePath);
                        fileEntities = XmlHelper.GetXmlDeserialize<List<FileEntity>>(text);
                    }
                }
                return fileEntities;
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void SaveFileEntities()
        {
            string fullFilePath = FullFilePath;
            string text = XmlHelper.GetXmlSerialize(FileEntities);
            File.WriteAllText(fullFilePath,text);
        }
        /// <summary>
        /// 更新上传文件信息
        /// </summary>
        /// <param name="fileName">相对地址+文件名</param>
        /// <param name="isNew">是否新上传的文件</param>
        /// <param name="fileLength">文件大小</param>
        /// <param name="uploadLength">已经上传的文件大小</param>
        internal void Update(string fileName, bool isNew, long fileLength, long uploadLength)
        {
            if (isNew)
            {
                FileEntity fe = new FileEntity
                                    {
                                            FilePathName = fileName,
                                            FileSize = fileLength,
                                            LastActiveTime = DateTime.Now,
                                            StartTime = DateTime.Now,
                                            UploadSize = uploadLength
                                    };
                FileEntities.Add(fe);
            }
            else
            {
                var fe = FileEntities.FirstOrDefault(w => w.FilePathName.Equals(fileName, StringComparison.OrdinalIgnoreCase) && w.UploadSize != w.FileSize);
                if (fe != null)
                {
                    fe.LastActiveTime = DateTime.Now;
                    fe.UploadSize = uploadLength;
                }
            }
            SaveFileEntities();
        }
    }
}