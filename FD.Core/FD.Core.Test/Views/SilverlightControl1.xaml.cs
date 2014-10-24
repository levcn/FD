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


namespace FD.Core.Test.Views
{
    public partial class SilverlightControl1
    {
        public SilverlightControl1()
        {
            InitializeComponent();
        }

        public override string PageCode
        {
            get
            {
                return "SilverlightControl1";
            }
        }
    }
}
