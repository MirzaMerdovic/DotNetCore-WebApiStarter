using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CoreStarter.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            return
                WebHost.CreateDefaultBuilder(args)
                    .UseApplicationInsights()
                    .UseStartup<Startup>()
                    .Build();
        }
    }
}
