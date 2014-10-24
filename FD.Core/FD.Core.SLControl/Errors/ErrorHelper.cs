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
using SLControls.DataClientTools;


namespace SLControls.Errors
{
    public class ErrorHelper
    {
        public async Task<bool> ReportError(Exception e)
        {
            return await ActionHelper.CustomRequest<bool>("PageAction", "ReportError", new object[] { e.ToError() });
        }
    }

    public class Error
    {
        public string Message { get; set; }
        public string Stack { get; set; }
    }

    public static class ExceptionEx
    {
        public static Error ToError(this Exception e)
        {
            return new Error {
                Message = e.Message,
                Stack = e.StackTrace,
            };
        }
    }
}
