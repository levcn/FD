using System;
using System.Collections.Generic;
using System.Windows;
using FD.Core.Test.Entity;
using SLControls.helpers;
using Telerik.Windows.Controls;


namespace FD.Core.Test.ViewModel
{
    public class RoleTypeModel : ViewModelBase
    {
        public RoleTypeModel()
        {
            initCommand();
            initDataSource();
        }

        private void initDataSource()
        {
            itemSource = new List<SYS_RoleType>();
            itemSource.Add(new SYS_RoleType{Name = "内部管理员",StartPage="Index1.aspx"});
            itemSource.Add(new SYS_RoleType { Name = "内部一般人员", StartPage = "Index2.aspx" });
            itemSource.Add(new SYS_RoleType { Name = "供货商", StartPage = "Index3.aspx" });
            itemSource.Add(new SYS_RoleType { Name = "供货商123", StartPage = "Index4.aspx" });
        }

        private void initCommand()
        {
            AddCommand = new DelegateCommand(o => AddItem(), o => true);
            DelCommand = new DelegateCommand(o => DelItem((SYS_RoleType) o), o => true);
            EditCommand = new DelegateCommand(o => EditItem((SYS_RoleType) o), o => true);
            EditRoleCommand = new DelegateCommand(o =>
            {
                EditRoleItem((SYS_RoleType) o, () =>
                {
                    initDataSource();
                });
            }, o => true);
        }
        
        private void EditRoleItem(SYS_RoleType sysRoleType,Action callback)
        {
            Busy = true;
            AlertHelper.Alert(Application.Current.RootVisual as FrameworkElement, "修改＂" + sysRoleType.Name + "＂的权限",ok: () =>
            {
                Busy = false;
                callback();
            });
            
        }

        private void EditItem(SYS_RoleType o)
        {}

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
                OnPropertyChanged(()=>Busy);
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

        List<SYS_RoleType> itemSource = new List<SYS_RoleType>();

        public List<SYS_RoleType> ItemSource
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
    }
}
