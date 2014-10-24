using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace ServerFw.Encry
{
    public class RijndaelHelper
    {
        private static string superKey = "1111111111111111";

        private static string vectoryString = "2222222222222222222222";

        private static readonly RijndaelManaged rijndael = new RijndaelManaged();

        private static byte[] key;

        private static byte[] iv;

        private static void InitialKeyAndIV()

        {
            key = new byte[32];

            byte[] byte_pwd = Encoding.UTF8.GetBytes(superKey);
            MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
            key = provider_MD5.ComputeHash(byte_pwd);

//            Array.Copy(Encoding.UTF8.GetBytes(superKey), key, 32);

            iv = new byte[16];

            var sourceArray = Encoding.UTF8.GetBytes(vectoryString);
            Array.Copy(sourceArray, iv, 16);
        }

        public static string EncryptInforamtion(string dataString)

        {
            var utf32Encoding = new UTF32Encoding();

            if (key == null || iv == null)

            {
                InitialKeyAndIV();
            }

            Byte[] returnVal = AESEncrypt(utf32Encoding.GetBytes(dataString), rijndael.CreateEncryptor(key, iv));

            return Convert.ToBase64String(returnVal);
        }

        public static string DecryptInformation(string dataString)

        {
            var utf32Encoding = new UTF32Encoding();

            if (key == null || iv == null)

            {
                InitialKeyAndIV();
            }

            Byte[] returnVal = AESDencrypt(Convert.FromBase64String(dataString), rijndael.CreateDecryptor(key, iv));

            //因為加解密會對byte[]做填充，所以解完密後要去掉。

            return utf32Encoding.GetString(returnVal).Replace("\0", "");
        }

        /// <summary>
        ///     AES 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encryptor"></param>
        /// <returns></returns>
        private static byte[] AESEncrypt(byte[] input, ICryptoTransform encryptor)

        {
            //Encrypt the data.

            var msEncrypt = new MemoryStream();

            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            //Write all data to the crypto stream and flush it.

            csEncrypt.Write(input, 0, input.Length);

            csEncrypt.FlushFinalBlock();

            //Get encrypted array of bytes.

            return msEncrypt.ToArray();
        }

        /// <summary>
        ///     AES 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="decryptor"></param>
        /// <returns></returns>
        private static byte[] AESDencrypt(byte[] input, ICryptoTransform decryptor)

        {
            //Now decrypt the previously encrypted message using the decryptor

            // obtained in the above step.

            var msDecrypt = new MemoryStream(input);

            var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            var fromEncrypt = new byte[input.Length];

            //Read the data out of the crypto stream.

            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

            return fromEncrypt;
        }
    }
}