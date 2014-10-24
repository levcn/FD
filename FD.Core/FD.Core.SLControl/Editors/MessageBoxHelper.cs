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
using Telerik.Windows.Controls;


namespace SLControls.Editors
{
    public class TMessageBox
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public Task<bool> Confirm(string content, string title, MessageType messageType)
        {
            var tcs = new TaskCompletionSource<bool>();
            //            DialogParameters p = new DialogParameters();
            RadWindow.Confirm(content, (s, e) =>
            {
                var re = e.DialogResult.HasValue ? e.DialogResult.Value : false;
                tcs.SetResult(re);
            });
            return tcs.Task;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public Task<bool> Alert(string content, string title, MessageType messageType)
        {
            var tcs = new TaskCompletionSource<bool>();
            //            DialogParameters p = new DialogParameters();
            RadWindow.Alert(content, (s, e) =>
            {
                var re = e.DialogResult.HasValue ? e.DialogResult.Value : false;
                tcs.SetResult(re);
            });
            return tcs.Task;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public Task<bool> Prompt(string content, string title, MessageType messageType)
        {
            var tcs = new TaskCompletionSource<bool>();
            //            DialogParameters p = new DialogParameters();
            RadWindow.Prompt(content, (s, e) =>
            {
                var re = e.DialogResult.HasValue ? e.DialogResult.Value : false;
                tcs.SetResult(re);
            });
            return tcs.Task;
        }
    }
}
