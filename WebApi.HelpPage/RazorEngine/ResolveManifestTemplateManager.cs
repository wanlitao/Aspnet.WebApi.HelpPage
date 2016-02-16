using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebApi.HelpPage
{
    /// <summary>
    /// 查找嵌入模板
    /// </summary>
    public class ResolveManifestTemplateManager : ITemplateManager
    {
        private readonly ReadOnlyCollection<string> layoutRoots;

        public ResolveManifestTemplateManager(params string[] layoutRoots)
        {
            this.layoutRoots = new ReadOnlyCollection<string>(new List<string>(layoutRoots));            
        }

        public ITemplateSource Resolve(ITemplateKey key)
        {
            ResourcePathTemplateKey resourcePathTemplateKey = key as ResourcePathTemplateKey;
            if (resourcePathTemplateKey == null)
            {
                throw new NotSupportedException("You can only use ResourcePathTemplateKey with this manager");
            }
            return new LoadedTemplateSource(ManifestResourceHelper.getManifestResourceString(resourcePathTemplateKey.ResourcePath),
                resourcePathTemplateKey.ResourcePath);
        }

        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            if (ManifestResourceHelper.checkExistsManifestResource(name))
            {
                return new ResourcePathTemplateKey(name, name, resolveType, context);
            }

            var existsResourceNames = layoutRoots.Select(m =>
            {
                string resourcePath = string.Format(m, name);
                if (ManifestResourceHelper.checkExistsManifestResource(resourcePath))
                {
                    return resourcePath;
                }
                resourcePath += ".cshtml";
                if (ManifestResourceHelper.checkExistsManifestResource(resourcePath))
                {
                    return resourcePath;
                }
                return null;               
            });

            var resourceName = existsResourceNames.Where(m => !string.IsNullOrEmpty(m)).FirstOrDefault();
            if (resourceName == null)
            {
                throw new InvalidOperationException(string.Format("Could not resolve template {0}", name));
            }

            return new ResourcePathTemplateKey(name, resourceName, resolveType, context);
        }

        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            throw new NotSupportedException("Adding templates dynamically is not supported! Instead you probably want to use the full-path in the name parameter?");
        }
    }
}
