using System.Web.Http.Routing;
using WebApi.HelpPage.Controllers;
using System.Net.Http;

namespace WebApi.HelpPage.Sample.Controllers
{
    public class HelpController : HelpControllerBase
    {
        private readonly HttpRouteValueDictionary helpRouteValues;

        public HelpController()
        {
            helpRouteValues = new HttpRouteValueDictionary(new { controller = "Help" });
            helpRouteValues.Add(HttpRoute.HttpRouteKey, true);
        }

        protected override string HelpRoute
        {
            get
            {
                IHttpVirtualPathData virtualPathData = Configuration.Routes.GetVirtualPath(
                    new HttpRequestMessage(),
                    ApiStartup.apiRouteName, helpRouteValues);

                if (virtualPathData == null)
                {
                    return string.Empty;
                }
                return virtualPathData.VirtualPath;
            }
        }
    }
}
