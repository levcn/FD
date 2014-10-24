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
using FD.Core.SLControl.Data;
using Telerik.Windows.Controls;


namespace FD.Core.SLControl.Controls
{
    /// <summary>
    /// 对于列表中可以选择条目的模板选择类
    /// </summary>
    public class SelectableTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate SelectedTemplate { get; set; }

        public HierarchicalDataTemplate UnSelectedTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            base.SelectTemplate(item, container);

            if (item is IIsSelected)
            {
                IIsSelected selectTemplate = (item as IIsSelected);
                if (selectTemplate.IsSelected)
                {
                    return SelectedTemplate;
                }
                return UnSelectedTemplate;
            }
            return UnSelectedTemplate;
        }
    }
}
