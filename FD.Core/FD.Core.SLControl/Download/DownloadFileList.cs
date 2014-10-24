using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls;
using SLControls.Editors;


namespace SLControls.Download
{
    public class DownloadFileList : BaseItemsControl
    {
        public DownloadFileList()
        {
            this.DefaultStyleKey = typeof(DownloadFileList);
        }
        private Panel C_ListPanel;
        /// <summary>
        ///     添加一个上传文件
        /// </summary>
        /// <param name="file"></param>
        private void AddItem(string url, string savePath)
        {
            if (IsDesign()) return;
            DownloadFileItem ufi = null;
            if (ItemTemplate != null)
            {
                ufi = ItemTemplate.LoadContent() as DownloadFileItem;
            }
            if (ufi == null) ufi = new DownloadFileItem();
            C_ListPanel.Children.Add(ufi);
//            ufi.DisplayFileName = file.Name;
//            ufi.FileInfo = file;
//            ufi.UploadType = UploadType;
//            ufi.UploadServerPath = UploadServerPath;
            ufi.Delete += (s, e) =>
            {
                C_ListPanel.Children.Remove(ufi);
                Thread.Sleep(100);
                CheckAddFile();
                OnDeletedFile(null);
            };
//            ufi.UploadComplete += (s, e) => OnUploadComplete(e);
            //            ufi.UploadFile();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            C_ListPanel = GetTemplateChild("C_ListPanel") as Panel;
        }
        /// <summary>
        ///     删除文件事件
        /// </summary>
        public event TEventHandler<DownloadFileList, string> DeletedFile;

        protected virtual void OnDeletedFile(string args)
        {
            TEventHandler<DownloadFileList, string> handler = DeletedFile;
            if (handler != null) handler(this, args);
        }


        private void CheckAddFile()
        {
            {
                if (C_ListPanel.Children.Count == 0)
                {
                    C_ListPanel.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        ///     上传完成事件
        /// </summary>
        public event EventHandler<DownloadCompleteEventArgs> UploadComplete;

        protected void OnUploadComplete(DownloadCompleteEventArgs e)
        {
            Dispatcher.BeginInvoke(delegate
            {
                EventHandler<DownloadCompleteEventArgs> handler = UploadComplete;
                if (handler != null) handler(this, e);
            });
        }
    }

}
