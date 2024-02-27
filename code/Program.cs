using Microsoft.Extensions.Configuration.Json;
using System.Configuration;

namespace OnIPv6ChangeSend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();

            string connstr = configuration["ConnectionString"];

            var fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.MySql, connstr)
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库
                .Build();

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(fsql);
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}