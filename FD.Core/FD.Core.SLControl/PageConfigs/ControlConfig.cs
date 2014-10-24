using System.Collections.Generic;
using FD.Core.SLControl.Data;
using SLComponse.Validate;
using SLControls.Editors;
using StaffTrain.FwClass.DataClientTools;


namespace SLControls.PageConfigs
{
    /// <summary>
    /// 控件配置
    /// </summary>
    public class ControlConfig:BaseListEntity
    {
        private List<ColumnValidataConfig> _validateConfigs;
        private ControlDataConfig _dataConfig;
        private List<PropItem> _itemConfig;
        private string _entityCode;
        private List<ControlConfig> subControlConfigs;

        public ControlConfig()
        {
            ItemConfig = new List<PropItem>();
            DataConfig = new ControlDataConfig();
            ValidateConfigs = new List<ColumnValidataConfig>();
            subControlConfigs = new List<ControlConfig>();
        }

        public List<ControlConfig> SubControlConfigs
        {
            get
            {
                return subControlConfigs;
            }
            set
            {
                subControlConfigs = value;
                OnPropertyChanged(() => SubControlConfigs);
            }
        }

        /// <summary>
        /// 名称,控件的Name
        /// </summary>
        public string Name { get; set; }

//        /// <summary>
//        /// 配置的内容
//        /// </summary>
//        public string Content { get; set; }

        /// <summary>
        /// 需要绑定的对象代码(页面上控件对象的Name)
        /// </summary>
        public string ObjectCode { get; set; }

//        /// <summary>
//        /// 该控件的数据源是页面的哪个对象;Object1-Object10
//        /// </summary>
//        public string PageObject { get; set; }

        /// <summary>
        /// 实体类名
        /// </summary>
        public string EntityCode
        {
            get
            {
                return _entityCode;
            }
            set
            {
                _entityCode = value;
                OnPropertyChanged(() => EntityCode);

            }
        }

        /// <summary>
        /// 控件的设置
        /// </summary>
        public List<PropItem> ItemConfig
        {
            get
            {
                return _itemConfig;
            }
            set
            {
                _itemConfig = value;
                OnPropertyChanged(() => ItemConfig);

            }
        }

        /// <summary>
        /// 数据来源设置
        /// </summary>
        public ControlDataConfig DataConfig
        {
            get
            {
                return _dataConfig;
            }
            set
            {
                _dataConfig = value;
                OnPropertyChanged(() => DataConfig);

            }
        }

        /// <summary>
        /// 控件的验证
        /// </summary>
        public List<ColumnValidataConfig> ValidateConfigs
        {
            get
            {
                return _validateConfigs;
            }
            set
            {
                _validateConfigs = value;
                OnPropertyChanged(() => ValidateConfigs);
            }
        }
    }

    /// <summary>
    /// 数据来源设置
    /// </summary>
    public class ControlDataConfig
    {
        public ControlDataConfig()
        {
            Parametes = new List<STParamete>();
        }
        /// <summary>
        /// 和页面的哪个对象绑定
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 1数据,2存储过程
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string SPName { get; set; }

        /// <summary>
        /// 存储过程的参数设置
        /// </summary>
        public List<STParamete> Parametes { get; set; }

        /// <summary>
        /// 该控件的数据源是页面的哪个对象;Object1-Object10
        /// </summary>
        public string PageObject { get; set; }

    }
}
