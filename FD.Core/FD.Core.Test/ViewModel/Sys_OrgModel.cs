using System;
using System.Collections.Generic;
using System.Windows;
using FD.Core.SLControl.Data;
using FD.Core.Test.Entity;
using SLControls.helpers;
using Telerik.Windows.Controls;


namespace FD.Core.Test.ViewModel
{
    public class Sys_OrgModel : ViewModelBase
    {
        public Sys_OrgModel()
        {
            initCommand();
            initDataSource();
        }

        private void initDataSource()
        {
            itemSource = new List<SYS_Org>();
            itemSource.Add(new SYS_Org
            {
                Name = "生产部",
                StartPage = "Index1.aspx",
                IsExpanded = true,
                Children = new List<INode> {
                    new SYS_Org { Name = "运行一班", StartPage = "Index2.aspx" }
                    ,new SYS_Org { Name = "运行二班", StartPage = "Index2.aspx" }
                }
            });
            itemSource.Add(new SYS_Org { Name = "运行部", StartPage = "Index2.aspx" });
            itemSource.Add(new SYS_Org { Name = "安检部", StartPage = "Index3.aspx" });
            itemSource.Add(new SYS_Org { Name = "检修部", StartPage = "Index4.aspx" });
            currentItem = itemSource[0];
        }

        private void initCommand()
        {
            AddCommand = new DelegateCommand(o => AddItem(), o => true);
            DelCommand = new DelegateCommand(o => DelItem((SYS_RoleType)o), o => true);
            EditCommand = new DelegateCommand(o => EditItem((SYS_RoleType)o), o => true);
            EditRoleCommand = new DelegateCommand(o =>
            {
                EditRoleItem((SYS_RoleType)o, () =>
                {
                    initDataSource();
                });
            }, o => true);
        }

        private void EditRoleItem(SYS_RoleType sysRoleType, Action callback)
        {
            Busy = true;
            AlertHelper.Alert(Application.Current.RootVisual as FrameworkElement, "修改＂" + sysRoleType.Name + "＂的权限", ok: () =>
            {
                Busy = false;
                callback();
            });

        }

        private void EditItem(SYS_RoleType o)
        { }

        private void DelItem(SYS_RoleType type)
        {

        }

        private void AddItem()
        {

        }

        private DelegateCommand addCommand;
        public DelegateCommand AddCommand
        {
            get
            {
                return addCommand;
            }
            set
            {
                addCommand = value;
            }
        }

        bool busy;
        public bool Busy
        {
            get
            {
                return busy;
            }
            set
            {
                busy = value;
                OnPropertyChanged(() => Busy);
            }
        }
        private DelegateCommand editRoleCommand;
        public DelegateCommand EditRoleCommand
        {
            get
            {
                return editRoleCommand;
            }
            set
            {
                editRoleCommand = value;
            }
        }
        private DelegateCommand delCommand;
        public DelegateCommand DelCommand
        {
            get
            {
                return delCommand;
            }
            set
            {
                delCommand = value;
            }
        }
        private DelegateCommand editCommand;
        public DelegateCommand EditCommand
        {
            get
            {
                return editCommand;
            }
            set
            {
                editCommand = value;
            }
        }

        List<SYS_Org> itemSource = new List<SYS_Org>();

        public List<SYS_Org> ItemSource
        {
            get
            {
                return itemSource;
            }
            set
            {
                itemSource = value;
                OnPropertyChanged(() => ItemSource);
            }
        }

        private SYS_Org currentItem;

        public SYS_Org CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                currentItem = value;
                OnPropertyChanged(() => CurrentItem);
            }
        }
    }
}
