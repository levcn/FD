using System.Collections.ObjectModel;
using System.ComponentModel;


namespace STComponse.CFG
{
    /// <summary>
    /// 存储过程
    /// </summary>
    public class StoredProcedure : INotifyPropertyChanged, IPositioning
    {

        public ObservableCollection<SPParameter> Parameters { get; set; }

        /// <summary>
        /// 存储过程的内容
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
                OnPropertyChanged("Remark");
            }
        }

        public string ObjectCode
        {
            get
            {
                return _objectCode;
            }
            set
            {
                _objectCode = value;
                OnPropertyChanged("Code");

            }
        }

        private string _objectName;
        private string _objectCode;
        private string _content;
        private string _remark;

        public string ObjectName
        {
            get
            {
                return _objectName;
            }
            set
            {
                _objectName = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Left { get; set; }
        public int Top { get; set; }
    }
}