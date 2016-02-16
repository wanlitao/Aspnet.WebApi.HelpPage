using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace WebApi.HelpPage
{
    /// <summary>
    /// 嵌入资源助手
    /// </summary>
    public static class ManifestResourceHelper
    {
        private static readonly IList<string> manifestResourceNames; //缓存程序集中所有嵌入资源名称

        static ManifestResourceHelper()
        {
            manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList();
        }

        /// <summary>
        /// 是否存在嵌入资源
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <returns></returns>
        public static bool checkExistsManifestResource(string resourcePath)
        {
            if (string.IsNullOrEmpty(resourcePath))
                return false;

            return manifestResourceNames.Contains(resourcePath);
        }

        /// <summary>
        /// 获取嵌入资源字符串
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <returns></returns>
        public static string getManifestResourceString(string resourcePath)
        {
            if (!checkExistsManifestResource(resourcePath))
                return string.Empty;

            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            using (var reader = new StreamReader(resourceStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
