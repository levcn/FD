using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;


namespace Fw.Helpers
{
    public class ZipHelper
    { 
        public FileInfo CurrentFile { get; set; }
        private byte[] buffer = new byte[2048];

        #region 压缩文件夹,支持递归

        /// <summary>  
        ///　压缩文件夹  
        /// </summary>  
        /// <param name="dir">待压缩的文件夹</param>  
        /// <param name="targetFileName">压缩后文件路径（包括文件名）</param>  
        /// <param name="recursive">是否递归压缩</param>  
        /// <returns></returns>  
        public bool Compress(string dir, string targetFileName, bool recursive)
        {
            //如果已经存在目标文件，询问用户是否覆盖  
            if (File.Exists(targetFileName))
            {
                // if (!_ProcessOverwrite(targetFileName))  
                return false;
            }
            string[] ars = new string[2];
            if (recursive == false)
            {
                //return Compress(dir, targetFileName);  
                ars[0] = dir;
                ars[1] = targetFileName;
                return ZipFileDictory(ars);
            }
            FileStream ZipFile;
            ZipOutputStream ZipStream;

            //open  
            ZipFile = File.Create(targetFileName);
            ZipStream = new ZipOutputStream(ZipFile);

            if (dir != String.Empty)
            {
                _CompressFolder(dir, ZipStream, dir.Substring(3));
            }

            //close  
            ZipStream.Finish();
            ZipStream.Close();

            if (File.Exists(targetFileName))
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 压缩目录  
        /// </summary>  
        /// <param name="args">数组(数组[0]: 要压缩的目录; 数组[1]: 压缩的文件名)</param>  
        public static bool ZipFileDictory(string[] args)
        {
            ZipOutputStream s = null;
            try
            {
                string[] filenames = Directory.GetFiles(args[0]);

                Crc32 crc = new Crc32();
                s = new ZipOutputStream(File.Create(args[1]));
                s.SetLevel(6);

                foreach (string file in filenames)
                {
                    //打开压缩文件  
                    FileStream fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ZipEntry entry = new ZipEntry(file);

                    entry.DateTime = DateTime.Now;

                    entry.Size = fs.Length;
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;

                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);

                }

            }
            catch (Exception e)
            {
                return false;
            }

            finally
            {
                s.Finish();
                s.Close();
            }
            return true;
        }





        /// <summary>  
        /// 压缩某个子文件夹  
        /// </summary>  
        /// <param name="basePath"></param>  
        /// <param name="zips"></param>  
        /// <param name="zipfolername"></param>       
        private static void _CompressFolder(string basePath, ZipOutputStream zips, string zipfolername)
        {
            if (File.Exists(basePath))
            {
                _AddFile(basePath, zips, zipfolername);
                return;
            }
            string[] names = Directory.GetFiles(basePath);
            foreach (string fileName in names)
            {
                _AddFile(fileName, zips, zipfolername);
            }

            names = Directory.GetDirectories(basePath);
            foreach (string folderName in names)
            {
                _CompressFolder(folderName, zips, zipfolername);
            }

        }

        /// <summary>  
        ///　压缩某个子文件  
        /// </summary>  
        /// <param name="fileName"></param>  
        /// <param name="zips"></param>  
        /// <param name="zipfolername"></param>  
        private static void _AddFile(string fileName, ZipOutputStream zips, string zipfolername)
        {
            if (File.Exists(fileName))
            {
                _CreateZipFile(fileName, zips, zipfolername);
            }
        }

        /// <summary>  
        /// 压缩单独文件  
        /// </summary>  
        /// <param name="FileToZip"></param>  
        /// <param name="zips"></param>  
        /// <param name="zipfolername"></param>  
        private static void _CreateZipFile(string FileToZip, ZipOutputStream zips, string zipfolername)
        {
            try
            {
                FileStream StreamToZip = new FileStream(FileToZip, FileMode.Open, FileAccess.Read);
                string temp = FileToZip;
                string temp1 = zipfolername;
                if (temp1.Length > 0)
                {
                    int i = temp1.LastIndexOf("\\") + 1; //这个地方原来是个bug用的是"//"，导致压缩路径过长路径2012-7-2  
                    int j = temp.Length - i;
                    temp = temp.Substring(i, j);
                }
                ZipEntry ZipEn = new ZipEntry(temp.Substring(3));

                zips.PutNextEntry(ZipEn);
                byte[] buffer = new byte[16384];
                System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
                zips.Write(buffer, 0, size);
                try
                {
                    while (size < StreamToZip.Length)
                    {
                        int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                        zips.Write(buffer, 0, sizeRead);
                        size += sizeRead;
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                StreamToZip.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        /// <summary>  
        /// 解压缩目录  
        /// </summary>  
        /// <param name="zipDirectoryPath">压缩目录路径</param>  
        /// <param name="unZipDirecotyPath">解压缩目录路径</param>
        /// <param name="process">总长度,已经处理的长度,百分比,已经处理的数量</param>  
        public void UnZipDirectory(string zipDirectoryPath, string unZipDirecotyPath, string Password,Action<long,long,int,int> process=null)
        {
            while (unZipDirecotyPath.LastIndexOf("\\") + 1 == unZipDirecotyPath.Length) //检查路径是否以"\"结尾  
            {
                unZipDirecotyPath = unZipDirecotyPath.Substring(0, unZipDirecotyPath.Length - 1); //如果是则去掉末尾的"\"  
            }
            int index = 0;
            long sum = 0;
            using(FileStream fs = File.OpenRead(zipDirectoryPath))
            using (ZipInputStream zipStream = new ZipInputStream(fs))
            {
                
                //判断Password  
                if (Password != null && Password.Length > 0)
                {
                    zipStream.Password = Password;
                }
                
                ZipEntry zipEntry = null;
                while ((zipEntry = zipStream.GetNextEntry()) != null)
                {
                    sum += zipStream.Length;
                    var p = (int)(((float)sum / fs.Length) * 100);
                    ++index;
                    if (process != null) process(fs.Length, sum,p, index);
                    string directoryName = Path.GetDirectoryName(zipEntry.Name);
                    string fileName = Path.GetFileName(zipEntry.Name);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(unZipDirecotyPath + @"\" + directoryName);
                    }

                    if (!string.IsNullOrEmpty(fileName))
                    {
//                        if (zipEntry.CompressedSize == 0)
//                            continue;
                        if (zipEntry.IsDirectory) //如果压缩格式为文件夹方式压缩  
                        {
                            directoryName = Path.GetDirectoryName(unZipDirecotyPath + @"\" + zipEntry.Name);
                            Directory.CreateDirectory(directoryName);
                        }
                        else //2012-5-28修改，支持单个文件压缩时自己创建目标文件夹  
                        {
                            if (!Directory.Exists(unZipDirecotyPath))
                            {
                                Directory.CreateDirectory(unZipDirecotyPath);
                            }
                        }

                        using (FileStream stream = File.Create(unZipDirecotyPath + @"\" + zipEntry.Name))
                        {
                            while (true)
                            {
                                int size = zipStream.Read(buffer, 0, buffer.Length);
                                if (size > 0)
                                {
                                    stream.Write(buffer, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
