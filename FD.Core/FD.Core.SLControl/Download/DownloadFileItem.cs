using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using SLControls;
using SLControls.Editors;
using Telerik.Windows.Controls;


namespace SLControls.Download
{
    public class DownloadFileItem : BaseControl
    {
        public DownloadFileItem()
        {
            this.DefaultStyleKey = typeof(DownloadFileItem);
        }
        public static readonly DependencyProperty ProcessValueProperty =
                DependencyProperty.Register("ProcessValue", typeof (double), typeof (DownloadFileItem), new PropertyMetadata(0D
                        ,
                        (s, e) => { ((DownloadFileItem) s).OnProcessValueChanged((double) e.NewValue); }
                        ));

        public static readonly DependencyProperty IntProcessValueProperty =
                DependencyProperty.Register("IntProcessValue", typeof (int), typeof (DownloadFileItem), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty DisplayFileNameProperty =
                DependencyProperty.Register("DisplayFileName", typeof (string), typeof (DownloadFileItem), new PropertyMetadata(""));

        public static readonly DependencyProperty DownloadPathProperty =
                DependencyProperty.Register("DownloadPath", typeof (string), typeof (DownloadFileItem), new PropertyMetadata(default(string)));
        
        public static readonly DependencyProperty FileSizeProperty =
                DependencyProperty.Register("FileSize", typeof (long), typeof (DownloadFileItem), new PropertyMetadata(0L));

        public static readonly DependencyProperty UploadedSizeProperty =
                DependencyProperty.Register("UploadedSize", typeof (long), typeof (DownloadFileItem), new PropertyMetadata(default(long), (s, e) => { ((DownloadFileItem) s).OnDownloadedSizeChanged((long) e.NewValue); }));

        public static readonly DependencyProperty RemainFileSizeProperty =
                DependencyProperty.Register("RemainFileSize", typeof (long), typeof (DownloadFileItem), new PropertyMetadata(default(long)));

        private Button C_DeleteButton;
        private RadProgressBar C_Process;

        private FileInfo fileInfo;
    public FileInfo FileInfo
        {
            get
            {
                return fileInfo;
            }
            set
            {
                FileSize = value.Length;
                fileInfo = value;
            }
        }

        public double ProcessValue
        {
            get
            {
                return (double) GetValue(ProcessValueProperty);
            }
            set
            {
                SetValue(ProcessValueProperty, value);
            }
        }

        public int IntProcessValue
        {
            get
            {
                return (int) GetValue(IntProcessValueProperty);
            }
            protected set
            {
                SetValue(IntProcessValueProperty, value);
            }
        }

        public string DisplayFileName
        {
            get
            {
                return (string) GetValue(DisplayFileNameProperty);
            }
            set
            {
                SetValue(DisplayFileNameProperty, value);
            }
        }
        
        /// <summary>
        ///     服务器的上传地址
        /// </summary>
        public string DownloadPath
        {
            get
            {
                return (string) GetValue(DownloadPathProperty);
            }
            set
            {
                SetValue(DownloadPathProperty, value);
            }
        }


        /// <summary>
        ///     文件大小
        /// </summary>
        public long FileSize
        {
            get
            {
                return (long) GetValue(FileSizeProperty);
            }
            protected set
            {
                SetValue(FileSizeProperty, value);
            }
        }

        /// <summary>
        ///     已上传的文件大小
        /// </summary>
        public long UploadedSize
        {
            get
            {
                return (long) GetValue(UploadedSizeProperty);
            }
            protected set
            {
                SetValue(UploadedSizeProperty, value);
            }
        }

        /// <summary>
        ///     剩余文件大小
        /// </summary>
        public long RemainFileSize
        {
            get
            {
                return (long) GetValue(RemainFileSizeProperty);
            }
            protected set
            {
                SetValue(RemainFileSizeProperty, value);
            }
        }

        public void UploadFile()
        {
        }

        private void upload_UploadProgressChanged(object sender, UploadProgressChangedEventArgs args)
        {
//            UploadedSize = args.TotalBytesUploaded;
        }

        private void fileUpload_Complete(object sender, DownloadCompleteEventArgs e)
        {
            OnDownloadComplete(e);
            ProcessValue = 100;
        }

        public override void OnLoadDesignData()
        {
            base.OnLoadDesignData();
            DisplayFileName = "test.docx";
            ProcessValue = 60;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            C_Process = GetTemplateChild("C_Process") as RadProgressBar;
            C_DeleteButton = GetTemplateChild("C_DeleteButton") as Button;
            if (C_DeleteButton != null)
            {
                C_DeleteButton.Click += C_DeleteButton_Click;
            }
        }

        private void C_DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            OnDelete(new RoutedEventArgs());
        }

        /// <summary>
        ///     删除事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> Delete;

        public void OnDelete(RoutedEventArgs e)
        {
            EventHandler<RoutedEventArgs> handler = Delete;
            if (handler != null) handler(this, e);
        }

        private void OnProcessValueChanged(double newValue)
        {
            IntProcessValue = (int) newValue;
        }

        //

        private void OnDownloadedSizeChanged(long p)
        {
            if (FileSize != 0)
            {
                ProcessValue = (((double) p)/FileSize)*100;
                RemainFileSize = FileSize - p;
            }
        }

        public event EventHandler<DownloadCompleteEventArgs> DownloadComplete;

        protected void OnDownloadComplete(DownloadCompleteEventArgs e)
        {
            Dispatcher.BeginInvoke(delegate
            {
                EventHandler<DownloadCompleteEventArgs> handler = DownloadComplete;
                if (handler != null) handler(this, e);
            });
        }
    }
    /// <summary>
    ///     上传完成事件的参数
    /// </summary>
    public class DownloadCompleteEventArgs : EventArgs
    {
        /// <summary>
        ///     上传返回的结果
        /// </summary>
        public string Result { get; set; }
    }
}
