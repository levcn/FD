using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;


namespace SLControls.Editor1
{

    [TemplateVisualState(GroupName = "SelectionStates", Name = "UnSelected")] //可以选择
    [TemplateVisualState(GroupName = "SelectionStates", Name = "Selected")] //已经选择
    [TemplateVisualState(GroupName = "MouseStates", Name = "MouseOver")] //已经选择
    [TemplateVisualState(GroupName = "MouseStates", Name = "MouseLeave")] //已经选择
    public class ItemContainer : ContentControl
    {
        public ItemContainer()
        {
            DefaultStyleKey = typeof(ItemContainer);
            if (!DesignerProperties.IsInDesignTool)
            {
                AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(mouseLeftButtonUp), true);
                MouseEnter += ItemContainer_MouseEnter;
                MouseLeave += ItemContainer_MouseLeave;
                //                AddHandler(, new MouseEventHandler(bbb), true);
            }
        }

        public static readonly DependencyProperty EditCommandParameterProperty =
                DependencyProperty.Register("EditCommandParameter", typeof (object), typeof (ItemContainer), new PropertyMetadata(default(object), (s, e) =>
                {
                    ((ItemContainer) s).OnEditCommandParameterChanged(e.NewValue as object);
                }));

        private void OnEditCommandParameterChanged(object list)
        {}

        public object EditCommandParameter
        {
            get
            {
                return (object) GetValue(EditCommandParameterProperty);
            }
            set
            {
                SetValue(EditCommandParameterProperty, value);
            }
        }
        public static readonly DependencyProperty DeleteCommandParameterProperty =
                DependencyProperty.Register("DeleteCommandParameter", typeof (object), typeof (ItemContainer), new PropertyMetadata(default(object), (s, e) =>
                {
                    ((ItemContainer) s).OnDeleteCommandParameterChanged(e.NewValue as object);
                }));

        private void OnDeleteCommandParameterChanged(object list)
        {}

        public object DeleteCommandParameter
        {
            get
            {
                return (object) GetValue(DeleteCommandParameterProperty);
            }
            set
            {
                SetValue(DeleteCommandParameterProperty, value);
            }
        }
        public static readonly DependencyProperty DeleteCommandProperty =
                DependencyProperty.Register("DeleteCommand", typeof (ICommand), typeof (ItemContainer), new PropertyMetadata(default(ICommand), (s, e) =>
                {
                    ((ItemContainer) s).OnDeleteCommandChanged(e.NewValue as ICommand);
                }));

        private void OnDeleteCommandChanged(ICommand list)
        {}

        public ICommand DeleteCommand
        {
            get
            {
                return (ICommand) GetValue(DeleteCommandProperty);
            }
            set
            {
                SetValue(DeleteCommandProperty, value);
            }
        }
        public static readonly DependencyProperty EditCommandProperty =
                DependencyProperty.Register("EditCommand", typeof (ICommand), typeof (ItemContainer), new PropertyMetadata(default(ICommand), (s, e) =>
                {
                    ((ItemContainer) s).OnEditCommandChanged(e.NewValue as ICommand);
                }));

        private void OnEditCommandChanged(ICommand list)
        {}

        public ICommand EditCommand
        {
            get
            {
                return (ICommand) GetValue(EditCommandProperty);
            }
            set
            {
                SetValue(EditCommandProperty, value);
            }
        }
        public static readonly DependencyProperty MouseOverProperty =
                DependencyProperty.Register("MouseOver", typeof(bool), typeof(ItemContainer), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((ItemContainer)s).OnMouseOverChanged(e.NewValue is bool ? (bool)e.NewValue : false);
                }));

        private void OnMouseOverChanged(bool list)
        {

        }

        public bool MouseOver
        {
            get
            {
                return (bool)GetValue(MouseOverProperty);
            }
            set
            {
                SetValue(MouseOverProperty, value);
            }
        }
        private void ItemContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseOver = false;
            GoToState();
        }

        private void ItemContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseOver = true;
            GoToState();
        }

        private void mouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (IsEnabled)
            {
                var border = e.OriginalSource as Border;
                if (border != null && border.Name == "PART_ItemContainer_Border")
                {
                    Selected = !Selected;
                    var listPanel = this.ParentOfType<ListPanel>();
                    if (listPanel!=null)
                    {
                        listPanel.InvalidateDeleteEnabled();
                    }
                    if (!MultiSelect && !KeyboardModifiers.IsControlDown)
                    {
                        var listParent = this.ParentOfType<ItemsPresenter>();
                        if (listParent != null)
                        {
                            var otherList = listParent.ChildrenOfType<ItemContainer>().Where(w => w != this).ToList();
                            otherList.ForEach(w =>
                            {
                                w.Selected = false;
                            });
                        }
                    }
                }
            }
        }

        public static readonly DependencyProperty MultiSelectProperty =
                DependencyProperty.Register("MultiSelect", typeof (bool), typeof (ItemContainer), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((ItemContainer) s).OnMultiSelectChanged(e.NewValue is bool ? (bool) e.NewValue : false);
                }));

        private void OnMultiSelectChanged(bool list)
        {}

        public bool MultiSelect
        {
            get
            {
                return (bool) GetValue(MultiSelectProperty);
            }
            set
            {
                SetValue(MultiSelectProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedProperty =
                DependencyProperty.Register("Selected", typeof(bool), typeof(ItemContainer), new PropertyMetadata(default(bool), (s, e) =>
                {
                    ((ItemContainer)s).OnSelectedChanged(e.NewValue is bool ? (bool)e.NewValue : false);
                }));

        private void OnSelectedChanged(bool list)
        {
            GoToState();
        }

        public bool Selected
        {
            get
            {
                return (bool)GetValue(SelectedProperty);
            }
            set
            {
                SetValue(SelectedProperty, value);
            }
        }
        void GoToState()
        {
            if (Selected)
            {
                VisualStateManager.GoToState(this, "Selected", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "UnSelected", true);

            }
            if (MouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "MouseLeave", true);

            }
        }

        private Button EditButton, DeleteButton;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            EditButton = GetTemplateChild("PART_EditButton") as Button;
            DeleteButton = GetTemplateChild("PART_DeleteButton") as Button;
            GoToState();
        }
    }
}
