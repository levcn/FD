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

namespace StaffTrain.FwClass.DataClientTools
{
    /// <summary>
    /// 存储过程的参数名和值
    /// </summary>
    public class STParamete
    {
        public STParamete()
        {}

        public STParamete(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
