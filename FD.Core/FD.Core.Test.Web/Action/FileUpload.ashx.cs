using System;
using System.IO;
using System.Web;
using Fw;
using Fw.Entity;
using Fw.Serializer;
using Fw.FileUpload;


namespace StaffTrain.Web.Action
{
    /// <summary>
    /// FileUpload 的摘要说明
    /// </summary>
    public class FileUpload : IHttpHandler
    {
        private HttpContext ctx;
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Process(context);
            }
            catch (Exception ee)
            {
                var detailErrorMsg = ee.GetBaseException().ToString();
                var rd = new ResultData { HaveError = true, ErrorMsg = ee.Message, DetailErrorMsg = detailErrorMsg };
                context.Response.Write(JsonHelper.JsonSerializer(rd));
                context.Response.End();
            }
        }

        private void Process(HttpContext context)
        {
            ctx = context;
            string uploadPath = context.Server.MapPath("~/" + AppSetting.FileTempPath);
            FileUploadManage.Init(uploadPath, "__uploadConfig.xml");
            FileUploadManage.ProcessRequest(context, fileUpload_FileUploadCompleted);
        }

        void fileUpload_FileUploadCompleted(object sender, FileUploadCompletedEventArgs args)
        {
            string fileType = ctx.Request.QueryString["FileType"];
            string savePath = ctx.Request.QueryString["SavePath"];
            string newFileName = "";
            if (!string.IsNullOrEmpty(savePath))
            {
                var path = Path.Combine(AppSetting.SiteRoot, savePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileInfo fi = new FileInfo(args.FilePath);
                newFileName = Guid.NewGuid() + fi.Extension;
                var destFileName = Path.Combine(path,newFileName);
                File.Move(fi.FullName,destFileName);
            }
            switch (fileType)
            {
                case "1"://人员头像
//                    var newFileName = MoveFile(args.FilePath, args.FileName, ctx.Server.MapPath("~/Upload"));
                    ctx.Response.Clear();
                    ctx.Response.Write((@"{""FileName"":""" + newFileName + @""",""OriginalFileName"":""" + args.FileName + @"""}"));
                    //ctx.Response.Write((@"{""FileName"":""" + newFileName + @"""}"));
                    break;
                case "2"://课件
                    var action = new System.Action(() => ImportCourse(args.FilePath));
                    action.BeginInvoke(null, null);
                    break;
                case "3"://试题
                    var action1 = new System.Action(() => ImportPaperItems(args.FilePath));
                    action1.BeginInvoke(null, null);
                    break;
                case "4"://公告
                    var newFileName1 = MoveFile(args.FilePath, args.FileName, ctx.Server.MapPath("~/Upload/Notice"));
                    ctx.Response.Clear();
                    ctx.Response.Write((@"{""FileName"":""" + newFileName1 + @""",""OriginalFileName"":""" + args.FileName + @"""}"));
                    break;
                case "8"://导入用户excel
                    var newFileName2 = MoveFile_GUID(args.FilePath, args.FileName, ctx.Server.MapPath("~/Upload/Excel"));
                    ctx.Response.Clear();
                    ctx.Response.Write((@"{""FileName"":""" + newFileName2 + @""",""OriginalFileName"":""" + args.FileName + @"""}"));
                    break;
                case "5"://自定义课程导入
                    var strDate = DateTime.Now.ToString("yyyyMMdd");
                    var dirPath = ctx.Server.MapPath(string.Format("~/Upload/CustomCourse/{0}", strDate));
                    var dir = new DirectoryInfo(dirPath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    var newCourseFileName = String.Format("{0}/{1}", strDate, MoveFile(args.FilePath, args.FileName, dirPath));
                    ctx.Response.Clear();
                    ctx.Response.Write((@"{""FileName"":""" + newCourseFileName + @""",""OriginalFileName"":""" + args.FileName + @"""}"));
                    break;
            }
        }
        #region 导入试题

        /// <summary>
        /// 导入试题入口
        /// </summary>
        /// <param name="filePath">带文件名的完整的路径</param>
        private void ImportPaperItems(string filePath)
        {
//            ExamAction.ImportByPath(filePath);
        }

        #endregion

        #region 导入课件

        /// <summary>
        /// 导入课件入口
        /// </summary>
        /// <param name="filePath">带文件名的完整的路径</param>
        /// <param name="dbType">Standardization改成1,现在的标准版库code=1</param>
        /// <param name="isDeleteOld"></param>
        private void ImportCourse(string filePath, string dbType = "1", bool isDeleteOld = true)
        {
//            CoursewareAction.ImportByPathThead(filePath, true, dbType, isDeleteOld);
        }

        #endregion

        private string MoveFile(string fullFilePath, string fileName, string targetFileFoler)
        {
            var fi = new FileInfo(fullFilePath);
            var uploadPath = targetFileFoler;// ;
            var name = GetFileName(fileName);
            var targetFile = Path.Combine(uploadPath, name);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            if (File.Exists(targetFile))
                File.Delete(targetFile);
            fi.MoveTo(targetFile);
            return name;
        }
        private string MoveFile_GUID(string fullFilePath, string fileName, string targetFileFoler)
        {
            var fi = new FileInfo(fullFilePath);
            var uploadPath = targetFileFoler;// ;
            var name = GetFileName_GUID(fileName);
            var targetFile = Path.Combine(uploadPath, name);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            if (File.Exists(targetFile))
                File.Delete(targetFile);
            fi.MoveTo(targetFile);
            return name;
        }

        private string MoveWordFile(string fullFilePath,string fileName,string targetFileFoler)
        {
            var fi = new FileInfo(fullFilePath);
            var uploadPath = targetFileFoler;// ;
            var name = fileName.Substring(0,fileName.LastIndexOf("."))+GetFileName(fileName);
            var targetFile = Path.Combine(uploadPath, name);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            if (File.Exists(targetFile))
                File.Delete(targetFile);
            fi.MoveTo(targetFile);
            return name;
        }

        private readonly Random r = new Random();
        /// <summary>
        /// 返回一个随机的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFileName(string fileName)
        {
            var index = fileName.LastIndexOf(".");
            var d = DateTime.Now;
            var re = string.Format("{0}{1}{2}{3}{4}{5}{6}", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, r.Next(1, 100));
            if (index > 0)
            {
                re += fileName.Substring(index);
            }
            return re;
        }
        /// <summary>
        /// 返回一个随机的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFileName_GUID(string fileName)
        {
            var index = fileName.LastIndexOf(".");
            var d = DateTime.Now;
            var re = string.Format("{0}{1}{2}{3}{4}{5}{6}", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, r.Next(1, 100));
            re += Guid.NewGuid();
            if (index > 0)
            {
                re += fileName.Substring(index);
            }
            return re;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}