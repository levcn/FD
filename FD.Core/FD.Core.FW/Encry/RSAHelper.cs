using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ServerFw.Encry
{
    public static class RSAHelper
    {
        static public byte[] Encry(string publickey, byte[] data)
        {
            byte[] re = null;

            using (RSACryptoServiceProvider rsaReceive = new RSACryptoServiceProvider())
            {
                
                rsaReceive.FromXmlString(publickey);
                re = rsaReceive.Encrypt(data, false);
            }
            return re;
        }

        /// <summary>
        /// 使用提供的公钥加密
        /// </summary>
        /// <param name="modulus"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public byte[] Encry(byte[] modulus, byte[] data)
        {
            var d = BitConverter.GetBytes(326119);
            Array.Reverse(d);
            return Encry(modulus, d, data);
        }

        /// <summary>
        /// 使用提供的公钥加密
        /// </summary>
        /// <param name="modulus"></param>
        /// <param name="exponent"></param>
        /// <param name="data"></param>
        /// <param name="count"> </param>
        /// <returns></returns>
        static public byte[] Encry(byte[] modulus, byte[] exponent, byte[] data)
        {
            byte[] re = null;

            using (RSACryptoServiceProvider rsaReceive = new RSACryptoServiceProvider())
            {
                RSAParameters rsaParameters = new RSAParameters { Modulus = modulus, Exponent = exponent };
                //rsa.Modulus = Convert.FromBase64String("U32KX4Foa41H20cFEZviYUvPgQEvR0+jgUvUvxmc4AC48OwbHdaoZiidT3ZxCZtogwXNmKVy8ZRVn/lzX1xHrw==");//
                //rsa.Exponent = Convert.FromBase64String("AQAB");//
                rsaReceive.ImportParameters(rsaParameters);
                re = rsaReceive.Encrypt(data, true);
            }
            return re;
        }
        static public byte[] DencryPlus(RSAParameters para, byte[] data, bool foaep, bool reverseSizeBytes)
        {
            RSACryptoServiceProvider rs = new RSACryptoServiceProvider();

            rs.ImportParameters(para);
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            BinaryReader br = new BinaryReader(input);
            while (true)
            {
                int size = 0;
                var sizeBytes = br.ReadBytes(4);
                if (reverseSizeBytes) Array.Reverse(sizeBytes);
                size = BitConverter.ToInt32(sizeBytes, 0);
                if (size == 0) break;
                byte[] buffEncry = new byte[size];
                int readedLen = br.Read(buffEncry, 0, buffEncry.Length);
                byte[] dencryedBuff = rs.Decrypt(buffEncry, foaep);
                output.Write(dencryedBuff, 0, dencryedBuff.Length);
                if (input.Position == input.Length) break;
            }
            return output.ToArray();
        }
        /// <summary>
        /// 加密字节，支持无限多个。
        /// 
        /// 加密结果为分块加密方式，【大小int|区块内容】【大小int|区块内容】【大小int|区块内容】
        /// </summary>
        /// <param name="modulus">加密参数m</param>
        /// <param name="exponent">加密参数e</param>
        /// <param name="data">要加密的数据</param>
        /// <param name="foaep">加密参数xp系统之后支持true，最好设置为false</param>
        /// <param name="reverseSizeBytes">是否对区块大小的字节倒序</param>
        /// <param name="count">区块总数</param>
        /// <returns></returns>
        static public byte[] EncryPlus(byte[] modulus, byte[] exponent, byte[] data, bool foaep, bool reverseSizeBytes, out int count)
        {
            if (modulus[0] == 0 && modulus.Length == 65 ||
                modulus[0] == 0 && modulus.Length == 129)
            {
                modulus = modulus.Skip(1).Take(modulus.Length - 1).ToArray();
            }
            return EncryPlus(new RSAParameters { Modulus = modulus, Exponent = exponent }, data, foaep, reverseSizeBytes, out count);
        }
        static public byte[] EncryPlus(RSAParameters para, byte[] data, bool foaep, bool reverseSizeBytes, out int count)
        {

            RSACryptoServiceProvider rs = new RSACryptoServiceProvider();
            rs.ImportParameters(para);
            int buffSize = 53;
            if (rs.KeySize == 1024) buffSize = 117;
            count = data.Length / buffSize;
            if (data.Length % buffSize > 0) count++;

            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            input.Position = 0;

            var buff = new byte[buffSize];
            for (int i = 0; i < count; i++)
            {
                int readedLen = input.Read(buff, 0, buff.Length);
                if (readedLen == 0) break;
                byte[] ff = new byte[readedLen];
                Array.Copy(buff, 0, ff, 0, ff.Length);
                var encryedBuff = rs.Encrypt(ff, foaep);
                //if(encryedBuff[0] == 0)
                //{
                //    encryedBuff = Array.Copy()
                //}
                int len = encryedBuff.Length;
                byte[] byteLen = BitConverter.GetBytes(len);
                if (reverseSizeBytes)
                {
                    Array.Reverse(byteLen);
                }
                output.Write(byteLen, 0, byteLen.Length);//写入当前区块的长度
                output.Write(encryedBuff, 0, encryedBuff.Length);
            }
            return output.ToArray();
        }
    }
}
