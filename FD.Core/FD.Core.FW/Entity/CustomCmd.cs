using System.Collections.Generic;


namespace Fw.Entity
{
    /// <summary>
    /// ×Ô¶¨ÒåÃüÁî
    /// </summary>
    public class CustomCmd
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public List<string> Params { get; set; }
        public List<string> ParamTypes { get; set; }

        public bool IsRelativePath { get; set; }
    }
}