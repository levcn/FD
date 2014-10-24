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
using FD.Core.Test.ViewModel;


namespace FD.Core.Test.Views.Sys
{
    public partial class Org
    {
        public Org()
        {
            InitializeComponent();
            Loaded += loaded;
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
//            Sys_OrgModel d = new Sys_OrgModel();
//            d.ItemSource.ForEach(w=>w.IsSelected = true);
//            tree.DataSource = d.ItemSource;
        }
    }
}
