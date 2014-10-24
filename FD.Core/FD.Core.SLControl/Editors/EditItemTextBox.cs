using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SLControls.Extends;
using SLControls.Threads;


namespace SLControls.Editors
{

    [TemplateVisualState(GroupName = "BrowserStates", Name = "Browser")]
    [TemplateVisualState(GroupName = "BrowserStates", Name = "Editor")]
    public class EditItemTextBox : ContentEditItem
    {
        public EditItemTextBox()
        {
            DefaultStyleKey = typeof(EditItemTextBox);
            BorderThickness = new Thickness(1);
        }

        private TextBox TB_TextBox;
        private TextBlock TB_TextBrowse;
        private TextBlock TB_ItemLableRequired;
        private TextBlock TB_ItemLable;
//        private Button TB_Save;
        private List<BaseLevcnValidTooltip> Tips;
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
//            return;
            TB_TextBox = GetTemplateChild("TB_TextBox") as TextBox;
            TB_TextBrowse = GetTemplateChild("TB_TextBrowse") as TextBlock;
            TB_ItemLableRequired = GetTemplateChild("TB_ItemLableRequired") as TextBlock;
            TB_ItemLable = GetTemplateChild("TB_ItemLable") as TextBlock;
//            TB_Save = GetTemplateChild("TB_Save") as Button;
            Tips = new List<BaseLevcnValidTooltip>();
            
            for (int i = 0; i < 10; i++)
            {
                var t = GetTemplateChild("Tip" + i) as LevcnValidTooltip;
                if (t != null)
                {
                    Tips.Add(t);
                }
            }
//            if (TB_Save != null)
//            {
//                TB_Save.Click += TB_Save_Click;
//            }
            if (TB_TextBox != null)
            {
                TB_TextBox.TextChanged += TextBox_TextChanged_1;
                TB_TextBox.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Escape)
                    {
                        CancelEdit();
                    }
                    if (!IsCardInput) return;
                    lastText.Add(TB_TextBox.Text);
                    if ((DateTime.Now - lastInput).TotalMilliseconds < inputDelay)
                    {
                        e.Handled = true;
                        if (!deleteFirst)
                        {
                            lock (lastText)
                            {
                                if (lastText.Count >= 2)
                                {
                                    //                            aaa.Text = aaa.Text.Substring(0, aaa.Text.Length - 1);
                                    TB_TextBox.Text = lastText[lastText.Count - 2];
                                }
                            }
                            deleteFirst = true;
                        }
                    }
                    ThreadHelper.DelayRun(() =>
                    {
                        lock (lastText)
                        {
                            lastText.Clear();
                        }
                        deleteFirst = false;
                    }, 10, "KeyDown_dferweqe");
                    lastInput = DateTime.Now;
                };
            }
            if (TB_TextBrowse != null)
            {
                TB_TextBrowse.MouseLeftButtonDown += TB_TextBrowse_MouseLeftButtonUp;
            }
//            if (TB_TextBox != null)
            {
                LostFocus += TB_TextBox_LostFocus;
            }
        }

        /// <summary>
        /// 取消编辑,还原数据
        /// </summary>
        private void CancelEdit()
        {
            if (!ClickToEditor) return;
            if (oldTextValue != null && TB_TextBox != null) TB_TextBox.Text = oldTextValue;
            ViewMode = ViewMode.Browser;
        }

//        void TB_Save_Click(object sender, RoutedEventArgs e)
//        {
//            ViewMode = ViewMode.Browser;
//            
//        }

        public static readonly DependencyProperty SaveVisibilityProperty =
                DependencyProperty.Register("SaveVisibility", typeof (Visibility), typeof (EditItemTextBox), new PropertyMetadata(default(Visibility)));

        public Visibility SaveVisibility
        {
            get
            {
                return (Visibility) GetValue(SaveVisibilityProperty);
            }
            set
            {
                SetValue(SaveVisibilityProperty, value);
            }
        }
        /// <summary>
        /// 从浏览模式转到编辑模式时,记录编辑之前的文本内容
        /// </summary>
        string oldTextValue = null;
        protected override void OnViewModeChanged(ViewMode newValue)
        {
            base.OnViewModeChanged(newValue);
            if (newValue == ViewMode.Editor)
            {
                if (TB_TextBox != null)
                {
                    oldTextValue = TB_TextBox.Text;
                    TB_TextBox.Focus();
                }
            }
        }

        void TB_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LostFocusToBrowser)
            {
                ViewMode = ViewMode.Browser;
            }
            
        }

        void TB_TextBrowse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickToEditor)
            {
                ViewMode = ViewMode.Editor;
            }
        }

        public static readonly DependencyProperty IsCardInputProperty =
                DependencyProperty.Register("IsCardInput", typeof(bool), typeof(EditItemTextBox), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((EditItemTextBox)s).OnDisableFastInputChanged((bool)e.OldValue, (bool)e.NewValue);
                }));

        private void OnDisableFastInputChanged(bool oldValue, bool newValue)
        {

        }
        
