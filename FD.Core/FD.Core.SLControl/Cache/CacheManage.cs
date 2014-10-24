using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Cache
{
    public class CacheManage
    {
        CacheManage()
        {

        }

        public static CacheManage current;
        private static object currentFlag = new object();
        /// <summary>
        /// 返回当前的缓存管理者
        /// </summary>
        public static CacheManage Current
        {
            get
            {
                lock (currentFlag)
                {
                    if (current == null)
                    {
                        current = new CacheManage();
                    }
                    return current;
                }
            }
        }
    }
}
