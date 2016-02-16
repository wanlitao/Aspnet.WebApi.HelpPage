using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.HelpPage.ModelDescriptions;
using WebApi.HelpPage.Models;

namespace WebApi.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class HelpControllerBase : ApiController
    {
        public HelpControllerBase()
        {           
            Engine.Razor = RazorEngineService.Create(new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { }),
                TemplateManager = new ResolveManifestTemplateManager("WebApi.HelpPage.Views.{0}", "WebApi.HelpPage.Views.DisplayTemplates.{0}")
            });
        }

        #region 辅助方法
        protected abstract string HelpRoute { get; }

        private DynamicViewBag getInitViewBag()
        {
            var viewBag = new DynamicViewBag();
            viewBag.AddValue("HelpRoute", HelpRoute);  //将Help路由 传递到View

            return viewBag;
        }
        #endregion

        [HttpGet]
        public virtual HttpResponseMessage Index()
        {
            var viewBag = getInitViewBag();
            viewBag.AddValue("DocumentationProvider", Configuration.Services.GetDocumentationProvider());                        

            var viewContent = Engine.Razor.RunCompile("Index", typeof(Collection<ApiDescription>),
                Configuration.Services.GetApiExplorer().ApiDescriptions, viewBag);

            return new HttpResponseMessage
            {
                Content = new StringContent(viewContent, System.Text.Encoding.UTF8, "text/html")
            };
        }

        [HttpGet]
        public virtual HttpResponseMessage Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    var viewContent = Engine.Razor.RunCompile("Api", typeof(HelpPageApiModel), apiModel, getInitViewBag());
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(viewContent, System.Text.Encoding.UTF8, "text/html")
                    };
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "API not found.");
        }

        [HttpGet]
        public virtual HttpResponseMessage ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    var viewContent = Engine.Razor.RunCompile("ResourceModel", typeof(ModelDescription), modelDescription, getInitViewBag());
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(viewContent, System.Text.Encoding.UTF8, "text/html")
                    };
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Model not found.");
        }

        [HttpGet]
        public virtual HttpResponseMessage HelpPageCss(string cssFileName)
        {
            if (!String.IsNullOrEmpty(cssFileName))
            {
                var cssResourcePath = string.Format("WebApi.HelpPage.Views.Content.{0}.css", cssFileName);
                if (ManifestResourceHelper.checkExistsManifestResource(cssResourcePath))
                {
                    var cssContent = ManifestResourceHelper.getManifestResourceString(cssResourcePath);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(cssContent, System.Text.Encoding.UTF8, "text/css")
                    };
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Css not found.");
        }
    }
}