using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Microsoft.Win32;

namespace Fw.Helpers
{
    public class EnvironmentCheckHelper
    {
        /// <summary>
        /// 检测文件夹是否存在
        /// </summary>
        /// <param name="fullPath">文件夹完整路径</param>
        /// <returns></returns>
        public static bool CheckFolderExist(string fullPath)
        {
            return Directory.Exists(fullPath);
        }
        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="fullPath">文件完整路径</param>
        /// <returns></returns>
        public static bool CheckFileExist(string fullPath)
        {
            return File.Exists(fullPath);
        }
        /// <summary>
        /// 检测数据库连接是否正确
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <returns></returns>
        public static bool CheckDbIsConnectioned(string strConn)
        {
            var result = false;
            using (var conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    result = true;
                }
                catch (Exception)
                {

                }
            }

            return result;
        }
        /// <summary>
        /// 检测Framework4.0是否安装
        /// </summary>
        /// <returns></returns>
        public static bool CheckFramework4IsSetup()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\v4.0.30319");

            return key != null;
        }
        /// <summary>
        /// 检测文件夹写权限
        /// </summary>
        /// <param name="fullPath">文件夹完整路径</param>
        /// <returns></returns>
        public static bool CheckFolderCanWrite(string fullPath)
        {
            var result = false;
            try
            {
                var nFile = new FileStream(string.Format(@"{0}\{1}.txt", fullPath, Guid.NewGuid()), FileMode.OpenOrCreate);
                var writer = new StreamWriter(nFile);

                writer.Close();
                result = true;
                writer.WriteLine("检测写权限成功！");
            }
            catch (Exception)
            {
            }

            return result;
        }
        /// <summary>
        /// 检测文件夹写权限
        /// </summary>
        /// <param name="fullPath">文件夹完整路径</param>
        public static bool CheckFolderAuthority(string fullPath)
        {
            var result = true;

            try
            {
                var directory = new DirectoryInfo(fullPath);
                if (directory.Exists)
                {
                    var newDir = new DirectoryInfo(Path.Combine(fullPath, Guid.NewGuid().ToString()));
                    newDir.Create();
                    newDir.Delete();

                    var fileName = string.Format(@"{0}\{1}.txt", fullPath, Guid.NewGuid());
                    var nFile = new FileStream(fileName, FileMode.OpenOrCreate);
                    var writer = new StreamWriter(nFile);
                    writer.WriteLine("检测写权限成功！");
                    writer.Flush();
                    writer.Close();

                    var file = new FileInfo(fileName);
                    var stream = file.OpenRead();
                    //result = !stream.CanWrite;
                    stream.Close();
                    file.Delete();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                result = false;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;

        }

        /// <summary>
        /// 检测文件夹读权限
        /// </summary>
        /// <param name="fullPath">文件夹完整路径</param>
        /// <returns></returns>
        public static bool CheckFolderCanRead(string fullPath)
        {
            var result = true;
            try
            {
                var directory = new DirectoryInfo(fullPath);
                if (directory.Exists)
                {
                    directory.GetFiles();
                }
                //File.OpenRead(fullPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                result = false;
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// 验证MIME类型配置
        /// </summary>
        /// <param name="url">网站路径，如：http://192.168.232.128:9166/Resourse/Environment/test.xap </param>
        /// <returns></returns>
        public static bool CheckHttpHeader(string url)
        {
            var result = true;
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.GetResponse();
            }
            catch (Exception exception)
            {
                result = false;
            }
            return result;
        }
    }
}
