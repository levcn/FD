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


namespace FD.Core.Test.Entity
{
    public class SYS_RoleType : BaseListEntity
    {
        public SYS_RoleType()
        {
            ID = Guid.NewGuid();
        }

        Guid _iD;
        public Guid ID
        {
            get
            {
                return _iD;
            }
            set
            {
                if (value != _iD)
                {
                    _iD = value;
                    OnPropertySelectedChanged();
                }
            }
        }

        private string name;
        private int isDeleted;
        private int orderID;
        private string startPage;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertySelectedChanged();

            }
        }

        public int IsDeleted
        {
            get
            {
                return isDeleted;
            }
            set
            {
                isDeleted = value;
                OnPropertySelectedChanged();

            }
        }

        public int OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                orderID = value;
                OnPropertySelectedChanged();

            }
        }

        public string StartPage
        {
            get
            {
                return startPage;
            }
            set
            {
                startPage = value;
                OnPropertySelectedChanged();
            }
        }
    }
}
