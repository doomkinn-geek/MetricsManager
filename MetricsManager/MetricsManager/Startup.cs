using FluentMigrator.Runner;
using MetricsManager.Client;
using MetricsManager.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Polly;
using System;
using System.Data.SQLite;

namespace MetricsManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSqlLiteConnection(services);

            services.AddSingleton<CpuMetricsRepository>();
            services.AddSingleton<DotNetMetricsRepository>();
            services.AddSingleton<HddMetricsRepository>();
            services.AddSingleton<NetworkMetricsRepository>();
            services.AddSingleton<RamMetricsRepository>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(Configuration["ConnectionStrings:DefaultConnection"])
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            //services.AddSingleton<ILogger>();

            services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p =>
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            services.AddSingleton<IMetricsAgentClient, MetricsAgentClient>();
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            //string connectionString = Configuration["ConnectionStrings: DefaultConnection"];
            //string connectionString = Configuration.Get("ConnectionStrings");
            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            //PrepareSchema(connection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            try
            {
                // запускаем миграции
                migrationRunner.MigrateUp();
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
