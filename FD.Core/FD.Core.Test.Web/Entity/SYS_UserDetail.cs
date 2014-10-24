using System;


namespace FD.Core.Test.Entity
{
    public class SYS_UserDetail 
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

        int _orderID;
        public int OrderID
        {
            get
            {
                return _orderID;
            }
            set
            {
                if (value != _orderID)
                {
                    _orderID = value;
                }
            }
        }

        int _isCancel;
        public int IsCancel
        {
            get
            {
                return _isCancel;
            }
            set
            {
                if (value != _isCancel)
                {
                    _isCancel = value;
                }
            }
        }

        int _address;
        public int Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != _address)
                {
                    _address = value;
                }
            }
        }
    }
}
