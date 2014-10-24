using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
//using STComponse.Annotations;
using STComponse.DB;


namespace STComponse.CFG
{
    /// <summary>
    /// 关联
    /// </summary>
    public class Relation:INotifyPropertyChanged
    {
        public const string 字典 = "字典";
        public const string 简单关联 = "简单关联";
        public const string 复杂关联 = "复杂关联";
        private string _objectPorertity;
        private string _relationType;
        private string _relationInfo;

        /// <summary>
        /// 属性代码
        /// </summary>
        public string ObjectPorertity
        {
            get
            {
                return _objectPorertity;
            }

            set
            {
                if (value == _objectPorertity) return;
                _objectPorertity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 关联名称
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// C#属性名称
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 关系类型(字典,简单关联,复杂关联)
        /// </summary>
        public string RelationType
        {
            get
            {
                return _relationType;
            }

            set
            {
                if (value == _relationType) return;
                _relationType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 关系描述
        /// Person_Course(PesonID,CouseID),Course("CuseID","Name")  
        /// Person_Role(PersonID,RoleID)，Dict_Role("ID","Name1")
        /// </summary>
        public string RelationInfo
        {
            get
            {
                return _relationInfo;
            }

            set
            {
                if (value == _relationInfo) return;
                _relationInfo = value;
                OnPropertyChanged();
                OnPropertyChanged("RelConfig");
            }
        }

        [XmlIgnore]
        public RelConfig RelConfig
        {
            get
            {
                return RelationInfo;
            }

            set
            {
                if (RelationType == "字典")
                {
                    RelationInfo = value.ToDictStr();
                }
                else
                {
                    RelationInfo = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

//        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
//            [CallerMemberName] 
            string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class RelationEx
    {
        /// <summary>
        /// 返回简单关联表对象
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static EDataObject GetSimpleRelDataObject(this Relation owner)
        {
            var relConfig = owner.RelConfig;
            return new EDataObject {
                ID = Guid.Empty,
                KeyTableName = relConfig.RelTableName,
                ObjectCode = relConfig.RelTableName,
                ObjectName = relConfig.RelTableName,
                Property = new ObservableCollection<Property>(new List<Property> {
                    new Property {
                        Code = "ID",
                        ColumnType = Property.GUID,
                        IsPrimaryKey = true,
                    },
                    new Property {
                        Code = relConfig.RelDictKey,
                        ColumnType = Property.GUID,
                        IsPrimaryKey = true,
                    },
                    new Property {
                        Code = relConfig.RelMasertKey,
                        ColumnType = Property.GUID,
                        IsPrimaryKey = true,
                    }
                })
            };
        }
    }
}
