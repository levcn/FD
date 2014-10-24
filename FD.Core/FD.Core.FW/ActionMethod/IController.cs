using System.Collections;
using System.Web;


namespace Fw.ActionMethod
{
    public interface IController
    {
    }

    public class BaseController : IController
    {
        public HttpContext Current
        {
            get
            {
                return HttpContext.Current;
            }
        }
        public HttpResponse Response
        {
            get
            {
                return Current.Response;
            }
        }
        public HttpServerUtility Server
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }
        public HttpRequest Request
        {
            get
            {
                return Current.Request;
            }

        }
    }
}