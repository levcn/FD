using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using StaffTrain.FwClass.DataClientTools;


namespace StaffTrain.FwClass.NavigatorTools
{
    public static class UriHelper
    {
        /// <summary>
        /// 合并Uri
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Uri Combine(this Uri url, string path)
        {
//            var re = url.ToString();
//            re = re.TrimEnd('/', '\\');
//            path = path.TrimStart('/', '\\');
            return new Uri(url.ToString().TrimEnd('/', '\\') + "/" + path.TrimStart('/', '\\'));
        }
        /// <summary>
        /// 设置当前URL的锚点
        /// </summary>
        /// <param name="str"></param>
        public static void SetAnchor(string str)
        {
            if (Application.Current.IsRunningOutOfBrowser) return;
            HtmlPage.Window.Navigate(new Uri("#" + str, UriKind.Relative));
        }
        /// <summary>
        /// 返回锚点信息
        /// </summary>
        /// <returns></returns>
        public static string GetAnchor()
        {
            var df = CurrentUri;
            if (df != null && df.Fragment.StartsWith("#"))
            {
                var substring = df.Fragment.Substring(1).Trim('/');
                return substring;
            }
            return null;
        }
        static Uri currentUri;
        /// <summary>
        /// 返回当前的URL
        /// </summary>
        public static Uri CurrentUri
        {
            get
            {
                
                if (Application.Current.IsRunningOutOfBrowser) return null;
                if (currentUri == null)
                {
                    ScriptObject location = (HtmlPage.Window.GetProperty("location") as ScriptObject);
                    currentUri = new Uri(location.GetProperty("href").ToString());
                }
                return currentUri;
            }
        }
        /// <summary>
        /// 返回课程外链
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="courseCode"></param>
        /// <returns></returns>
        public static string GetOnlineCourseOuterLinkUrl(string orgCode, string courseCode,string courseSectionCode)
        {
            return string.Format("{0}#C@{1}@{2}@{3}", DataAccess.BaseUri, orgCode, courseCode, courseSectionCode);
        }
        /// <summary>
        /// URL中是否包含课程信息,直接转到课程播放
        /// </summary>
        /// <returns>转到课程返回true,否则false</returns>
        public static bool CheckGotoCoursePlayer()
        {
            var anchor = GetAnchor();
//            anchor.Trim('/');
            if (anchor != null && anchor.StartsWith("C",StringComparison.OrdinalIgnoreCase))
            {
                var strs = anchor.Split('@');
                if (strs.Length == 4)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 返回课件播放的URL中的参数,0:电厂CODE,1:CourseCode,2:SectionCode
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCoursePlayParams()
        {
            var anchor = GetAnchor();
            if (anchor != null && anchor.StartsWith("C", StringComparison.OrdinalIgnoreCase))
            {
                var strs = anchor.Split('@');
                if (strs.Length == 4)
                {
                    return strs.Skip(1).ToList();
                }
            }
            return null;
        }
    }
}
