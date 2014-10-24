using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace STComponse.CFG
{
    /// <summary>
    /// 对象配置
    /// </summary>
    public class FwConfig
    {
        public FwConfig()
        {
            DataObjects = new ObservableCollection<EDataObject>();
            DictObject = new ObservableCollection<EDataObject>();
            StoredProcedures = new ObservableCollection<StoredProcedure>();
            VirtualEDataObject = new ObservableCollection<EDataObject>();
            SYSObject = GetSYSObject();
        }

        static ObservableCollection<EDataObject> GetSYSObject()
        {
            return new ObservableCollection<EDataObject> {
                new EDataObject {
                    ObjectName="系统配置表1",
                    ObjectCode="_SYS_Table1",
                    KeyTableName="_SYS_Table1",
                    Property = new ObservableCollection<Property> {
                        new Property {
                            Code = "ID",
                            Name="ID",
                            ColumnType="GUID",
                            IsPrimaryKey=true,
                        }, 
                        new Property {
                            Code = "Name",
                            Name="Name",
                            ColumnType="字符串",
                            VarcharConfig = new VarcharConfig{Length = 200},
                        },
                        new Property {
                            Code = "Code",
                            Name="Code",
                            ColumnType="整数",
                            ColumnSize="10,3",
                        },
                        new Property {
                            Code = "Content",
                            Name="Content",
                            ColumnType="备注",
                        },

                    }
                },
            };
        }
        /// <summary>
        /// 数据对象
        /// </summary>
        public ObservableCollection<EDataObject> DataObjects { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 字典对象
        /// </summary>
        public ObservableCollection<EDataObject> DictObject { get; set; }

        /// <summary>
        /// 虚拟对象
        /// </summary>
        public ObservableCollection<EDataObject> VirtualEDataObject { get; set; }

        /// <summary>
        /// 系统表
        /// </summary>
        public ObservableCollection<EDataObject> SYSObject { get; private set; }

        /// <summary>
        /// 存储过程列表
        /// </summary>
        public ObservableCollection<StoredProcedure> StoredProcedures { get; set; }
        

        /// <summary>
        /// 版本号,从1开始递增
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// 版本的名称
        /// </summary>
        public string VersionName { get; set; }

    }
}