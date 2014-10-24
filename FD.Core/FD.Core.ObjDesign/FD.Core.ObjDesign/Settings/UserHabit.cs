using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Xml.Serialization;


namespace ProjectCreater.Settings
{
    public class UserHabit
    {
        public UserHabit()
        {
            IntValues = new Dictionary<string,int?>();
            StrValues = new Dictionary<string, string>();
            DoubleValues = new Dictionary<string, double?>();
            LongValues = new Dictionary<string, long?>();
            DateValues = new Dictionary<string, DateTime?>();
            MainPage = new MainPageConfig(this);
            ObjectMainEdit = new ObjectMainEditConfig(this);
        }
        public Dictionary<string,int?> IntValues { get; set; }
        public Dictionary<string, string> StrValues { get; set; }
        public Dictionary<string, DateTime?> DateValues { get; set; }
        public Dictionary<string, double?> DoubleValues { get; set; }
        public Dictionary<string, long?> LongValues { get; set; }

        [XmlIgnore]
        public MainPageConfig MainPage { get; set; }
        public ObjectMainEditConfig ObjectMainEdit { get; set; }
    }
}