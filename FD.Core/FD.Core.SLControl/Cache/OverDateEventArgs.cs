using System;


namespace Fw.Caches
{
    /// <summary>
    /// �����¼�����
    /// </summary>
    public class OverDateEventArgs:EventArgs
    {
        public object Content { get; set; }
        public bool Cancel { get; set; }
        public DateTime NewOverDate { get; set; }
    }
}