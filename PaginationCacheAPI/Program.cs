using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StructureMap;
using System;

namespace PaginationCacheAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static void Main(string[] args)
        //{
        //    var service = new ServiceCollection().AddFluentMigratorCore().ConfigureRunner(
        //        builder => builder.AddPostgres()
        //        .WithGlobalConnectionString("User ID=postgres;Password=asd123;Server=localhost;Port=5432;Database=Pagination;Integrated Security=true;Pooling=true;")
        //        .WithMigrationsIn(typeof(InitialMigration).Assembly)).BuildServiceProvider();

        //    var container = new Container();
        //    container.Configure(
        //        config =>
        //        {
        //            config.Scan(_ =>
        //            {
        //                _.AssemblyContainingType(typeof(Program));


        //                _.WithDefaultConventions();


        //                _.AssemblyContainingType<IMigrationRunnerBuilder>();
        //            }
        //                );
        //        }
        //        );

        //    try
        //    {
        //        using (var scope = service.CreateScope())
        //        {
        //            var context = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        //            context.MigrateUp();
        //            //context.MigrateDown(202208150930);
        //            Console.WriteLine("Succesful");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    Console.ReadLine();

        //    CreateHostBuilder(args).Build().Run();
        //}
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
