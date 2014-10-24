using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Fw.ActionMethod;


namespace Fw.AD
{
    /// <summary>
    /// 活动目录操作类
    /// </summary>
    public class ADHelper : BaseController
    {
        /// <summary>
        /// 域登录的通用方法//ADSearch("abc.com", "administrator", "123qwe/", "user", "tht");
        /// </summary>
        /// <param name="domainADsPath">域名地址</param>
        /// <param name="username">登录域的账户</param>
        /// <param name="password">登录域的密码</param>
        /// <param name="schemaClassNameToSearch">查询内容</param>
        /// <param name="loginname">查询哪个账户的相关信息</param>
        /// <returns></returns>
        public static ADUserInfo ADSearch(string domainADsPath, string username, string password, string schemaClassNameToSearch, string loginname)
        {
            ADUserInfo ad = new ADUserInfo();
            DirectoryEntry de1 = new DirectoryEntry("LDAP://" + domainADsPath, username, password);
            DirectorySearcher searcher = new DirectorySearcher(de1);
            //            searcher.SearchRoot = new DirectoryEntry("LDAP://" + domainADsPath, username, password);
            string filter = "(objectClass=" + schemaClassNameToSearch + ")";
            if (schemaClassNameToSearch == "user")
            {
                filter = "(&" + filter + "(sAMAccountName=" + loginname + "))";
            }
            searcher.Filter = filter;
            //searcher.SearchScope = SearchScope.Subtree;
            //searcher.Sort = new SortOption("sAMAccountName", System.DirectoryServices.SortDirection.Ascending);
            // If there is a large set to be return ser page size for a paged search
            //searcher.PageSize = 512;
            //searcher.PropertiesToLoad.AddRange(new string[] { "sAMAccountName", "displayname", "company", "department", "title" });
            //File.WriteAllText(@"C:\aaa\aaaaa.txt", loginname + "------------------------");
            try
            {
                SearchResultCollection results = searcher.FindAll();
                if (results.Count > 0)
                {
                    foreach (SearchResult resEnt in results)
                    {
                        DirectoryEntry de2 = resEnt.GetDirectoryEntry();
                        string sname = de2.Properties["DisplayName"].Value.ToString();
                        string[] spname = sname.Split('.');
                        if (spname.Length > 0)
                        {
                            ad.Displayname = spname[0];
                        }
                        //                    ad.displayname = (GetString(de2, "DisplayName")); //真实姓名
                        ad.Company = (GetString(de2, "company")); //公司
                        ad.Department = (GetString(de2, "department")); //部门
                        ad.Title = (GetString(de2, "title")); //职务
                        if (!string.IsNullOrEmpty(ad.Displayname)) break;
                    }
                }
                else
                {
                    ad = null;
                }
            }
            catch (Exception exception)
            {
                //File.WriteAllText(@"C:\aaa\aaaaa.txt", exception.Message + " ----------------" + exception.StackTrace);
            }

            return ad;
        }
        public class ADUserInfo
        {
            /// <summary>
            /// 真实姓名
            /// </summary>
            public string Displayname { get; set; }

            /// <summary>
            /// 职务
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 部门
            /// </summary>
            public string Department { get; set; }

            /// <summary>
            /// 公司
            /// </summary>
            public string Company { get; set; }

