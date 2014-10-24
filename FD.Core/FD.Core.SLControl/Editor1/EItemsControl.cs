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
using Telerik.Windows.Controls;
using ItemsControl = Telerik.Windows.Controls.ItemsControl;
using WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation;


namespace SLControls.Editor1
{
    public class EItemsControl : ItemsControl, IBrowseble
    {
        public EItemsControl()
        { 
            this.DefaultStyleKey = typeof(EItemsControl);
            EditCommand = new DelegateCommand(o => EditListItem(o), o => CheckHaveCancelProp(o));
            
        }

        public static readonly DependencyProperty WindowFollowItemProperty =
                DependencyProperty.Register("WindowFollowItem", typeof (bool), typeof (EItemsControl), new PropertyMetadata(true, (s, e) =>
                {
                    ((EItemsControl) s).OnWindowFollowItemChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnWindowFollowItemChanged(bool list)
        {}

        public bool WindowFollowItem
        {
            get
            {
                return (bool) GetValue(WindowFollowItemProperty);
            }
            set
            {
                SetValue(WindowFollowItemProperty, value);
            }
        }
        private Point windowDefaultPosition = new Point(0,0);
        List<EditLayer> rls = new List<EditLayer>();
        private bool EditListItem(object o)
        {
            Point p;
            if (WindowFollowItem || windowDefaultPosition.X == 0 && windowDefaultPosition.Y == 0)
            {
                var sde = this.ChildrenOfType<FrameworkElement>().Where(w => w.DataContext == o).ToList()[0];
                var gt = sde.TransformToVisual(null);
                p = gt.Transform(new Point(0, 0));
                p.X += sde.ActualWidth;
                windowDefaultPosition = p;
            }
            else
            {
                p = windowDefaultPosition;
            }

            
            return EditItem(o, p);
        }

        public bool EditItem(object o, Point p,Action<object> ok=null)
        {
            EditLayer rw;
            bool isnew = false;
            if (rls.Count > 0)
            {
                rw = rls[0];
            }
            else
            {
                rw = new EditLayer { Header = "编辑" };
                isnew = true;
            }
            rls.Add(rw);
            rw.Width = 500;
            rw.Height = 400;
            if (p.X != 0 && p.Y != 0)
            {
                rw.Left = p.X;
                rw.Top = p.Y;
            }
            else
            {
                rw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            FrameworkElement content = null;
            if (IsBrowsMode)
            {
                if (BrowserTemplate != null)
                {
                    content = BrowserTemplate.LoadContent() as FrameworkElement;
                }
                else
                {
                    if (EditorTemplate != null) content = EditorTemplate.LoadContent() as EditorPanel;
                }
            }
            else
            {
                if (EditorTemplate != null) content = EditorTemplate.LoadContent() as EditorPanel;
            }
//            if (EditorTemplate != null) content = EditorTemplate.LoadContent() as FrameworkElement;

            
            if (content != null)
            {
                content.ChildrenOfType<EditItemText>().ToList().ForEach(w => w.IsBrowsMode = IsBrowsMode);
                content.DataContext = o;
                rw.Content = content;
            }
            if (isnew)
            {
                if (UseModalWindow)
                {
                    rw.ShowDialog();
                }
                else
                {
                    rw.Show();
                }
            }
            else
            {
                rw.Focus();
            }
            rw.Closed += (s, e) =>
            {
                rls.Remove(rw);
                if (rw.DialogResult.HasValue && rw.DialogResult.Value)
                {
                    if (ok != null) ok(o);
                }
            };
            rw.LocationChanged += (s, e) =>
            {
                windowDefaultPosition.X = rw.Left;
                windowDefaultPosition.Y = rw.Top;
            };
            rw.PreviewClosed += (s, e) =>
            {
                if (rw.DialogResult.HasValue && rw.DialogResult.Value)
                {
                    content.ChildrenOfType<EditItemText>()
                            .ToList()
                            .ForEach(w =>
                            {
                                if (Validation.GetHasError(w))
                                {
                                    w.Focus();
                                    e.Cancel = true;
                                }
                            });
                    if (e.Cancel.HasValue && e.Cancel.Value)
                    {
                        return;
                    }
                    content.ChildrenOfType<EditItemText>()
                            .ToList()
                            .ForEach(w =>
                            {
                                if (Validation.GetHasError(w))
                                {
                                    e.Cancel = true;
                                }
                                var b = w.GetBindingExpression(EditItemText.TextProperty);
                                b.UpdateSource();
                            });
                }
            };
            
            if (o != null) return true;
            return false;
        }

        public static readonly DependencyProperty UseModalWindowProperty =
                DependencyProperty.Register("UseModalWindow", typeof (bool), typeof (EItemsControl), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((EItemsControl) s).OnUseModalWindowChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnUseModalWindowChanged(bool list)
        {}

        public bool UseModalWindow
        {
            get
            {
                return (bool) GetValue(UseModalWindowProperty);
            }
            set
            {
                SetValue(UseModalWindowProperty, value);
            }
        }

        public static readonly DependencyProperty BrowserTemplateProperty =
                DependencyProperty.Register("BrowserTemplate", typeof (DataTemplate), typeof (EItemsControl), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((EItemsControl) s).OnBrowsTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnBrowsTemplateChanged(DataTemplate list)
        {}

        public DataTemplate BrowserTemplate
        {
            get
            {
                return (DataTemplate) GetValue(BrowserTemplateProperty);
            }
            set
            {
                SetValue(BrowserTemplateProperty, value);
            }
        }
        private bool CheckHaveCancelProp(object o)
        {
            return true;
        }

        public static readonly DependencyProperty EditCommandProperty =
                DependencyProperty.Register("EditCommand", typeof(DelegateCommand), typeof(EItemsControl), new PropertyMetadata(default(DelegateCommand), (s, e) =>
                {
                    ((EItemsControl)s).OnAddCancelPropCommandChanged(e.NewValue as DelegateCommand);
                }));

        private void OnAddCancelPropCommandChanged(DelegateCommand list)
        {}

        public DelegateCommand EditCommand
        {
            get
            {
                return (DelegateCommand)GetValue(EditCommandProperty);
            }
            set
            {
                SetValue(EditCommandProperty, value);
            }
        }
        public static readonly DependencyProperty EditorTemplateProperty =
                DependencyProperty.Register("EditorTemplate", typeof (DataTemplate), typeof (EItemsControl), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((EItemsControl) s).OnContentTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnContentTemplateChanged(DataTemplate list)
        {}

        public DataTemplate EditorTemplate
        {
            get
            {
                return (DataTemplate) GetValue(EditorTemplateProperty);
            }
            set
            {
                SetValue(EditorTemplateProperty, value);
            }
        }
        public static readonly DependencyProperty HeaderTemplateProperty =
                DependencyProperty.Register("HeaderTemplate", typeof (DataTemplate), typeof (EItemsControl), new PropertyMetadata(default(DataTemplate), (s, e) =>
                {
                    ((EItemsControl) s).OnHeaderTemplateChanged(e.NewValue as DataTemplate);
                }));

        private void OnHeaderTemplateChanged(DataTemplate list)
        {}

        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate) GetValue(HeaderTemplateProperty);
            }
            set
            {
                SetValue(HeaderTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty IsBrowsModeProperty =
                DependencyProperty.Register("IsBrowsMode", typeof(bool), typeof(EItemsControl), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((EItemsControl)s).OnIsBrowsModeChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnIsBrowsModeChanged(bool list)
        {}

        public bool IsBrowsMode
        {
            get
            {
                return (bool)GetValue(IsBrowsModeProperty);
            }
            set
            {
                SetValue(IsBrowsModeProperty, value);
            }
        }
    }
}
