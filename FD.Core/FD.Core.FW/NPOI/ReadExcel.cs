using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using Fw.Reflection;
using Fw.UserAttributes;
using NPOI.DDF;
using NPOI.HSSF.Record;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using aa=NPOI.SS;
using NPOI.Util;
using NPOI.POIFS;

namespace ServerFw.NPOI
{
    public class ReadExcel
    {
        IWorkbook hssfworkbook;
        public void InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = WorkbookFactory.Create(file);
            }
        }
        public DataSet ConvertToDataTable(out  List<MyPictureData> list)
        {
            DataSet ds = new DataSet();
            ISheet sheet = hssfworkbook.GetSheetAt(0);

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();
            //获取sheet的首行
            IRow headerRow = sheet.GetRow(0);

            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                dt.Columns.Add(headerRow.GetCell(i).StringCellValue);
            }
            //获取内容

            IRow row = null;
            DataRow dr = null;
            string str = "";
            list = getAllPictures(hssfworkbook,out str);

            for (int j = 1; j <= sheet.LastRowNum; j++)
            {
                row = sheet.GetRow(j);
                if (row != null)
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < cellCount; i++)
                    {
                        ICell cell = row.GetCell(i);

                        if (cell == null)
                        {
                           if (i<dt.Columns.Count)
                               dr[i] = "";
                        }
                        else
                        {
                            if (i < dt.Columns.Count)
                              dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }

            ds.Tables.Add(dt);

            dt = null;
            dt = new DataTable();
            dt.Columns.Add("TableName");
            dt.Columns.Add("Error");
            DataRow drow = dt.NewRow();
            drow["TableName"] = sheet.SheetName;
            drow["Error"] = str;
            dt.Rows.Add(drow);
            
            ds.Tables.Add(dt);
            return ds;
        }


        public static List<MyPictureData> getAllPictures(IWorkbook workbook,out string str)
        {

            List<MyPictureData> list = new List<MyPictureData>();

            IList listPicture = workbook.GetAllPictures();
             str = "";
            List<ClientAnchorInfo> clientAnchorRecords = getClientAnchorRecords(workbook,out str);
            if (str.Trim().Length == 0)
            {
                if (listPicture.Count != clientAnchorRecords.Count)
                {
                    str += "【解析文件中的图片信息出错，找到的图片数量和图片位置信息数量不匹配】";
                }


                for (int i = 0; i < listPicture.Count; i++)
                {
                    HSSFPictureData pictureData = listPicture[i] as HSSFPictureData;
                    ClientAnchorInfo anchor = clientAnchorRecords[i] as ClientAnchorInfo;
                    ISheet sheet = anchor.sheet;
                    EscherClientAnchorRecord clientAnchorRecord = anchor.clientAnchorRecord;
                    list.Add(new MyPictureData(workbook, sheet, pictureData, clientAnchorRecord));
                }
            }

            return list;
        }

        private class ClientAnchorInfo
        {
            public ISheet sheet;
            public EscherClientAnchorRecord clientAnchorRecord;

            public ClientAnchorInfo(ISheet sheet, EscherClientAnchorRecord clientAnchorRecord)
            {
                this.sheet = sheet;
                this.clientAnchorRecord = clientAnchorRecord;
            }
        }
        private static List<ClientAnchorInfo> getClientAnchorRecords(IWorkbook workbook,out string str)
        {
            List<ClientAnchorInfo> list = new List<ClientAnchorInfo>();
            str = "";
            EscherAggregate drawingAggregate = null;
            ISheet sheet = null;
            List<EscherRecord> recordList = null;
            IEnumerator<EscherRecord> recordIter = null;
            sheet = workbook.GetSheetAt(0) as HSSFSheet;
            if (sheet == null)
            {
                IList l=workbook.GetAllPictures();
                if(l.Count>0)
                   str = "【你上传的版本是excel2007，图片暂时读不出来，可以另存为Excel 2003再上传导入】";
                //sheet = workbook.GetSheetAt(0);
                //drawingAggregate = ((XSSFSheet)sheet).DrawingEscherAggregate;
            }
            else
            {
                drawingAggregate = ((HSSFSheet)sheet).DrawingEscherAggregate;
                if (drawingAggregate != null)
                {
                    recordList = drawingAggregate.EscherRecords;
                    recordIter = recordList.GetEnumerator();
                    while (recordIter.MoveNext())
                    {
                        getClientAnchorRecords(sheet, recordIter.Current, 1, list);
                    }
                }
            }

            return list;
        }

        private static void getClientAnchorRecords(ISheet sheet, EscherRecord escherRecord, int level, List<ClientAnchorInfo> list)
        {
            List<EscherRecord> recordList = null;
            IEnumerator<EscherRecord> recordIter = null;
            EscherRecord childRecord = null;
            recordList = escherRecord.ChildRecords;
            recordIter = recordList.GetEnumerator();
            while (recordIter.MoveNext())
            {
                childRecord = recordIter.Current;
                if (childRecord is EscherClientAnchorRecord)
                {
                    ClientAnchorInfo e = new ClientAnchorInfo(sheet, (EscherClientAnchorRecord)childRecord);
                    list.Add(e);
                }
                if (childRecord.ChildRecords.Count() > 0)
                {
                    getClientAnchorRecords(sheet, childRecord, level + 1, list);
                }
            }
        }
    }

    public class MyPictureData
    {
        private IWorkbook workbook;
        private ISheet sheet;
        private HSSFPictureData pictureData;
        private EscherClientAnchorRecord clientAnchor;

        public MyPictureData(IWorkbook workbook, ISheet sheet, HSSFPictureData pictureData, EscherClientAnchorRecord clientAnchor)
        {
            this.workbook = workbook;
            this.sheet = sheet;
            this.pictureData = pictureData;
            this.clientAnchor = clientAnchor;
        }

        public IWorkbook getWorkbook()
        {
            return workbook;
        }

        public ISheet getSheet()
        {
            return sheet;
        }

        public EscherClientAnchorRecord getClientAnchor()
        {
            return clientAnchor;
        }

        public HSSFPictureData getPictureData()
        {
            return pictureData;
        }

        public byte[] getData()
        {
            return pictureData.Data;
        }

        public String suggestFileExtension()
        {
            return pictureData.SuggestFileExtension();
        }

        /**
         * 推测图片中心所覆盖的单元格，这个值不一定准确，但通常有效
         * 
         * @return the row0
         */
        public short getRow0()
        {
            int row1 = getRow1();
            int row2 = getRow2();
            if (row1 == row2)
            {
                return (short)row1;
            }

            int[] heights = new int[row2 - row1 + 1];
            for (int i = 0; i < heights.Length; i++)
            {
                heights[i] = getRowHeight(row1 + i);
            }

            // HSSFClientAnchor 中 dx 只能在 0-1023 之间,dy 只能在 0-255 之间
            // 表示相对位置的比率，不是绝对值
            int dy1 = getDy1() * heights[0] / 255;
            int dy2 = getDy2() * heights[heights.Length - 1] / 255;
            return (short)(getCenter(heights, dy1, dy2) + row1);
        }
        private short getRowHeight(int rowIndex)
        {
            HSSFRow row = (HSSFRow)sheet.GetRow(rowIndex);
            short h = row == null ? sheet.DefaultRowHeight : row.Height;
            return h;
        }

        /**
         * 推测图片中心所覆盖的单元格，这个值不一定准确，但通常有效
         * 
         * @return the col0
         */
        public short getCol0()
        {
            short col1 = getCol1();
            short col2 = getCol2();

            if (col1 == col2)
            {
                return col1;
            }

            int[] widths = new int[col2 - col1 + 1];
            for (int i = 0; i < widths.Length; i++)
            {
                widths[i] = sheet.GetColumnWidth(col1 + i);
            }

            // HSSFClientAnchor 中 dx 只能在 0-1023 之间,dy 只能在 0-255 之间
            // 表示相对位置的比率，不是绝对值
            int dx1 = getDx1() * widths[0] / 1023;
            int dx2 = getDx2() * widths[widths.Length - 1] / 1023;

            return (short)(getCenter(widths, dx1, dx2) + col1);
        }

        /**
         * 给定各线段的长度，以及起点相对于起点段的偏移量，终点相对于终点段的偏移量，
         * 求中心点所在的线段
         * 
         * @param a the a 各线段的长度
         * @param d1 the d1 起点相对于起点段
         * @param d2 the d2 终点相对于终点段的偏移量
         * 
         * @return the center
         */
        protected static int getCenter(int[] a, int d1, int d2)
        {
            // 线段长度
            int width = a[0] - d1 + d2;
            for (int i = 1; i < a.Length - 1; i++)
            {
                width += a[i];
            }

            // 中心点位置
            int c = width / 2 + d1;
            int x = a[0];
            int cno = 0;

            while (c > x)
            {
                x += a[cno];
                cno++;
            }

            return cno;
        }

        /**
         * 左上角所在列
         * 
         * @return the col1
         */
        public short getCol1()
        {
            return clientAnchor.Col1;
        }

        /**
         * 右下角所在的列
         * 
         * @return the col2
         */
        public short getCol2()
        {
            return clientAnchor.Col2;
        }

        /**
         * 左上角的相对偏移量
         * 
         * @return the dx1
         */
        public short getDx1()
        {
            return clientAnchor.Dx1;
        }

        /**
         * 右下角的相对偏移量
         * 
         * @return the dx2
         */
        public short getDx2()
        {
            return clientAnchor.Dx2;
        }

        /**
         * 左上角的相对偏移量
         * 
         * @return the dy1
         */
        public short getDy1()
        {
            return clientAnchor.Dy1;
        }

        /**
         * 右下角的相对偏移量
         * 
         * @return the dy2
         */
        public short getDy2()
        {
            return clientAnchor.Dy2;
        }

        /**
         * 左上角所在的行
         * 
         * @return the row1
         */
        public short getRow1()
        {
            return clientAnchor.Row1;
        }

        /**
         * 右下角所在的行
         * 
         * @return the row2
         */
        public short getRow2()
        {
            return clientAnchor.Row2;
        }

    }
    public class WriteExcel
    {
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="savePath"></param>
        public static string CreatExcelByEntity(Type objType, string savePath)
        {
            string tableName = string.Empty;
            string name = string.Empty;
            List<string> listColumnName = new List<string>();
            //取属性上的自定义特性
            foreach (PropertyInfo propInfo in objType.GetProperties())
            {
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(LevcnColumnAttribute), true);
                if (objAttrs.Length > 0)
                {
                    LevcnColumnAttribute attr = objAttrs[0] as LevcnColumnAttribute;
                    if (attr != null)
                    {
                        if (attr.IsNeedImport)
                        {
                            listColumnName.Add(attr.DisplayName); //列名
                        }
                    }
                }
            }

            //取类上的自定义特性
            object[] objs = objType.GetCustomAttributes(typeof(LevcnTableAttribute), true);
            foreach (object obj in objs)
            {
                LevcnTableAttribute attr = obj as LevcnTableAttribute;
                if (attr != null)
                {
                    name = attr.Name;
                    tableName = attr.DisplayName + "(" + attr.Name + ")";//表名只有获取一次
                    break;
                }
            }
            //开始生成excel表格
            //创建工作薄
            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个名称为mySheet的表
            ISheet sh = wk.CreateSheet(tableName);
            HSSFCellStyle lo_Style = (HSSFCellStyle)wk.CreateCellStyle();
            lo_Style.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");

            //设置单元的宽度  
            //sh.SetColumnWidth(0, 15 * 256);
            //sh.SetColumnWidth(1, 35 * 256);
            //sh.SetColumnWidth(2, 15 * 256);
            //sh.SetColumnWidth(3, 10 * 256);
            int i = 0;
            #region 设置表头
            IRow row1 = sh.CreateRow(0);
            row1.Height = 20 * 20;
            ICell cell = null;

            foreach (string column in listColumnName)
            {
                cell = row1.CreateCell(i);
                cell.CellStyle = lo_Style;
                cell.SetCellValue(column);
                sh.SetDefaultColumnStyle(i, lo_Style);
                i++;
            }
            #endregion
            if (Directory.Exists(savePath) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(savePath);
            }
            savePath += @"\" + name + "Template.xls";
            using (FileStream stm = File.OpenWrite(savePath))
            {
                wk.Write(stm);
            }
            wk = null;
            return savePath;
        }
    }
}