            /// <summary>
            /// 域账号
            /// </summary>
            public string AdAccount { get; set; }
        }
        public class MyClass
        {
            public string name { get; set; }
            public string value { get; set; }
            public override string ToString()
            {
                return string.Format("name:{0}          value:{1}", name, value);
            }
        }
        public void ini(int z)
        {
            var dfer = HttpContext.Current;
            //return 1;
        }
        private static string GetString(DirectoryEntry de2, string name)
        {
            //            List<MyClass> m = new List<MyClass>();
            //            foreach (PropertyValueCollection entry in de2.Properties)
            //            {
            //                //                entry
            //                //                if (entry.Key != null)
            //                //                {
            //                //                    var v = "";
            //                //                    if (entry.Value != null) v = entry.Value.ToString();
            //                //                    m.Add(new MyClass { name = entry.Key.ToString(), value = v});
            //                //                }
            //                m.Add(new MyClass { name = entry.PropertyName, value = entry.Value.ToString() });
            //            }
            return de2.Properties[name] != null && de2.Properties[name].Value != null ? de2.Properties[name].Value.ToString() : "";
        }
        /// <summary>
        /// 返回域中的用户名 
        /// </summary>
        /// <param name="sname"></param>
        /// <returns></returns>
        public static string GetUserName(string sname)
        {
            var spname = sname.Split('\\');
            if (spname.Length > 0)
            {
                return spname.Length == 2 ? spname[1] : spname[0];
            }
            return "";
        }

        #region 连接到域测试 并提取用户信息

