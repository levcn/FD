using System;
using System.Collections.Generic;


namespace FD.Core.Test.Entity
{
    public class SYS_User 
    {
        public SYS_User()
        {
            ID = Guid.NewGuid();
            _userDetail = "{\"ID\":\"47cb9aac-7fbf-4c56-9c0a-398c14a57bf8\",\"ConfigItems\":[{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"Name\",\"GroupIndex\":0,\"DisplayName\":\"姓名\",\"ColumnSize\":\"20\",\"UID\":\".0.Name\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"Age\",\"GroupIndex\":0,\"DisplayName\":\"年龄\",\"GroupCode\":\"\",\"ColumnSize\":\"20\",\"UID\":\".0.Age\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"CourseName\",\"GroupIndex\":0,\"DisplayName\":\"课程名称\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"20\",\"UID\":\"Group1.0.CourseName\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"CourseCode\",\"GroupIndex\":0,\"DisplayName\":\"课程代码\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"20\",\"UID\":\"Group1.0.CourseCode\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"PaperName\",\"GroupIndex\":0,\"DisplayName\":\"试卷名称\",\"GroupCode\":\"Group2\",\"ColumnSize\":\"20\",\"UID\":\"Group2.0.PaperName\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"PaperCode\",\"GroupIndex\":0,\"DisplayName\":\"试卷代码\",\"GroupCode\":\"Group2\",\"ColumnSize\":\"20\",\"UID\":\"Group2.0.PaperCode\",\"ItemType\":\"字符串\"}]}";

            _introduction = "{\"ID\":\"0229505e-72b4-41d9-ab27-2a418a9043d6\",\"ConfigItems\":[{\"ControlType\":\"字符串\",\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"YingPinJiGou\",\"GroupIndex\":0,\"DisplayName\":\"应聘机构\",\"ColumnSize\":\"50\",\"UID\":\".0.YingPinJiGou\",\"ItemType\":\"字符串\"},{\"ControlType\":\"字符串\",\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"YingPinZhiWei\",\"GroupIndex\":0,\"DisplayName\":\"应聘职位\",\"ColumnSize\":\"50\",\"UID\":\".0.YingPinZhiWei\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"GongZuoDiDian\",\"GroupIndex\":0,\"DisplayName\":\"工作地点\",\"ColumnSize\":\"50\",\"UID\":\".0.GongZuoDiDian\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ID\",\"GroupIndex\":0,\"DisplayName\":\"ID\",\"ColumnSize\":\"50\",\"UID\":\".0.ID\",\"DefaultValue\":\"JR125305094R902500050000\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"XingMing\",\"GroupIndex\":0,\"DisplayName\":\"姓名\",\"ColumnSize\":\"50\",\"UID\":\".0.XingMing\",\"DefaultValue\":\"张三\",\"ItemType\":\"字符串\"},{\"ControlConfig\":\"{\\\"Items\\\":[{\\\"DisplayName\\\":\\\"男\\\",\\\"Value\\\":\\\"男\\\"},{\\\"DisplayName\\\":\\\"女\\\",\\\"Value\\\":\\\"女\\\"}]}\",\"ControlType\":\"下拉框\",\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"XingBie\",\"GroupIndex\":0,\"DisplayName\":\"性别\",\"ColumnSize\":\"50\",\"UID\":\".0.XingBie\",\"DefaultValue\":\"男\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"NianLing\",\"GroupIndex\":0,\"DisplayName\":\"年龄\",\"ColumnSize\":\"50\",\"UID\":\".0.NianLing\",\"DefaultValue\":\"22\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ChuSheng\",\"GroupIndex\":0,\"DisplayName\":\"出生日期\",\"ColumnSize\":\"50\",\"UID\":\".0.ChuSheng\",\"DefaultValue\":\"1987.9\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"GongZuoNian\",\"GroupIndex\":0,\"DisplayName\":\"工作经验\",\"ColumnSize\":\"50\",\"UID\":\".0.GongZuoNian\",\"DefaultValue\":\"3\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"XieLi\",\"GroupIndex\":0,\"DisplayName\":\"学历\",\"ColumnSize\":\"50\",\"UID\":\".0.XieLi\",\"DefaultValue\":\"本科\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"HunYin\",\"GroupIndex\":0,\"DisplayName\":\"婚姻\",\"ColumnSize\":\"50\",\"UID\":\".0.HunYin\",\"DefaultValue\":\"已婚\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"JiZhuDi\",\"GroupIndex\":0,\"DisplayName\":\"居住地\",\"ColumnSize\":\"50\",\"UID\":\".0.JiZhuDi\",\"DefaultValue\":\"北京\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"HuJi\",\"GroupIndex\":0,\"DisplayName\":\"户籍\",\"ColumnSize\":\"50\",\"UID\":\".0.HuJi\",\"DefaultValue\":\"北京\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ShenFenZheng\",\"GroupIndex\":0,\"DisplayName\":\"身份证\",\"ColumnSize\":\"50\",\"UID\":\".0.ShenFenZheng\",\"DefaultValue\":\"588899966665554815\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"YouBian\",\"GroupIndex\":0,\"DisplayName\":\"邮编\",\"ColumnSize\":\"50\",\"UID\":\".0.YouBian\",\"DefaultValue\":\"100000\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ShouJi\",\"GroupIndex\":0,\"DisplayName\":\"手机\",\"ColumnSize\":\"50\",\"UID\":\".0.ShouJi\",\"DefaultValue\":\"14899877789\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"DianHua\",\"GroupIndex\":0,\"DisplayName\":\"电话\",\"ColumnSize\":\"50\",\"UID\":\".0.DianHua\",\"DefaultValue\":\"14899877789\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"YouXiang\",\"GroupIndex\":0,\"DisplayName\":\"邮箱\",\"ColumnSize\":\"50\",\"UID\":\".0.YouXiang\",\"DefaultValue\":\"abc@163.com\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"QGongZuoDi\",\"GroupIndex\":0,\"DisplayName\":\"期望工作地区\",\"ColumnSize\":\"50\",\"UID\":\".0.QGongZuoDi\",\"DefaultValue\":\"北京\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"QYueXin\",\"GroupIndex\":0,\"DisplayName\":\"期望月薪\",\"ColumnSize\":\"50\",\"UID\":\".0.QYueXin\",\"DefaultValue\":\"4000\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"MuXianZhuangKuang\",\"GroupIndex\":0,\"DisplayName\":\"目前状态\",\"ColumnSize\":\"50\",\"UID\":\".0.MuXianZhuangKuang\",\"DefaultValue\":\"离职\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"QGongZuoXingZhi\",\"GroupIndex\":0,\"DisplayName\":\"期望工作性质\",\"ColumnSize\":\"50\",\"UID\":\".0.QGongZuoXingZhi\",\"DefaultValue\":\"全职\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"QZhiYie\",\"GroupIndex\":0,\"DisplayName\":\"期望从事职业\",\"ColumnSize\":\"50\",\"UID\":\".0.QZhiYie\",\"DefaultValue\":\"计算机/互联网\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"QHangYe\",\"GroupIndex\":0,\"DisplayName\":\"期望从事行业\",\"ColumnSize\":\"50\",\"UID\":\".0.QHangYe\",\"DefaultValue\":\"计算机/互联网\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ZiWoPingJia\",\"GroupIndex\":0,\"DisplayName\":\"自我评价\",\"ColumnSize\":\"50\",\"UID\":\".0.ZiWoPingJia\",\"DefaultValue\":\"评价评价评价评价评价评价评价评价评价评价评价评价评价评价评价评价评价评价\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"KaiShiShiJian\",\"GroupIndex\":0,\"DisplayName\":\"开始时间\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.KaiShiShiJian\",\"DefaultValue\":\"2012.5\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"JieZhuShiJian\",\"GroupIndex\":0,\"DisplayName\":\"结束时间\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.JieZhuShiJian\",\"DefaultValue\":\"2013.5\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"GongSiMingCheng\",\"GroupIndex\":0,\"DisplayName\":\"公司名称\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.GongSiMingCheng\",\"DefaultValue\":\"xxxxxxxxxxxxxxx公司\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"BuMen\",\"GroupIndex\":0,\"DisplayName\":\"部门\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.BuMen\",\"DefaultValue\":\"项目研发部\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ZhiWei\",\"GroupIndex\":0,\"DisplayName\":\"职位\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.ZhiWei\",\"DefaultValue\":\"副项目经理\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"GongSiXingZhi\",\"GroupIndex\":0,\"DisplayName\":\"企业性质\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.GongSiXingZhi\",\"DefaultValue\":\"股份制企业\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"GuiMo\",\"GroupIndex\":0,\"DisplayName\":\"规模\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.GuiMo\",\"DefaultValue\":\"100-499人\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"GongZuoMiaoShu\",\"GroupIndex\":0,\"DisplayName\":\"工作描述\",\"GroupCode\":\"Group1\",\"ColumnSize\":\"50\",\"UID\":\"Group1.0.GongZuoMiaoShu\",\"DefaultValue\":\"工作描述：\\\" VerticalAlignment=\\\"Top\\\" Margin=\\\"0 5 0 0\\\"></TextBlock>\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ShouJiHao\",\"GroupIndex\":0,\"DisplayName\":\"手机号\",\"GroupCode\":\"\",\"ColumnSize\":\"50\",\"UID\":\".0.ShouJiHao\",\"DefaultValue\":\"14899877789\",\"ItemType\":\"字符串\"},{\"ID\":\"00000000-0000-0000-0000-000000000000\",\"Code\":\"ZhaoPian\",\"GroupIndex\":0,\"DisplayName\":\"照片\",\"ColumnSize\":\"50\",\"UID\":\".0.ZhaoPian\",\"ItemType\":\"字符串\"},{\"ID\":\"066a2367-6c04-40d3-8351-476e75c71ed9\",\"Code\":\"ShouJi1\",\"GroupIndex\":0,\"DisplayName\":\"手机1\",\"ColumnSize\":\"50\",\"UID\":\".0.ShouJi1\",\"DefaultValue\":\"14899877789\",\"ItemType\":\"字符串\"}]}";
        }

