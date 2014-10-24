using System;
using System.Collections.Generic;


namespace StaffTrain.Entity
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

        SYS_Role _sYS_Role;
        public SYS_Role SYS_Role
        {
            get
            {
                return _sYS_Role;
            }
            set
            {
                if (value != _sYS_Role)
                {
                    _sYS_Role = value;
                    OnPropertySelectedChanged(() => SYS_Role);
                }
            }
        }
    }
}