        /// <summary>
        /// 功能：是否连接到域
        /// </summary>
        /// <param name="domainName">域名或IP</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>        
        /// <param name="domain">域</param>
        /// <returns></returns>
        public static bool IsConnected(string domainName, string userName, string userPwd, out DirectoryEntry domain)
        {
            domain = new DirectoryEntry();
            try
            {
                domain.Path = string.Format("LDAP://{0}", domainName);
                domain.Username = userName;
                domain.Password = userPwd;
                domain.AuthenticationType = AuthenticationTypes.Secure;

                domain.RefreshCache();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static ADUserInfo GetAdUserInfo(DirectoryEntry domain, String pSLoginName)
        {
            //MessageBox.Show(pSLoginName);
            var adUserInfo = new ADUserInfo();
            var search = new DirectorySearcher(domain) { Filter = string.Format("(SAMAccountName={0})", pSLoginName) };

            try
            {
                var result = search.FindOne();
                if (result == null)
                {
                    search = new DirectorySearcher(domain) { Filter = string.Format("(userPrincipalName={0})", pSLoginName) };

                    result = search.FindOne();
                }

                if (result != null)
                {
                    BindAdUserInfo(result, adUserInfo);
                }
                else
                {
                    adUserInfo = null;
                }
            }
            catch (Exception ex)
            {
                adUserInfo = null;
            }
            return adUserInfo;
        }       

        public static void BindAdUserInfo(SearchResult result, ADUserInfo adUserInfo)
        {
            var de = result.GetDirectoryEntry();

            var depart = GetProperty(de, "department").Split('/');
            var displayName = GetProperty(de, "displayName");

            adUserInfo.Company = GetProperty(de, "company");
            adUserInfo.Department = depart.Length > 0 ? depart[0] : "未找到部门信息";
            adUserInfo.Displayname = displayName.Split('.').Length > 0 ? displayName.Split('.')[0] : displayName;
            adUserInfo.Title = GetProperty(de, "title");
            adUserInfo.AdAccount = GetProperty(de, "SAMAccountName");
        }


        ///
        ///获得指定搜索结果 中指定属性名对应的值
        ///
        ///
        ///属性名称
        ///属性值
        public static string GetProperty(SearchResult searchResult, string propertyName)
        {
            return searchResult.Properties.Contains(propertyName) ? searchResult.Properties[propertyName][0].ToString() : string.Empty;
        }

        ///
        ///获得指定 指定属性名对应的值
        ///
        ///
        ///属性名称
        ///属性值
        public static string GetProperty(DirectoryEntry de, string propertyName)
        {
            return de.Properties.Contains(propertyName) && de.Properties[propertyName].Count > 0 ? de.Properties[propertyName][0].ToString() : string.Empty;
        }

        #endregion

    }

    public static class DomainInformation
    {
        private static DirectoryEntry Directory()
        {
            return new DirectoryEntry(usersLdapPath, adLoginName, adLoginPassword);
        }

        private static DirectoryEntry Directoryunits(string domainADsPath)
        {
            return new DirectoryEntry();
        }
        #region Constants

        //static string[] usersLdapPath = @"LDAP://zzzzzz.com/OU=xxxxxx,DC=yyyyyy,DC=com";
        private static string usersLdapPath =
                System.Configuration.ConfigurationManager.AppSettings["LDAPConnectionString"].ToString();

        private const string adLoginName = "administrator"; //管理员用户
        private const string adLoginPassword = "88888888";

        #endregion

        public static string[] GetGroupsForUser(string domainADsPath, string username) // 获取用户所属组
        {

            DirectoryEntry usersDE = Directoryunits(domainADsPath);
            DirectorySearcher ds = new DirectorySearcher(usersDE);
            ds.Filter = "(&(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("memberof");
            SearchResult r = ds.FindOne();

            if (r.Properties["memberof"].Count == 0)
            {
                return (null);
            }

            string[] results = new string[r.Properties["memberof"].Count];
            for (int i = 0; i < r.Properties["memberof"].Count; i++)
            {
                string theGroupPath = r.Properties["memberof"][i].ToString();
                results[i] = theGroupPath.Substring(3, theGroupPath.IndexOf(",") - 3);
            }
            usersDE.Close();
            return (results);
        }


        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string[] GetGroupsForUser(string username)
        {
            DirectoryEntry usersDE = DomainInformation.Directory();
            DirectorySearcher ds = new DirectorySearcher(usersDE);
            ds.Filter = "(&(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("memberof");
            SearchResult r = ds.FindOne();
            if (r.Properties["memberof"] == null)
            {
                return (null);
            }
            string[] results = new string[r.Properties["memberof"].Count + 1];
            for (int i = 0; i < r.Properties["memberof"].Count; i++)
            {
                string theGroupPath = r.Properties["memberof"][i].ToString();
                results[i] = theGroupPath.Substring(3, theGroupPath.IndexOf(",") - 3);
            }
            results[r.Properties["memberof"].Count] = "All"; //All组属于任何人,在AD之外定义了一个组，以便分配用户权限
            usersDE.Close();
            return (results);
        }

        public static string[] GetUsersForGroup(string domainADsPath, string Groupname) // 获取用户
        {

            DirectoryEntry usersDE = Directoryunits(domainADsPath);
            DirectorySearcher ds = new DirectorySearcher(usersDE);
            ds.Filter = "(&(objectClass=group)(cn=" + Groupname + "))";
            ds.PropertiesToLoad.Add("member");
            SearchResult r = ds.FindOne();

            if (r.Properties["member"] == null)
            {
                return (null);
            }

            string[] results = new string[r.Properties["member"].Count];
            for (int i = 0; i < r.Properties["member"].Count; i++)
            {
                string theGroupPath = r.Properties["member"][i].ToString();
                results[i] = theGroupPath.Substring(3, theGroupPath.IndexOf(",") - 3);
            }
            usersDE.Close();
            return (results);
        }


        public static string GetUserDisplayName(string username) // 获取组用户
        {
            string results;
            DirectoryEntry usersDE = Directory();

            DirectorySearcher ds = new DirectorySearcher(usersDE);
            ds.Filter = "(&(objectClass=user)(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add(UserProperty.DisplayName);
            SearchResult r = ds.FindOne();
            results = r.GetDirectoryEntry().InvokeGet(UserProperty.DisplayName).ToString();
            usersDE.Close();
            return (results);

        }
        public static void GetUserInfo(string username)
        {
            DirectoryEntry usersDE = Directory();
            DirectorySearcher ds = new DirectorySearcher(usersDE);
            ds.Filter = "(&(objectClass=user)(objectCatogery=person)(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("cn");
            ds.PropertiesToLoad.Add("Sn");
            SearchResult r = ds.FindOne();

            UserInfoEx result = new UserInfoEx();

            result.Name = r.GetDirectoryEntry().InvokeGet("Sn").ToString();
            result.LoginName = r.GetDirectoryEntry().InvokeGet(UserProperty.UserName).ToString();
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.FirstName) != null)
            {
                result.FirstName = r.GetDirectoryEntry().InvokeGet(UserProperty.FirstName).ToString();
            }
            else
            {
                result.FirstName = "";
            }
        }
        public static UserInfoEx GetUserInfoEx(string username) //获取域用户详细信息
        {
            DirectoryEntry usersDE = Directory();
            DirectorySearcher ds = new DirectorySearcher(usersDE);
            ds.Filter = "(&(objectClass=user)(objectCatogery=person)(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("cn");
            ds.PropertiesToLoad.Add(UserProperty.Name);
            ds.PropertiesToLoad.Add(UserProperty.UserName);
            ds.PropertiesToLoad.Add(UserProperty.homePhone);
            ds.PropertiesToLoad.Add(UserProperty.FirstName);
            ds.PropertiesToLoad.Add(UserProperty.LastName);
            ds.PropertiesToLoad.Add(UserProperty.Email);
            ds.PropertiesToLoad.Add(UserProperty.Title);
            ds.PropertiesToLoad.Add(UserProperty.Company);
            ds.PropertiesToLoad.Add(UserProperty.Address);
            ds.PropertiesToLoad.Add(UserProperty.City);
            ds.PropertiesToLoad.Add(UserProperty.State);
            ds.PropertiesToLoad.Add(UserProperty.PostalCode);
            ds.PropertiesToLoad.Add(UserProperty.Phone);
            ds.PropertiesToLoad.Add(UserProperty.Country);
            SearchResult r = ds.FindOne();

            UserInfoEx result = new UserInfoEx();

            result.Name = r.GetDirectoryEntry().InvokeGet(UserProperty.Name).ToString();
            result.LoginName = r.GetDirectoryEntry().InvokeGet(UserProperty.UserName).ToString();
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.FirstName) != null)
            {
                result.FirstName = r.GetDirectoryEntry().InvokeGet(UserProperty.FirstName).ToString();
            }
            else
            {
                result.FirstName = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.homePhone) != null)
            {
                result.homePhone = r.GetDirectoryEntry().InvokeGet(UserProperty.homePhone).ToString();
            }
            else
            {
                result.homePhone = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.LastName) != null)
            {
                result.LastName = r.GetDirectoryEntry().InvokeGet(UserProperty.LastName).ToString();
            }
            else
            {
                result.LastName = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.Email) != null)
            {
                result.EmailAddress = r.GetDirectoryEntry().InvokeGet(UserProperty.Email).ToString();
            }
            else
            {
                result.EmailAddress = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.Title) != null)
            {
                result.Title = r.GetDirectoryEntry().InvokeGet(UserProperty.Title).ToString();
            }
            else
            {
                result.Title = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.Company) != null)
            {
                result.Company = r.GetDirectoryEntry().InvokeGet(UserProperty.Company).ToString();
            }
            else
            {
                result.Company = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.Address) != null)
            {
                result.Address = r.GetDirectoryEntry().InvokeGet(UserProperty.Address).ToString();
            }
            else
            {
                result.Address = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.City) != null)
            {
                result.City = r.GetDirectoryEntry().InvokeGet(UserProperty.City).ToString();
            }
            else
            {
                result.City = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.State) != null)
            {
                result.State = r.GetDirectoryEntry().InvokeGet(UserProperty.State).ToString();
            }
            else
            {
                result.State = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.PostalCode) != null)
            {
                result.PostalCode = r.GetDirectoryEntry().InvokeGet(UserProperty.PostalCode).ToString();
            }
            else
            {
                result.PostalCode = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.Phone) != null)
            {
                result.Phone = r.GetDirectoryEntry().InvokeGet(UserProperty.Phone).ToString();
            }
            else
            {
                result.Phone = "";
            }
            if (r.GetDirectoryEntry().InvokeGet(UserProperty.Country) != null)
            {
                result.Country = r.GetDirectoryEntry().InvokeGet(UserProperty.Country).ToString();
            }
            else
            {
                result.Country = "";
            }
            usersDE.Close();
            return (result);
        }

        private static string GetAdGroupDescription(string prefix) //根据CN获取组description
        {
            string results;

            DirectoryEntry groupsDE = Directory();
            DirectorySearcher groupsDS = new DirectorySearcher(groupsDE);
            groupsDS.Filter = "(&(objectClass=group)(CN=" + prefix + "*))";
            groupsDS.PropertiesToLoad.Add("cn");
            SearchResult sr = groupsDS.FindOne();
            results = sr.GetDirectoryEntry().InvokeGet("description").ToString();
            groupsDE.Close();
            return (results);
        }

        private static DataTable GetAdGroupInfo() //根据CN获取组信息 
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("URL", typeof(System.String));
            dt.Columns.Add("cn", typeof(System.String));
            dt.Columns.Add("Description", typeof(System.String));

            DirectoryEntry groupsDE = Directory();
            DirectorySearcher searcher = new DirectorySearcher(groupsDE);

            searcher.Filter = "(&(objectClass=group))";
            //searcher.SearchScope = SearchScope.Subtree;
            //searcher.Sort = new SortOption("description", System.DirectoryServices.SortDirection.Ascending);
            searcher.PropertiesToLoad.AddRange(new string[] { "cn", "description" });
            SearchResultCollection results = searcher.FindAll();
            if (results.Count == 0)
            {
                return (null);

            }
            else
            {
                foreach (SearchResult result in results)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.Path.ToString();
                    dr[1] = result.GetDirectoryEntry().InvokeGet("cn").ToString();
                    if (result.GetDirectoryEntry().InvokeGet("Description") != null)
                        dr[2] = result.GetDirectoryEntry().InvokeGet("Description").ToString();
                    else
                        dr[2] = result.GetDirectoryEntry().InvokeGet("cn").ToString();
                    dt.Rows.Add(dr);
                }
                dt.DefaultView.Sort = "description ASC";
                groupsDE.Close();
                return dt;

            }

        }



        public static string getAccountName(string cn) //根据CN获取登陆名
        {
            //            foreach (string path in usersLdapPath)
            {
                DirectoryEntry userContainerDE = Directoryunits(usersLdapPath);
                DirectorySearcher ds = new DirectorySearcher(userContainerDE);
                ds.Filter = "(&(objectClass=user)(cn=*" + cn + "*))";
                ds.PropertiesToLoad.Add("sAMAccountName");
                SearchResult r = ds.FindOne();
                if (r != null)
                    return r.GetDirectoryEntry().InvokeGet("sAMAccountName").ToString();
            }
            return null;
        }

        public static bool isAdUser(string username) //判断是否域用户
        {

            DirectoryEntry userContainerDE = Directory();
            DirectorySearcher ds = new DirectorySearcher(userContainerDE);
            ds.Filter = "(&(objectClass=user)(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("cn");
            SearchResult r = ds.FindOne();
            if (r == null)
            {
                userContainerDE.Close();
                return false;

            }
            else
            {
                userContainerDE.Close();
                return true;
            }

        }
    }

    public class UserProperty
    {
        //姓 Sn
        //名 Givename
        //英文缩写 Initials
        //显示名称 displayName
        //描述 Description
        //办公室 physicalDeliveryOfficeName 
        //电话号码 telephoneNumber
        //电话号码：其它 otherTelephone 多个以英文分号分隔
        //电子邮件 Mail
        //网页 wWWHomePage
        public static string DisplayName = "displayname";
        public static string Name;
        public static string UserName;
        public static string homePhone;
        public static string FirstName;
        public static string LastName;
        public static string Email;
        public static string Title;
        public static string Company;
        public static string Address;
        public static string City;
        public static string State;
        public static string PostalCode;
        public static string Phone;
        public static string Country;
    }
    public class UserInfoEx
    {
        public string Name { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string homePhone { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Title { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }
    }
}
