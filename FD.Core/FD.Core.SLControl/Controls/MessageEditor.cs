using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FD.Core.SLControl.Data;
using SLControls.Editors;


namespace FD.Core.SLControl.Controls
{
    public class MessageEditor : BaseMultiControl
    {
        public MessageEditor()
        {
            this.DefaultStyleKey = typeof(MessageEditor);
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            PART_EnterButton = GetTemplateChild("PART_EnterButton") as Button;
            if(PART_TextBox!=null)
            {
                PART_TextBox.TextChanged += TextBox_OnTextChanged;
            } if (PART_EnterButton != null)
            {
                PART_EnterButton.Click += Enter_OnClick;
            }
        }

        public override void LoadConfig(string configStr)
        {
            
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(string), typeof(MessageEditor), new PropertyMetadata(default(string), (s, e) =>
                {
                    ((MessageEditor)s).OnTextChanged(e.NewValue as string);
                }));

        private void OnTextChanged(string list)
        {
            if (list !=null)TextLength = list.Length;
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        public static readonly DependencyProperty TextMaxLengthProperty =
                DependencyProperty.Register("TextMaxLength", typeof(int), typeof(MessageEditor), new PropertyMetadata(1000, (s, e) =>
                {
                    ((MessageEditor)s).OnTextMaxLengthChanged(e.NewValue is int ? (int)e.NewValue : 0);
                }));

        private void OnTextMaxLengthChanged(int list)
        { }

        public int TextMaxLength
        {
            get
            {
                return (int)GetValue(TextMaxLengthProperty);
            }
            set
            {
                SetValue(TextMaxLengthProperty, value);
            }
        }
        public static readonly DependencyProperty HeaderTextProperty =
                DependencyProperty.Register("HeaderText", typeof(string), typeof(MessageEditor), new PropertyMetadata("测试一", (s, e) =>
                {
                    ((MessageEditor)s).OnHeaderTextChanged(e.NewValue as string);
                }));

        private void OnHeaderTextChanged(string list)
        { }

        public string HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }

        public static readonly DependencyProperty TextLengthProperty =
                DependencyProperty.Register("TextLength", typeof(int), typeof(MessageEditor), new PropertyMetadata(default(int), (s, e) =>
                {
                    ((MessageEditor)s).OnTextLengthChanged(e.NewValue is int ? (int)e.NewValue : 0);
                }));

        private TextBox PART_TextBox;
        private Button PART_EnterButton;

        private void OnTextLengthChanged(int list)
        { }

        public int TextLength
        {
            get
            {
                return (int)GetValue(TextLengthProperty);
            }
            set
            {
                SetValue(TextLengthProperty, value);
            }
        }
        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextLength = (sender as TextBox).Text.Length;
        }

        public event TEventHandler<object, EventArgs> Submited;

        protected virtual void OnSubmited()
        {
            var handler = Submited;

            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void Enter_OnClick(object sender, RoutedEventArgs e)
        {
            OnSubmited();
        }
    }
}
