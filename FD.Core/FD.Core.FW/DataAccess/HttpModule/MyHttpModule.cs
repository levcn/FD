using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace Fw.DataAccess.HttpModule
{
    public class MyHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);

            context.EndRequest += new EventHandler(Application_EndRequest);

        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
