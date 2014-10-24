using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fw.Extends
{
    public static class IntExtend
    {


        /// <summary>
        /// 返回是否是默认(0否,1是)
        /// </summary>
        /// <param name="isdefault"></param>
        /// <param name="invert">是否反转</param>
        /// <returns></returns>
        public static int GetIsDefault(this int? isdefault, bool invert)
        {
            return (isdefault ?? 0).Equals(0) ? 1 : 0;
        }
        /// <summary>
        /// 返回是否是默认(0否,1是)
        /// </summary>
        /// <param name="isdefault"></param>
        /// <returns></returns>
        public static bool CheckIsDefault(this int? isdefault)
        {
            return !(isdefault ?? 0).Equals(0);
        }
        /// <summary>
        /// 1返回A,2返回B,直到Z
        /// </summary>
        /// <param name="thisInt"></param>
        /// <returns></returns>
        public static string GetABCDEFG(this int thisInt)
        {
            //能看到      地图,不会加       载错误,看不清      楚 A|B|C

            //能看到${0}$地图,不会加${0}$载错误,看不清${0}$楚
            //List<string>A,B,C
            if (thisInt <= 27)
            {
                return Enumerable.Range(0, 27).Select(w => ((char)('A' + w))).ToList()[thisInt - 1].ToString();
            }
            return "";
        }

        /// <summary>
        /// 返回指定数组的索引对象
        /// </summary>
        /// <param name="thisInt"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T GetIndex<T>(this int thisInt, List<T> items)
        {
            if (items != null && thisInt >= 0 && thisInt < items.Count)
            {
                return items[thisInt];
            }
            return default(T);
        }
        /// <summary>
        /// 返回一二三
        /// </summary>
        /// <param name="thisInt"></param>
        /// <returns></returns>
        public static string Get1234(this int thisInt)
        {
            if (thisInt > 0)
            {
                Money money = new Money(thisInt, false);
                return money.Convert();
            }
            return "";
        }
        public static string ToSexString(this int thisInt)
        {
            return thisInt == 1 ? "男" : "女";

        }

        /// <summary>
        /// 返回thisInt是否可以被param中任意一个数整除
        /// </summary>
        /// <param name="thisInt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsNoRemainderAny(this int thisInt, params int[] param)
        {
            var f = (float)thisInt;
            return param.ToList().Any(w => (f % w) == 0);
        }
    }

    public class Money
    {
        /// <summary>
        /// 要转换的数字
        /// </summary>
        private double j;
        /// <summary>
        /// 
        /// </summary>
        private string[] NumChineseCharacter = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾" };
        public Money(double m, bool upper)
        {
            if (!upper) NumChineseCharacter = new string[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            this.j = m;
        }
        /// <summary>
        /// 判断输入的数字是否大于double类型
        /// </summary>
        private bool IsNumber
        {
            get
            {
                if (j > double.MaxValue || j <= 0)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// 数字转换成大写汉字主函数
        /// </summary>
        /// <returns>返回转换后的大写汉字</returns>
        public string Convert()
        {
            string bb = "";
            if (IsNumber)
            {
                string str = j.ToString();
                string[] Num = str.Split('.');
                if (Num.Length == 1)
                {
                    bb = NumberString(Num[0]);// +"元整";
                    bb = bb.Replace(NumChineseCharacter[0] + NumChineseCharacter[0], NumChineseCharacter[0]);
                }
                else
                {
                    bb = NumberString(Num[0]);// +"元";
                    bb += FloatString(Num[1]);
                    bb = bb.Replace(NumChineseCharacter[0] + NumChineseCharacter[0], NumChineseCharacter[0]);
                }
            }
            else
            {
                throw new FormatException("你输入的数字格式不正确或不是数字!");
            }
            return bb;
        }
        /// <summary>
        /// 小数位转换只支持两位的小数
        /// </summary>
        /// <param name="Num">转换的小数</param>
        /// <returns>小数转换成汉字</returns>
        private string FloatString(string Num)
        {
            string cc = "";
            if (Num.Length > 2)
            {
                throw new FormatException("小数位数过多.");
            }
            else
            {
                string bb = ConvertString(Num);
                int len = bb.IndexOf(NumChineseCharacter[0]);
                if (len != 0)
                {
                    bb = bb.Replace(NumChineseCharacter[0], "");
                    if (bb.Length == 1)
                    {
                        cc = bb.Substring(0, 1);// +"角整";
                    }
                    else
                    {
                        cc = bb.Substring(0, 1);// +"角";
                        cc += bb.Substring(1, 1);// +"分";
                    }
                }
                else
                    cc = bb;// +"分";
            }
            return cc;
        }
        /// <summary>
        /// 判断数字位数以进行拆分转换
        /// </summary>
        /// <param name="Num">要进行拆分的数字</param>
        /// <returns>转换成的汉字</returns>
        private string NumberString(string Num)
        {
            string bb = "";
            if (Num.Length <= 4)
            {
                bb = Convert4(Num);
            }
            else if (Num.Length > 4 && Num.Length <= 8)
            {
                bb = Convert4(Num.Substring(0, Num.Length - 4)) + "万";
                bb += Convert4(Num.Substring(Num.Length - 4, 4));
            }
            else if (Num.Length > 8 && Num.Length <= 12)
            {
                bb = Convert4(Num.Substring(0, Num.Length - 8)) + "亿";
                if (Convert4(Num.Substring(Num.Length - 8, 4)) == "")
                    if (Convert4(Num.Substring(Num.Length - 4, 4)) != "")
                        bb += NumChineseCharacter[0];
                    else
                        bb += "";
                else
                    bb += Convert4(Num.Substring(Num.Length - 8, 4)) + "万";
                bb += Convert4(Num.Substring(Num.Length - 4, 4));
            }
            return bb;
        }
        /// <summary>
        /// 四位数字的转换
        /// </summary>
        /// <param name="Num">准备转换的四位数字</param>
        /// <returns>转换以后的汉字</returns>
        private string Convert4(string Num)
        {
            string bb = "";
            if (Num.Length == 1)
            {
                bb = ConvertString(Num);
            }
            else if (Num.Length == 2)
            {
                bb = ConvertString(Num);
                bb = Convert2(bb);
            }
            else if (Num.Length == 3)
            {
                bb = ConvertString(Num);
                bb = Convert3(bb);
            }
            else
            {
                bb = ConvertString(Num);
                string cc = "";
                string len = bb.Substring(0, 4);
                if (len != NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0])
                {
                    len = bb.Substring(0, 3);
                    if (len != NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0])
                    {
                        bb = bb.Replace(NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0], "");
                        if (bb.Length == 1)
                        {
                            bb = bb.Substring(0, 1) + "仟";
                        }
                        else
                        {
                            if (bb.Substring(0, 1) != NumChineseCharacter[0] && bb.Substring(0, 2) != NumChineseCharacter[0])
                                cc = bb.Substring(0, 1) + "仟";
                            else
                                cc = bb.Substring(0, 1);
                            bb = cc + Convert3(bb.Substring(1, 3));
                        }
                    }
                    else
                    {
                        bb = bb.Replace(NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0], NumChineseCharacter[0]);
                    }
                }
                else
                {
                    bb = bb.Replace(NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0] + NumChineseCharacter[0], "");
                }
            }
            return bb;
        }
        /// <summary>
        /// 将数字转换成汉字
        /// </summary>
        /// <param name="Num">需要转换的数字</param>
        /// <returns>转换后的汉字</returns>
        private string ConvertString(string Num)
        {
            string bb = "";
            for (int i = 0; i < Num.Length; i++)
            {
                bb += NumChineseCharacter[int.Parse(Num.Substring(i, 1))];
            }
            return bb;
        }
        /// <summary>
        /// 两位数字的转换
        /// </summary>
        /// <param name="Num">两位数字</param>
        /// <returns>转换后的汉字</returns>
        private string Convert2(string Num)
        {
            string bb = ""; string cc = "";
            string len = Num.Substring(0, 1);
            if (len != NumChineseCharacter[0])
            {
                bb = Num.Replace(NumChineseCharacter[0], "");
                if (bb.Length == 1)
                {
                    cc = bb.Substring(0, 1) + NumChineseCharacter[10];
                }
                else
                {
                    cc = bb.Substring(0, 1) + NumChineseCharacter[10];
                    cc += bb.Substring(1, 1);
                }
            }
            else
                cc = Num;
            return cc;
        }
        /// <summary>
        /// 三位数字的转换
        /// </summary>
        /// <param name="Num">三位数字</param>
        /// <returns>转换后的汉字</returns>
        private string Convert3(string Num)
        {
            string bb = ""; string cc = "";
            string len = Num.Substring(0, 2);
            if (len != NumChineseCharacter[0] + NumChineseCharacter[0])
            {
                bb = Num.Replace(NumChineseCharacter[0] + NumChineseCharacter[0], "");
                if (bb.Length == 1)
                {
                    bb = bb.Substring(0, 1) + "佰";
                }
                else
                {
                    if (bb.Substring(0, 1) != NumChineseCharacter[0])
                        cc = bb.Substring(0, 1) + "佰";
                    else
                        cc = bb.Substring(0, 1);
                    bb = cc + Convert2(bb.Substring(1, 2));
                }
            }
            else
            {
                bb = Num.Replace(NumChineseCharacter[0] + NumChineseCharacter[0], NumChineseCharacter[0]);
            }
            return bb;
        }
    }
    public static class TypeExtend
    {
        public static bool IsNumber(this Type content)
        {
            List<Type> list = new List<Type>
                                  {
                                          typeof(int),typeof(int?),
                                          typeof(short),typeof(short?),
                                          typeof(byte),typeof(byte?),
                                          typeof(long),typeof(long?),
                                          typeof(double),typeof(double?),
                                          typeof(float),typeof(float?),
                                  };
            return list.Contains(content);
        }
    }
}
