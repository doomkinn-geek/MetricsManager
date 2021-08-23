using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.Client;
using MetricsManager.DAL;
using MetricsManager.DAL.Repositories;
using MetricsManager.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NLog;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

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

            services.AddSingleton<AgentsRepository>();
            services.AddSingleton<CpuMetricsRepository>();
            services.AddSingleton<DotNetMetricsRepository>();
            services.AddSingleton<HddMetricsRepository>();
            services.AddSingleton<NetworkMetricsRepository>();
            services.AddSingleton<RamMetricsRepository>();

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

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


            // ДОбавляем сервисы
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // добавляем нашу задачу
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<DotNetMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();

            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API сервиса агента сбора метрик",
                    Description = "Тут можно поиграть с api нашего сервиса",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "doomkinn",
                        Email = "doomkinn@yandex.ru",
                        Url = new Uri("https://github.com/doomkinn-geek"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "можно указать под какой лицензией все опубликовано",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                // Указываем файл из которого брать комментарии для Swagger UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
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
            // Включение middleware в пайплайн для обработки Swagger запросов.
            app.UseSwagger();
            // включение middleware для генерации swagger-ui 
            // указываем Swagger JSON эндпоинт (куда обращаться за сгенерированной спецификацией
            // по которой будет построен UI).
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса агента сбора метрик");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
