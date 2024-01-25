using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Web;

namespace ChoreBoard.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            var startup = new Startup(builder.Configuration);

            startup.ConfigureServices(builder.Services);

            AddLogging(builder);

            WebApplication app = builder.Build();

            startup.Configure(app, app.Environment);

            app.Run();
        }

        private static void AddLogging(WebApplicationBuilder builder)
        {
            builder.Host.UseNLog();

            NLog.LogManager.Configuration = new NLogLoggingConfiguration(
                builder.Configuration.GetSection("NLog")
            );
        }
    }
}