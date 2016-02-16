using System.Web.Http;

namespace WebApi.HelpPage.Sample.Controllers
{
    public class StatusController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return "ok";
        }
    }
}