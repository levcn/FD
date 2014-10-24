using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;


namespace Fw.DataAccess.HttpModule
{
    public class MyHttpHandlerFactory : IHttpHandlerFactory
    {
        // Methods
        public virtual IHttpHandler GetHandler(HttpContext app, string requestType, string url, string pathTranslated)
        {
//            UrlParser
            return PageParser.GetCompiledPageInstance(url, pathTranslated, app);
        }

        public virtual void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
