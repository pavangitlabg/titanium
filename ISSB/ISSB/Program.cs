using System;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


namespace ISSB
{

    public class Program
    {
        public static void Main(string[] args)
        {

            // CreateWebHostBuilder(args).Build().Run();
#if DEBUG
            BuildWebHost(args).Run();
#elif RELEASE
            BuildWebHost80(args).Run();
#endif
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
           
            .UseKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 5500);
              
                //options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                //{
                //    listenOptions.UseHttps("testCert.pfx", "testPassword");
                //});
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
            })
           .Build();
        }

        public static IWebHost BuildWebHost80(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()

            .UseKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 8080);

                //options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                //{
                //    listenOptions.UseHttps("testCert.pfx", "testPassword");
                //});
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
            })
           .Build();
        }
    }
}
