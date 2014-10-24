using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLControls.Controls;
using SLControls.Editors;
using SLControls.Extends;
using StaffTrain.FwClass.Reflectors;


namespace SLControls.ActiveMethod
{
    public class ActiveHelper
    {
        public static Type ActionType;
        /// <summary>
        /// 
        /// </summary>
        public static async Task<object> Execute(BaseMultiControl control,string code)
        {
            //CheckPassword(MainPage)
            control.GetType().GetProperty("UserName");
            control.GetType().GetProperty("Password");
            var userName = ReflectionHelper.GetPropertyValue("UserName", control) as string;
            var passwords = ReflectionHelper.GetPropertyValue("Password", control) as string;
            var method = ActionType.GetMethod("CheckPassword");
            if (method.IsAsynMethod())
            {
                var t = method.Invoke(null, new[] { userName, passwords, "SilverlightControl1" }) as Task;
                await t;
                var re = t.GetType().GetProperty("Result").GetValue(t, null);
                return re;
            }
            else
            {
                var re = method.Invoke(null, new[] { userName, passwords, "SilverlightControl1" });
                return re;
            }
        }
    }
}
