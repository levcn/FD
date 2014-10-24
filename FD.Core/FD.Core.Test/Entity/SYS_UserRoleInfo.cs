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
    public class SYS_UserRoleInfo : BaseListEntity
    {

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
                    OnPropertySelectedChanged(() => ID);
                }
            }
        }

        Guid? _userID;
        public Guid? UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                if (value != _userID)
                {
                    _userID = value;
                    OnPropertySelectedChanged(() => UserID);
                }
            }
        }

        Guid? _roleID;
        public Guid? RoleID
        {
            get
            {
                return _roleID;
            }
            set
            {
                if (value != _roleID)
                {
                    _roleID = value;
                    OnPropertySelectedChanged(() => RoleID);
                }
            }
        }

        int _userRoleType;
        public int UserRoleType
        {
            get
            {
                return _userRoleType;
            }
            set
            {
                if (value != _userRoleType)
                {
                    _userRoleType = value;
                    OnPropertySelectedChanged(() => UserRoleType);
                }
            }
        }
    }
}
