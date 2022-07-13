using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Slack.Core;

namespace Talent.WebAdmin
{
  public class Program
  {
    public static int Main(string[] args)
    {
      var configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.Development.json", optional: true)
          .AddJsonFile("appsettings.Staging.json", optional: true)
          .AddJsonFile("appsettings.json", optional: true)
          .AddUserSecrets<Startup>()
          .AddEnvironmentVariables()
          .Build();

      //var slackUrl = configuration.GetValue<string>("SlackLogHooks");

      Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          //.WriteTo.Async(Q => Q.Slack(slackUrl, null, Serilog.Events.LogEventLevel.Warning), blockWhenFull: false)
          .CreateLogger();

      try
      {
        Log.Information("Starting web host");
        CreateWebHostBuilder(args).Build().Run();

        return 0;
      }
      catch (Exception e)
      {
        Log.Fatal(e, "Host terminated unexpectedly");

        return 1;
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://localhost:45812/")
            .UseStartup<Startup>()
            .UseSerilog();
  }
}
