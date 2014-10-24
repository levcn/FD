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
using FD.Core.SLControl.Controls;
using FD.Core.SLControl.Extends;
using SLControls.Threads;
using Telerik.Windows.Controls;


namespace FD.Core.Test.Test
{
    public partial class 保存成功提示 : UserControl
    {
        public 保存成功提示()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof (object), typeof (保存成功提示), new PropertyMetadata(default(object), (s, e) =>
                {
                    ((保存成功提示) s).OnTextChanged(e.NewValue as object);
                }));

        private void OnTextChanged(object list)
        {}

        public object Text
        {
            get
            {
                return (object) GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).ShowTipMessageLayer();
            
            //            sss.IsBusy = true;
            //            ThreadHelper.DelayRun(() =>
            //            {
            //                Dispatcher.BeginInvoke(() =>
            //                {
            //                    sss.IsBusy = false;
            //                });
            //            },2000,"sdewe");
        }

        private void Aaa_OnOK(object sender, object args)
        {
            UIElement ui = sender as UIElement;
            ui.ShowBusyMessageLayer();
            ThreadHelper.DelayRun(() =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ui.ShowTipMessageLayer(closeAfterAction: () =>
                    {
                        RadDropDownButton button = ui.ParentOfType<RadDropDownButton>();
                        if (button != null) button.IsOpen = false;
                    });
                });
            },1000,"sdewe123");
          
        }

        private void Aaa_OnCancel(object sender, object args)
        {
            UIElement ui = sender as UIElement;
            RadDropDownButton radDropDownButton = ui.ParentOfType<RadDropDownButton>();
            if (radDropDownButton != null) radDropDownButton.IsOpen = false;
        }

        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
//            DropDownPopup.IsOpen = !DropDownPopup.IsOpen;
//            var wer = PopupPlacement.GetPopupPlacement(this.DropDownPopup);
//            PopupPlacement p = new PopupPlacement();
//            p.PlacementTarget = sender as Button;
////            PopupPlacement.
//            PopupPlacement.SetPopupPlacement(sender as Button,p);
//            wer.PlacementTarget = sender as Button;
        }

        private void DropdownButton_OnClick(object sender, RoutedEventArgs e)
        {
            UIElement uiElement = (dropdownButton.DropDownContent as UIElement);
            var ewerew = uiElement.ChildrenOfType<UserSelector>();
            var er123e = ewerew.FirstOrDefault(w => w.Name == "aaa");

//            var werwere = dropdownButton.GetVisualParent<>();
            var werwe =  dropdownButton.FindName("aaaaaaaa");
            var list123 = 动态添加测试.GetChildsByTypes<ContentControl>(dropdownButton);
            var list = dropdownButton.ChildrenOfType<Grid>().ToList();
            var ere =  list.FirstOrDefault(w => w.Name == "aaaaaaaa");
        }
    }

}
