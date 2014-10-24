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

namespace FD.Core.Test.Views
{
    public partial class DeskTop
    {
        public DeskTop()
        {
            InitializeComponent();
        }

        public override string PageCode
        {
            get
            {
                return "DeskTop";
            }
        }
    }
}
