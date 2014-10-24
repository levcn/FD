using System;
using System.Collections.Generic;


namespace StaffTrain.Entity
{
    public class SYS_UserType : BaseListEntity
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
                    OnPropertySelectedChanged(() => OrderID);
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
                    OnPropertySelectedChanged(() => IsCancel);
                }
            }
        }
    }
}