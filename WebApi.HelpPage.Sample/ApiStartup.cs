using Owin;
using System.Web.Http;

namespace WebApi.HelpPage.Sample
{
    public class ApiStartup
    {
        public static HttpConfiguration config = new HttpConfiguration();

        public static string apiRouteName = "DefaultApi";

        public void Configuration(IAppBuilder app)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: apiRouteName,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.SetDocumentationProvider(new XmlDocumentationProvider("WebApi.HelpPage.Sample.XML"));

            app.UseWebApi(config);

            config.EnsureInitialized();
        }
    }
}
