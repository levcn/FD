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
using SLControls;
using SLControls.Controls;
using SLControls.Editors;


namespace FD.Core.SLControl.Controls
{
    public class UserSelector : BaseMultiControl, ISelectable
    {
        public UserSelector()
        {
            this.DefaultStyleKey = typeof(UserSelector);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Cancel = GetTemplateChild("PART_Cancel") as Button;
            PART_OK = GetTemplateChild("PART_OK") as Button;
            if (PART_Cancel != null)
            {
                PART_Cancel.Click+=(s,e)=>OnCancel(null);
            }
            if (PART_OK != null)
            {
                PART_OK.Click += (s, e) => OnOk(null);
            }

        }

        public static readonly DependencyProperty HeaderTextProperty =
                DependencyProperty.Register("HeaderText", typeof (string), typeof (UserSelector), new PropertyMetadata("选择人员", (s, e) =>
                {
                    ((UserSelector) s).OnHeaderTextChanged(e.NewValue as string);
                }));

        private Button PART_Cancel;
        private Button PART_OK;

        private void OnHeaderTextChanged(string list)
        {}

        [Editable(GroupName = "基本属性", DisplayName = "标题栏", Description = "标题栏的文字设置。")]
        public string HeaderText
        {
            get
            {
                return (string) GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }
        public override void LoadConfig(string configStr)
        {
            
        }

        public event TEventHandler<object, object> OK;

        protected virtual void OnOk(object args)
        {
            var handler = OK;
            if (handler != null) handler(this, args);
        }

        public event TEventHandler<object, object> Cancel;

        protected virtual void OnCancel(object args)
        {
            var handler = Cancel;
            if (handler != null) handler(this, args);
        }
    }
}
