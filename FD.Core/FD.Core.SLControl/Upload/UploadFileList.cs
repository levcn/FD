using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using SLControls;
using SLControls.Editors;


namespace SLControls.Upload
{
    public class UploadFileList : BaseItemsControl
    {
        public static readonly DependencyProperty UploadTypeProperty =
                DependencyProperty.Register("UploadType", typeof (int), typeof (UploadFileList), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MuiltFileProperty =
                DependencyProperty.Register("MuiltFile", typeof (bool), typeof (UploadFileList), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty FileFilterProperty =
                DependencyProperty.Register("FileFilter", typeof (string), typeof (UploadFileList), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UploadServerPathProperty =
                DependencyProperty.Register("UploadServerPath", typeof (string), typeof (UploadFileList), new PropertyMetadata(default(string)));

        private Button C_AddFileButton;
        private Panel C_ListPanel;

        public UploadFileList()
        {
            DefaultStyleKey = typeof (UploadFileList);
        }

        public static readonly DependencyProperty SavePathProperty =
                DependencyProperty.Register("SavePath", typeof (string), typeof (UploadFileList), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((UploadFileList) s).OnSavePathChanged(e.NewValue as string);
                }));

        private void OnSavePathChanged(string list)
        {}

        public string SavePath
        {
            get
            {
                return (string) GetValue(SavePathProperty);
            }
            set
            {
                SetValue(SavePathProperty, value);
            }
        }
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
        ///     是否多文件上传
        /// </summary>
        public bool MuiltFile
        {
            get
            {
                return (bool) GetValue(MuiltFileProperty);
            }
            set
            {
                SetValue(MuiltFileProperty, value);
            }
        }

        /// <summary>
        ///     文件类型过虑器
        /// </summary>
        public string FileFilter
        {
            get
            {
                return (string) GetValue(FileFilterProperty);
            }
            set
            {
                SetValue(FileFilterProperty, value);
            }
        }

        /// <summary>
        ///     文件上传的服务器地址
        /// </summary>
        public string UploadServerPath
        {
            get
            {
                return (string) GetValue(UploadServerPathProperty);
            }
            set
            {
                SetValue(UploadServerPathProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            C_AddFileButton = GetTemplateChild("C_AddFileButton") as Button;
            C_ListPanel = GetTemplateChild("C_ListPanel") as Panel;
            if (C_AddFileButton != null) C_AddFileButton.Click += C_AddFileButton_Click;
        }

        private void C_AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = FileFilter;
            dlg.Multiselect = MuiltFile;

            bool? showDialog = dlg.ShowDialog();
            if (showDialog != null && (bool) showDialog)
            {
                if (!MuiltFile) //编辑页面需要替换之前上传的文件
                {
                    C_ListPanel.Children.OfType<UploadFileItem>().ToList().ForEach(w => C_ListPanel.Children.Remove(w));
                }
                foreach (FileInfo file in dlg.Files)
                {
                    AddItem(file);
                    OnAddedFile(file);
                }
                if (!MuiltFile)
                {
                    if (C_ListPanel.Children.Count > 0)
                    {
                        //编辑页面需要替换之前上传的文件
                        C_AddFileButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public event TEventHandler<UploadFileList, FileInfo> AddedFile;

        protected virtual void OnAddedFile(FileInfo args)
        {
            TEventHandler<UploadFileList, FileInfo> handler = AddedFile;
            if (handler != null) handler(this, args);
        }

        public static readonly DependencyProperty UploadUrlProperty =
                DependencyProperty.Register("UploadUrl", typeof (Uri), typeof (UploadFileList), new PropertyMetadata(default(Uri), (s, e) =>
                {
                    ((UploadFileList) s).OnUploadUrlChanged(e.NewValue as Uri);
                }));

        private void OnUploadUrlChanged(Uri list)
        {
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
        /// <summary>
        ///     添加一个上传文件
        /// </summary>
        /// <param name="file"></param>
        private void AddItem(FileInfo file)
        {
            if (UploadServerPath == null) throw new Exception("UploadServerPath为空");
            if (IsDesign()) return;
            UploadFileItem ufi = null;
            if (ItemTemplate != null)
            {
                ufi = ItemTemplate.LoadContent() as UploadFileItem;
            }
            if (ufi == null) ufi = new UploadFileItem();
            C_ListPanel.Children.Add(ufi);
            ufi.DisplayFileName = file.Name;
            ufi.FileInfo = file; 
            ufi.UploadUrl = UploadUrl;
            ufi.SavePath = SavePath;
            ufi.UploadType = UploadType;
            ufi.UploadUrl = new Uri(UploadServerPath);
            ufi.Delete += (s, e) =>
            {
                C_ListPanel.Children.Remove(ufi);
                if (C_ListPanel.Children.Count == 0)
                {
                    //编辑页面需要替换之前上传的文件
                    C_AddFileButton.Visibility = Visibility.Visible;
                }
                Thread.Sleep(100);
                CheckAddFile();
                OnDeletedFile(ufi.FileInfo);
            };
            ufi.UploadComplete += (s, e) => OnUploadComplete(e);
            ufi.UploadFile();
        }

        /// <summary>
        ///     删除文件事件
        /// </summary>
        public event TEventHandler<UploadFileList, FileInfo> DeletedFile;

        protected virtual void OnDeletedFile(FileInfo args)
        {
            TEventHandler<UploadFileList, FileInfo> handler = DeletedFile;
            if (handler != null) handler(this, args);
        }

        private void CheckAddFile()
        {
            if (!MuiltFile)
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
        public event EventHandler<CompleteEventArgs> UploadComplete;

        protected void OnUploadComplete(CompleteEventArgs e)
        {
            Dispatcher.BeginInvoke(delegate
            {
                EventHandler<CompleteEventArgs> handler = UploadComplete;
                if (handler != null) handler(this, e);
            });
        }
    }
}