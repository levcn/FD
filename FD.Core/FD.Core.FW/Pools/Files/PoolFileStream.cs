using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Fw.Pools.Files
{
    /// <summary>
    /// 线程池文件的操作流
    /// </summary>
    public class PoolFileStream
    {
        public PoolFileStream(string path)
            : this(path, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
            
        }

        public PoolFileStream(string path,FileMode fileMode,FileAccess fileAccess,FileShare fileShare)
        {
            FileInfo fi = new FileInfo(path);
            if(!fi.Directory.Exists) fi.Directory.Create();
            stream = new FileStream(path, fileMode, fileAccess, fileShare);
        }

        public long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }
        /// <summary>
        /// 写入指定的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public long WriteString(string str, Encoding encoding = null)
        {
            stream.Seek(0, SeekOrigin.End);
            if (encoding == null) encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(str);
            stream.Write(bytes,0,bytes.Length);
            return bytes.LongLength;
        }
        /// <summary>
        /// 写入文件内容,并关闭流
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        public void WriteAllText(string str, Encoding encoding = null)
        {
            stream.Position = 0;
            if (encoding == null) encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
            stream.SetLength(stream.Position);
            stream.Close();
        }
        /// <summary>
        /// 从指定位置读取指定长度的字符串
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadString(long position,long length,Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var bytes = ReadBytes(position, length);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// 读取所有文件内容
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadAllString(Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var bytes = ReadBytes(0, Length);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// 从文件中读取多个位置的指定长度的文本
        /// </summary>
        /// <param name="positions">位置列表(两个列表的长度要一致)</param>
        /// <param name="lengths">长度列表</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public List<string> ReadStrings(List<long> positions, List<long> lengths, Encoding encoding = null)
        {
            if (positions == null) throw new ArgumentNullException("positions");
            if (lengths == null) throw new ArgumentNullException("lengths");
            if (positions.Count != lengths.Count) throw new ArgumentException("positions和lengths的长度不一致");

            return Enumerable.Range(0, positions.Count)
                    .Select(i => ReadString(positions[i], lengths[i], encoding))
                    .ToList();
        }

        /// <summary>
        /// 从指定位置读取指定长度的字节
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadBytes(long position,long length)
        {
            stream.Position = position;
            byte[] re = new byte[length];
            stream.Read(re, 0, re.Length);
            return re;
        }
        public bool CanWrite
        {
            get
            {
                return stream.CanWrite;
            }
        }
        public bool CanRead
        {
            get
            {
                return stream.CanRead;
            }
        }

        public void Flush()
        {
            
            stream.Flush();
        }
        public byte ReadByte()
        {
            return (byte)stream.ReadByte();
        }

        public long Length
        {
            get
            {
                return stream.Length;
            }
        }
        public long Position
        {
            get
            {
                return stream.Position;
            }
            set
            {
                stream.Position = value;
            }
        }
        public int Read([In, Out] byte[] array, int offset, int count)
        {
            
            return stream.Read(array, offset, count);
        }

        internal void Close()
        {
            if(stream!=null) stream.Close();
        }
        FileStream stream;
        FileStream Stream
        {
            get
            {
                return stream;
            }
        }
    }
}
