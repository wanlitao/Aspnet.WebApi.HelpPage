using Microsoft.Owin.Hosting;
using System;

namespace WebApi.HelpPage.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = new UriBuilder("http", "+", 8088);
            WebApp.Start<ApiStartup>(baseAddress.ToString());

            Console.WriteLine("Service listening at: {0}", baseAddress);
            Console.WriteLine("Help page available at: {0}api/help", baseAddress);
            Console.WriteLine("Press Enter to shutdown the service.");

            Console.ReadLine();
        }
    }
}
