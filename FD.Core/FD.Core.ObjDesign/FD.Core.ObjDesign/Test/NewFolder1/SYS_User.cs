using System;
using System.Collections.Generic;


namespace StaffTrain.Entity
{
    public class SYS_User : BaseListEntity
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

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertySelectedChanged(() => Name);
                }
            }
        }

        Guid? _userType;
        public Guid? UserType
        {
            get
            {
                return _userType;
            }
            set
            {
                if (value != _userType)
                {
                    _userType = value;
                    OnPropertySelectedChanged(() => UserType);
                }
            }
        }

        List<SYS_Department> _departments;
        public List<SYS_Department> Departments
        {
            get
            {
                return _departments;
            }
            set
            {
                if (value != _departments)
                {
                    _departments = value;
                    OnPropertySelectedChanged(() => Departments);
                }
            }
        }

        int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value != _age)
                {
                    _age = value;
                    OnPropertySelectedChanged(() => Age);
                }
            }
        }

        List<SYS_UserRoleInfo> _rolus;
        public List<SYS_UserRoleInfo> Rolus
        {
            get
            {
                return _rolus;
            }
            set
            {
                if (value != _rolus)
                {
                    _rolus = value;
                    OnPropertySelectedChanged(() => Rolus);
                }
            }
        }

        SYS_UserType _sYS_UserType;
        public SYS_UserType SYS_UserType
        {
            get
            {
                return _sYS_UserType;
            }
            set
            {
                if (value != _sYS_UserType)
                {
                    _sYS_UserType = value;
                    OnPropertySelectedChanged(() => SYS_UserType);
                }
            }
        }
    }
}