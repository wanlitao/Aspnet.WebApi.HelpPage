using RazorEngine.Templating;

namespace WebApi.HelpPage
{
    /// <summary>
    /// 资源路径 模板Key
    /// </summary>
    public class ResourcePathTemplateKey : BaseTemplateKey
    {
        public string ResourcePath { get; private set; }

        public ResourcePathTemplateKey(string name, string resourcePath, ResolveType resolveType, ITemplateKey context)
            : base(name, resolveType, context)
		{
            ResourcePath = resourcePath;
        }

        public override string GetUniqueKeyString()
        {
            return ResourcePath;
        }

        public override bool Equals(object obj)
        {
            ResourcePathTemplateKey resourcePathTemplateKey = obj as ResourcePathTemplateKey;
            return resourcePathTemplateKey != null && resourcePathTemplateKey.ResourcePath == ResourcePath;
        }

        public override int GetHashCode()
        {
            return ResourcePath.GetHashCode();
        }
    }
}
