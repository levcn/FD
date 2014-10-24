using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STComponse.DB;


namespace ProjectCreater.SObjects
{
    public class Test
    {
        public static void TestCompareDB()
        {
            Guid UserTableGuid = Guid.NewGuid();
            Guid UserTypeTableGuid = Guid.NewGuid();

            Guid UserTableIDGuid = Guid.NewGuid();
            Guid UserTableNameGuid = Guid.NewGuid();

            Guid UserTypeIDGuid = Guid.NewGuid();
            Guid UserTypeNameGuid = Guid.NewGuid();

            var a1 = new EDB {
                ETables = new List<ETable> {
                    new ETable {
                        TableName = "User",
                        ID = UserTableGuid,
                        EFields = new List<EField> {
                            new EField {
                                ID = UserTableIDGuid,
                                
                                DefaultValue = "newid()",
                                FieldType = "GUID",
                                Name = "ID",
                                IsPrimary = true,
                                Remark = "UserTableIDGuid",
                            },
                            new EField {
                                ID = UserTableNameGuid,
                                Name = "Name",
                                DefaultValue = "",
                                FieldType = "字符串",
                                Length = 200,
                            },
                        }
                    }
                }
            };
            var b1 = a1.Clone() as EDB;
//            b1.ETables[0].EFields[1].Length = 100;
//            b1.ETables[0].EFields[1].Name = "Name1";
//            b1.ETables[0].EFields.RemoveAt(1);
            b1.ETables[0].EFields.Add( new EField {
                                ID = Guid.NewGuid(),
                                Name = "Name123",
                                DefaultValue = "",
                                FieldType = "字符串",
                                Length = 200,
                            });
            b1.ETables.Add(new ETable {

                TableName = "UserType",
                ID = UserTypeIDGuid,
                EFields = new List<EField> {
                    new EField {
                        ID = UserTypeIDGuid,

                        DefaultValue = "newid()",
                        FieldType = "GUID",
                        Name = "ID",
                        IsPrimary = true,
                        Remark = "UserTypeIDGuid",
                    },
                    new EField {
                        ID = UserTypeNameGuid,
                        Name = "Name",
                        DefaultValue = "",
                        FieldType = "字符串",
                        Length = 200,
                    },
                }
            });
            var re = EDBCompare.Compare(a1, b1);
            try
            {
                var strrrrrrrr = (string) re;
            }
            catch (Exception eee)
            {
                var werwerw=eee.GetBaseException().ToString();
            }
        }
    }
}