//        public string Text { get; set; }
        /// <summary>
        /// 刷卡输入
        /// </summary>
        public bool IsCardInput
        {
            get
            {
                return (bool)GetValue(IsCardInputProperty);
            }
            set
            {
                SetValue(IsCardInputProperty, value);
            }
        }

        public static readonly DependencyProperty ClickToEditorProperty =
                DependencyProperty.Register("ClickToEditor", typeof (bool), typeof (EditItemTextBox), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 浏览模式下是否点击切换编辑模式
        /// </summary>
        public bool ClickToEditor
        {
            get
            {
                return (bool) GetValue(ClickToEditorProperty);
            }
            set
            {
                SetValue(ClickToEditorProperty, value);
            }
        }

        public static readonly DependencyProperty LostFocusToBrowserProperty =
                DependencyProperty.Register("LostFocusToBrowser", typeof (bool), typeof (EditItemTextBox), new PropertyMetadata(default(bool)));

        public bool LostFocusToBrowser
        {
            get
            {
                return (bool) GetValue(LostFocusToBrowserProperty);
            }
            set
            {
                SetValue(LostFocusToBrowserProperty, value);
            }
        }
        public static readonly DependencyProperty TextBoxTextProperty =
                DependencyProperty.Register("TextBoxText", typeof(string), typeof(EditItemTextBox),
                                            new PropertyMetadata(default(string),
                                                                 (s, e) => ((EditItemTextBox)s).OnTextBoxTextChanged(e.OldValue, e.NewValue)));
        void OnTextBoxTextChanged(object old, object _new)
        {
            Text = IsPassword ? _new.ToString().ToBase64() : _new;
        }
        public TextBlock TxtBrowse
        {
            get
            {
                return TB_TextBrowse;
            }
        }
        protected override void OnReadOnlyChanged(object oldValue, object newValue)
        {
            base.OnReadOnlyChanged(oldValue, newValue);
            TB_TextBox.IsReadOnly = (bool)newValue;
        }
        protected override void OnTextChanged(object oldValue, object newValue)
        {
            base.OnTextChanged(oldValue, newValue);
            TextBoxText = IsPassword ? newValue.ToString().FromBase64() : newValue.ToString();
        }

        public string TextBoxText
        {
            get
            {
                return (string)GetValue(TextBoxTextProperty);
            }
            set
            {
                SetValue(TextBoxTextProperty, value);
            }
        }
        public static readonly DependencyProperty IsPasswordProperty =
                DependencyProperty.Register("IsPassword", typeof(bool), typeof(EditItemTextBox), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 当前文本框是否是密码
        /// </summary>
        public bool IsPassword
        {
            get
            {
                return (bool)GetValue(IsPasswordProperty);
            }
            set
            {
                SetValue(IsPasswordProperty, value);
            }
        }

        public static readonly DependencyProperty AcceptsReturnProperty =
                DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(EditItemTextBox), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 文件框是否可以换行
        /// </summary>
        public bool AcceptsReturn
        {
            get
            {
                return (bool)GetValue(AcceptsReturnProperty);
            }
            set
            {
                SetValue(AcceptsReturnProperty, value);
            }
        }
        public static readonly DependencyProperty TextBoxHeightProperty =
                DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(EditItemTextBox), new PropertyMetadata(double.NaN));

        public double TextBoxHeight
        {
            get
            {
                return (double)GetValue(TextBoxHeightProperty);
            }
            set
            {
                SetValue(TextBoxHeightProperty, value);
            }
        }

        
        public static readonly DependencyProperty TextBoxWidthProperty =
              DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(EditItemTextBox), new PropertyMetadata(double.NaN));

        public double TextBoxWidth
        {
            get { return (double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }
        private DateTime lastInput = DateTime.MinValue;
        private int inputDelay = 30;
        private bool deleteFirst = false;
        private List<string> lastText = new List<string>();
        void EditItemTextBox_Loaded(object sender, RoutedEventArgs ee)
        {
            
        }
        /// <summary>
        /// 返回验证控件
        /// </summary>
        public override List<BaseLevcnValidTooltip> ValidTooltip
        {
            get
            {
                return Tips;
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
        /// <summary>
        /// 
        /// </summary>
        protected override FrameworkElement ContentControl
        {
            get
            {
                return null;
            }
        }

        public TextBox TextBox
        {
            get
            {
                return TB_TextBox;
            }
        }

        
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBoxText = (sender as TextBox).Text;
        }
    }
}
