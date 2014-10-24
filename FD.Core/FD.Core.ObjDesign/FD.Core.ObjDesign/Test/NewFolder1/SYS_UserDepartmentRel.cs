using System;
using System.Collections.Generic;


namespace StaffTrain.Entity
{
    public class SYS_UserDepartmentRel : BaseListEntity
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

        List<SYS_UserDepartmentRel> _userID;
        public List<SYS_UserDepartmentRel> UserID
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

        Guid _departmentID;
        public Guid DepartmentID
        {
            get
            {
                return _departmentID;
            }
            set
            {
                if (value != _departmentID)
                {
                    _departmentID = value;
                    OnPropertySelectedChanged(() => DepartmentID);
                }
            }
        }
    }
}