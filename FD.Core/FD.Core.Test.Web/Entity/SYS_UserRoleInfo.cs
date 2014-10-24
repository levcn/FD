using System;


namespace FD.Core.Test.Entity
{
    public class SYS_UserRoleInfo 
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
                }
            }
        }
    }
}
