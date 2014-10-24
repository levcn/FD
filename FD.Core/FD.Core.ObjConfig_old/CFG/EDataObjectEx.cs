using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STComponse.GCode;


namespace STComponse.CFG
{
    public static class EDataObjectEx
    {
        public static BigFieldValue GetBigFieldByPropertyID(this EDataObject obj, Guid propID)
        {
            return obj.BigFields.FirstOrDefault(w => w.ID == propID);
        }
        public static Property GetDisplay(this EDataObject obj)
        {
            return obj.Property.FirstOrDefault(w => w.IsShow);
        }
        public static Property GetID(this EDataObject obj)
        {
            return obj.Property.FirstOrDefault(w => w.IsPrimaryKey);
        }

        /// <summary>
        /// 返回类型的基本字段
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Property> GetBaseProperties(this EDataObject obj)
        {
            return obj.Property.Where(w => w.ColumnType != Property.关联).ToList();
        }
    }
}
