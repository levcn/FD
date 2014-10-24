using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SLControls;
using SLControls.Editors;
using Telerik.Windows.Controls;


namespace SLControls.Upload
{
    /// <summary>
    ///     单个文件上传控件
    /// </summary>
    public class UploadFileItem : BaseControl
    {
        public static readonly DependencyProperty ProcessValueProperty =
                DependencyProperty.Register("ProcessValue", typeof (double), typeof (UploadFileItem), new PropertyMetadata(0D
                        ,
                        (s, e) => { ((UploadFileItem) s).OnProcessValueChanged((double) e.NewValue); }
                        ));

        public static readonly DependencyProperty IntProcessValueProperty =
                DependencyProperty.Register("IntProcessValue", typeof (int), typeof (UploadFileItem), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty DisplayFileNameProperty =
                DependencyProperty.Register("DisplayFileName", typeof (string), typeof (UploadFileItem), new PropertyMetadata(""));

        public static readonly DependencyProperty ServerFileNameProperty =
                DependencyProperty.Register("ServerFileName", typeof (string), typeof (UploadFileItem), new PropertyMetadata(default(string)));

//        public static readonly DependencyProperty UploadServerPathProperty =
//                DependencyProperty.Register("UploadServerPath", typeof (string), typeof (UploadFileItem), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UploadTypeProperty =
                DependencyProperty.Register("UploadType", typeof (int), typeof (UploadFileItem), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty FileSizeProperty =
                DependencyProperty.Register("FileSize", typeof (long), typeof (UploadFileItem), new PropertyMetadata(0L));

        public static readonly DependencyProperty UploadedSizeProperty =
                DependencyProperty.Register("UploadedSize", typeof (long), typeof (UploadFileItem), new PropertyMetadata(default(long), (s, e) => { ((UploadFileItem) s).OnUploadedSizeChanged((long) e.NewValue); }));

        public static readonly DependencyProperty RemainFileSizeProperty =
                DependencyProperty.Register("RemainFileSize", typeof (long), typeof (UploadFileItem), new PropertyMetadata(default(long)));

        private Button C_DeleteButton;
        private RadProgressBar C_Process;

        private FileInfo fileInfo;
        private FileUpload fileUpload;

        public UploadFileItem()
        {
            DefaultStyleKey = typeof (UploadFileItem);
        }

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

        public string ServerFileName
        {
            get
            {
                return (string) GetValue(ServerFileNameProperty);
            }
            set
            {
                SetValue(ServerFileNameProperty, value);
            }
        }
//
//        /// <summary>
//        ///     服务器的上传地址
//        /// </summary>
//        public string UploadServerPath
//        {
//            get
//            {
//                return (string) GetValue(UploadServerPathProperty);
//            }
//            set
//            {
//                SetValue(UploadServerPathProperty, value);
//            }
//        }

        /// <summary>
        ///     按业务分 上传的文件类型.
        /// </summary>
        public int UploadType
        {
            get
            {
                return (int) GetValue(UploadTypeProperty);
            }
            set
            {
                SetValue(UploadTypeProperty, value);
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

        public static readonly DependencyProperty UploadUrlProperty =
                DependencyProperty.Register("UploadUrl", typeof (Uri), typeof (UploadFileItem), new PropertyMetadata(default(Uri), (s, e) =>
                {
                    ((UploadFileItem) s).OnUploadUrlChanged(e.NewValue as Uri);
                }));

        private void OnUploadUrlChanged(Uri list)
        {
//            throw new NotImplementedException();
        }

        public Uri UploadUrl
        {
            get
            {
                return (Uri) GetValue(UploadUrlProperty);
            }
            set
            {
                SetValue(UploadUrlProperty, value);
            }
        }
        public void UploadFile()
        {
            if (IsDesign()) return;
            if (this.UploadUrl == null) 
                throw new Exception("UploadUrl为空");
            if (fileUpload == null)
            {
                fileUpload = new FileUpload(Dispatcher, UploadUrl, FileInfo);
                fileUpload.FileType = UploadType;
                fileUpload.SavePath = SavePath;
                fileUpload.UploadUrl = this.UploadUrl;
                //                fileUpload.StatusChanged += new EventHandler(upload_StatusChanged);
                fileUpload.Complete += (fileUpload_Complete);
                fileUpload.UploadProgressChanged += upload_UploadProgressChanged;
            }
            fileUpload.Upload();
        }

        private void upload_UploadProgressChanged(object sender, UploadProgressChangedEventArgs args)
        {
            UploadedSize = args.TotalBytesUploaded;
        }

        private void fileUpload_Complete(object sender, CompleteEventArgs e)
        {
            OnUploadComplete(e);
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

        private void OnUploadedSizeChanged(long p)
        {
            if (FileSize != 0)
            {
                ProcessValue = (((double) p)/FileSize)*100;
                RemainFileSize = FileSize - p;
            }
        }

        public event EventHandler<CompleteEventArgs> UploadComplete;

        protected void OnUploadComplete(CompleteEventArgs e)
        {
            Dispatcher.BeginInvoke(delegate
            {
                if(C_Process!=null)C_Process.Visibility = Visibility.Collapsed;
                EventHandler<CompleteEventArgs> handler = UploadComplete;
                if (handler != null) handler(this, e);
            });
        }

        public string SavePath { get; set; }
    }

    /// <summary>
    ///     上传完成之后的结果数据对象
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        ///     修改过后的文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     上传之前的原始文件名
        /// </summary>
        public string OriginalFileName { get; set; }
    }

    /// <summary>
    ///     上传完成事件的参数
    /// </summary>
    public class UploadCompleteEventArgs : EventArgs
    {
        /// <summary>
        ///     上传返回的结果
        /// </summary>
        public ResponseResult Result { get; set; }
    }
}