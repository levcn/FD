using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SLControls.Upload;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.DataClientTools.Configs;


namespace SLControls.Editors
{
    public class EditItemPicture:ContentEditItem//BaseEditItem
    {
        public EditItemPicture()
        {
            DefaultStyleKey = typeof (EditItemPicture);
//            InitializeComponent();
            Loaded += EditItemPicture_Loaded;
        }
        UploadFileList uploadFileList1;

        public override void OnApplyTemplate()
        {
            
            base.OnApplyTemplate();

            uploadFileList1 = GetTemplateChild("uploadFileList1") as UploadFileList;
            FileName = GetTemplateChild("FileName") as TextBox;
            image1 = GetTemplateChild("image1") as Image;
            Tip1 = GetTemplateChild("Tip1") as LevcnValidTooltip;
            Tip2 = GetTemplateChild("Tip2") as LevcnValidTooltip;
            TB_ItemLableRequired = GetTemplateChild("TB_ItemLableRequired") as TextBlock;
            if (TB_ItemLableRequired != null) TB_ItemLableRequired.Visibility = Visibility.Collapsed;
            Init();
        }

        public static readonly DependencyProperty ImgSourceProperty =
            DependencyProperty.Register("ImgSource", typeof(ImageSource), typeof(EditItemPicture), new PropertyMetadata(default(ImageSource)));

        private TextBox FileName;
        private Image image1;
        private LevcnValidTooltip Tip1;
        private LevcnValidTooltip Tip2;
        private TextBlock TB_ItemLableRequired;

        public static readonly DependencyProperty SavePathProperty =
                DependencyProperty.Register("SavePath", typeof (string), typeof (EditItemPicture), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemPicture) s).OnSavePathChanged(e.NewValue as string);
                }));

        private void OnSavePathChanged(string list)
        {
            
        }

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
        public ImageSource ImgSource
        {
            get { return (ImageSource)GetValue(ImgSourceProperty); }
            set { SetValue(ImgSourceProperty, value); }
        }

        void EditItemPicture_Loaded(object sender, RoutedEventArgs e)
        {
//            Init();
        }

        private void Init()
        {
            if (DesignerProperties.IsInDesignTool) return;
            if (uploadFileList1 != null)
            {
                uploadFileList1.UploadUrl = ActionSetting.UploadUri;
                uploadFileList1.UploadType = 1;
                uploadFileList1.SavePath = this.SavePath;
                uploadFileList1.UploadComplete += (s, ee) =>
                {
                    if (FileName != null)
                    {
                        FileName.Text = ee.Result.FileName;
                    }
                    if (image1 != null)
                    {
                        ImgSource = new BitmapImage(new Uri(DataAccess.BaseUri, Path.Combine(SavePath, ee.Result.FileName)));
                    }
                };
            }
            if (image1 != null && FileName != null)
            {
                ImgSource = new BitmapImage(new Uri(DataAccess.BaseUri, Path.Combine(SavePath, FileName.Text)));
            }

            if (FileName != null && string.IsNullOrEmpty(FileName.Text))
            {
                ImgSource = new BitmapImage(new Uri("../../Images/nophoto.jpg", UriKind.Relative));
                // image1.Source = new BitmapImage(new Uri("../../Images/nophoto.jpg", UriKind.Relative));  
                //user.FilePath = "";
            }
        }

        public static readonly DependencyProperty UploadServerPathProperty =
                DependencyProperty.Register("UploadServerPath", typeof(string), typeof(EditItemPicture), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((EditItemPicture)s).OnUploadServerPathChanged(e.NewValue as string);
                }));

        private void OnUploadServerPathChanged(string list)
        {}

        public string UploadServerPath
        {
            get
            {
                return (string)GetValue(UploadServerPathProperty);
            }
            set
            {
                SetValue(UploadServerPathProperty, value);
            }
        }
        /// <summary>
        /// 返回验证控件
        /// </summary>
        public override List<BaseLevcnValidTooltip> ValidTooltip
        {
            get
            {
                return new List<BaseLevcnValidTooltip> { Tip1, Tip2 };
            }
        }
        /// <summary>
        /// 返回*号控件
        /// </summary>
        public override TextBlock ItemLableRequired
        {
            get
            {
                return TB_ItemLableRequired;
            }
        }

       

        protected override void OnTextChanged(object oldValue, object newValue)
        { 
            base.OnTextChanged(oldValue, newValue);
            //var a = newValue;
            if (newValue!=null)ImgSource = new BitmapImage(new Uri(DataAccess.BaseUri, "Upload/" + newValue));
            //TextBoxText = IsPassword ? newValue.ToString().FromBase64() : newValue.ToString();
        }
        public UploadFileList UploadFileList1
        {
            get
            {
                return uploadFileList1;
            }
        }
    }
}
