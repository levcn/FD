using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FD.Core.SLControl.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Extensions;

namespace FD.Core.Test.Test
{
    public partial class 动态添加测试 : UserControl
    {
        public 动态添加测试()
        {
            InitializeComponent();
            Loaded += sdfsdfas;
        }

        private void sdfsdfas(object sender, RoutedEventArgs e)
        {
            var ok = asdfa.FindName("aaa") as IOkCancel;
            if (ok != null)
            {
                ok.OK += (ss, ee) =>
                {
                    asdfa.IsOpen = false;
                };

                ok.Cancel += (ss, ee) =>
                {
                    asdfa.IsOpen = false;
                };
            }
        }

        private bool inited = false;
        private void RadDropDownButton_OnDropDownOpened(object sender, RoutedEventArgs e)
        {

         
        }

        public static List<T> GetChildsByTypes<T>(DependencyObject owner) where T : DependencyObject
        {
            List<T> d = new List<T>();
            var c = VisualTreeHelper.GetChildrenCount(owner);
            if (owner is ContentControl)
            {
                var item1 = (owner as ContentControl);
                if (item1.Content is DependencyObject)
                {
                    var dep = item1.Content as DependencyObject;
                    if (dep is T) d.Add(dep as T);
                    var list = GetChildsByTypes<T>(dep);
                    if (list != null && list.Count > 0) d.AddRange(list);
                }
            }
            for (int i = 0; i < c; i++)
            {
                var item = VisualTreeHelper.GetChild(owner, i);
                if (item is T)
                {
                    d.Add(item as T);
                    var l = GetChildsByTypes<T>(item);
                    d.AddRange(l);
                }
                else
                {
                    if (item is ContentControl)
                    {
                        var item1 = (item as ContentControl);
                        if (item1.Content is DependencyObject)
                        {
                            var dep = item1.Content as DependencyObject;
                            if(dep is T)d.Add(dep as T);
                            var list = GetChildsByTypes<T>(dep);
                            if(list!=null&&list.Count>0)d.AddRange(list);
                        }
                    }
                    var l = GetChildsByTypes<T>(item);
                    d.AddRange(l);
                }
            }
            return d;
        }
    }
}
