using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FD.Core.SLControl.Data;
using FD.Core.SLControl.NavigatorTools;
using FD.Core.Test.Annotations;
using SLControls.Editors;
using Telerik.Windows.Controls;


namespace FD.Core.Test
{
    public partial class TestControl1
    {
        public TestControl1()
        {
            InitializeComponent();
            Loaded += sdfsd;
            BaseMultiControl.sss = aaaaaa;
        }

        private void sdfsd(object sender, RoutedEventArgs e)
        {
            aaa.SubControlNameList.Add("CCC");
            aaa.SubControlNameList.Add("DDD");
        }
        

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            BaseMultiControl.ToRunMode(this);
            return;
            var childs = this.ChildrenOfType<BaseMultiControl>().ToList();
            childs.ForEach(w =>
            {
                w.EditState = EditState.Run;
            });
        }
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            BaseMultiControl.ToEditMode(this);
            return;
            var childs = this.ChildrenOfType<BaseMultiControl>().ToList();
            childs.ForEach(w =>
            {
                w.EditState = EditState.Normal;
            });
        }

        public override string PageCode
        {
            get
            {
                return "MainPage1";
            }
        }
    }
}
