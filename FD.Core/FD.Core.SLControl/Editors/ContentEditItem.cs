using System.Windows;
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    public class ContentEditItem : BaseEditItem
    {

        public static readonly DependencyProperty WatermarkContentProperty =
                DependencyProperty.Register("WatermarkContent", typeof(object), typeof(ContentEditItem), new PropertyMetadata(default(object)));

        /// <summary>
        /// 文本框中的水印
        /// </summary>
        public object WatermarkContent
        {
            get
            {

                return (object)GetValue(WatermarkContentProperty);
            }
            set
            {
                SetValue(WatermarkContentProperty, value);
            }
        }

        public static readonly DependencyProperty WatermarkTemplateProperty =
                DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(ContentEditItem), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate WatermarkTemplate
        {
            get
            {
                return (DataTemplate)GetValue(WatermarkTemplateProperty);
            }
            set
            {
                SetValue(WatermarkTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty TextAlignmentProperty =
                DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(ContentEditItem), new PropertyMetadata(default(TextAlignment)));

        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        public static readonly DependencyProperty SelectionOnFocusProperty =
                DependencyProperty.Register("SelectionOnFocus", typeof(SelectionOnFocus), typeof(ContentEditItem), new PropertyMetadata(default(SelectionOnFocus)));

        public SelectionOnFocus SelectionOnFocus
        {
            get
            {
                return (SelectionOnFocus)GetValue(SelectionOnFocusProperty);
            }
            set
            {
                SetValue(SelectionOnFocusProperty, value);
            }
        }

    }
}