        Guid _iD;
        public Guid ID
        {
            get
            {
                return _iD;
            }
            set
            {
                if (value != _iD)
                {
                    _iD = value;
                }
            }
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                }
            }
        }

        Guid? _userType;
        public Guid? UserType
        {
            get
            {
                return _userType;
            }
            set
            {
                if (value != _userType)
                {
                    _userType = value;
                }
            }
        }

        List<SYS_UserRoleInfo> _roles;
        public List<SYS_UserRoleInfo> Roles
        {
            get
            {
                return _roles;
            }
            set
            {
                if (value != _roles)
                {
                    _roles = value;
                }
            }
        }

        List<Dict_Department> _departments;
        public List<Dict_Department> Departments
        {
            get
            {
                return _departments;
            }
            set
            {
                if (value != _departments)
                {
                    _departments = value;
                }
            }
        }

        int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value != _age)
                {
                    _age = value;
                }
            }
        }

        int _test;
        public int test
        {
            get
            {
                return _test;
            }
            set
            {
                if (value != _test)
                {
                    _test = value;
                }
            }
        }

        string _account;
        public string Account
        {
            get
            {
                return _account;
            }
            set
            {
                if (value != _account)
                {
                    _account = value;
                }
            }
        }

        string _passwords;
        public string Passwords
        {
            get
            {
                return _passwords;
            }
            set
            {
                if (value != _passwords)
                {
                    _passwords = value;
                }
            }
        }

        string _userDetail;
        public string UserDetail
        {
            get
            {
                return _userDetail;
            }
            set
            {
                if (value != _userDetail)
                {
                    _userDetail = value;
                }
            }
        }

        Dict_UserType _dict_UserType;
        public Dict_UserType Dict_UserType
        {
            get
            {
                return _dict_UserType;
            }
            set
            {
                if (value != _dict_UserType)
                {
                    _dict_UserType = value;
                }
            }
        }

        SYS_UserDetail _sYS_UserDetail;
        public SYS_UserDetail SYS_UserDetail
        {
            get
            {
                return _sYS_UserDetail;
            }
            set
            {
                if (value != _sYS_UserDetail)
                {
                    _sYS_UserDetail = value;
                }
            }
        }
        string _introduction;
        public string Introduction
        {
            get
            {
                return _introduction;
            }
            set
            {
                if (value != _introduction)
                {
                    _introduction = value;
                }
            }
        }
    }
}
