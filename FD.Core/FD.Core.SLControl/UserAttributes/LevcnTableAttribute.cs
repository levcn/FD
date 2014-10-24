using System;


namespace StaffTrain.FwClass.UserAttributes
{
    public class LevcnTableAttribute : Attribute
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
