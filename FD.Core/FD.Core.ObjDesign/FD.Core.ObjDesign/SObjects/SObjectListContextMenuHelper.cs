using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using STComponse.ObjectConfig;
using STComponse.CFG;


namespace ProjectCreater.SObjects
{
    public class SObjectListContextMenuHelper
    {
        public static void SetContextMenu(DependencyObject control,EDataObject obj)
        {
            ContextMenuService.SetContextMenu(control,GetContextMenu(obj));
        }

        private static ContextMenu GetContextMenu(EDataObject sObject)
        {
            MenuItem mi1 = new MenuItem {
                Header = "编辑(&E)"
            };
            mi1.Click+= (s, e) =>
            {
                MessageBox.Show("编辑");
            };

            MenuItem mi2 = new MenuItem
            {
                Header = "删除(&D)"
            };

            mi2.Click += (s, e) =>
            {
                MessageBox.Show("删除");
            };
            return new ContextMenu {
                Items = {
                    mi1,
                    mi2,
                }
            };
        }
    }
}
