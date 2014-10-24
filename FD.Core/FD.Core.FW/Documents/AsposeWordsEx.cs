using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Words;


namespace Fw.Documents
{
    public static class AsposeWordsEx
    {
        /// <summary>
        /// 保存文档为docx
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static byte[] GetDocxBytes(this DocumentBuilder owner)
        {
            return GetBytes(owner, SaveFormat.Docx);
        }
        /// <summary>
        /// 保存文档为pdf
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static byte[] GetPDFBytes(this DocumentBuilder owner)
        {
            return GetBytes(owner, SaveFormat.Pdf);
        }
        /// <summary>
        /// 保存文档为docx
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this DocumentBuilder owner,SaveFormat format)
        {
            MemoryStream ms = new MemoryStream();
            owner.Document.Save(ms, format);
            var re = ms.ToArray();
            ms.Dispose();
            return re;
        }
    }
}
