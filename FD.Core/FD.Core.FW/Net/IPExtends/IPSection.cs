using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Fw.Net.IPExtends
{
    /// <summary>
    /// IP区间
    /// </summary>
    public class IPSection
    {
        public uint Start { get; set; }
        public uint End { get; set; }
        [XmlIgnore]
        public string StringStart
        {
            get
            {
                return Start.ToIPString();
            }
            set
            {
                Start = value.ToIPInt();
            }
        }
        [XmlIgnore]
        public string StringEnd
        {
            get
            {
                return End.ToIPString();
            }
            set
            {
                End = value.ToIPInt();
            }
        }
    }
    public static class IPSectionExtends
    {
        public static bool HaveError(this IPSection ipSection)
        {
            if (ipSection.Start >= ipSection.End) return true;
            return false;
        }
    }
}