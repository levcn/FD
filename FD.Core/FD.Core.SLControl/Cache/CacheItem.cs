using System;
using Fw.Caches;


namespace SLControls.Cache
{
    /// <summary>
    /// ������Ŀ
    /// </summary>
    public class CacheItem
    {
        public object Content { get; set; }

        public DateTime OverDate { get; set; }

        public event EventHandler<OverDateEventArgs> OverDateBefore;

        protected virtual void OnOverDateBefore(OverDateEventArgs e)
        {
            EventHandler<OverDateEventArgs> handler = OverDateBefore;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// ���ض����Ƿ����,���û�й���(��������),�򷵻�false,��������򷵻�true
        /// </summary>
        /// <returns></returns>
        public bool CheckOverDateWithEvent()
        {
            if(CheckOverDate())
            {
                OverDateEventArgs ea = new OverDateEventArgs();
                ea.Content = Content;
                OnOverDateBefore(ea);
                if(ea.Cancel)
                {
                    OverDate = ea.NewOverDate;
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool CheckOverDate()
        {
            return DateTime.Now > OverDate;
        }
    }
}