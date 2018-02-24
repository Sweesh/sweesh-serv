using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using StructureMap.AspNetCore;

namespace Sweesh.Core.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseStructureMap()
                    .UseKestrel()
                    .Build();
    }
}
