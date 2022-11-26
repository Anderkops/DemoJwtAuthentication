using DemoJwt.Api.Extensions;
using DemoJwt.IoC;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DemoJwt.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddLogging();
            services.AddSwagger();

            services.AddApplicationServices();
            services.AddAuthorizedMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "swagger";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo JWT Api");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole());
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            using (loggerFactory = serviceProvider.GetService<ILoggerFactory>())
            {
                UseExceptionHandling(app, loggerFactory);                
            }

            app.UseAuthentication();            
            app.UseRouting();
        }

        private static void UseExceptionHandling(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    // using static System.Net.Mime.MediaTypeNames;
                    context.Response.ContentType = Text.Plain;

                    await context.Response.WriteAsync("Ocorreu uma exceção.");

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                    {
                        await context.Response.WriteAsync("Arquivo não encontrado.");
                    }

                    if (exceptionHandlerPathFeature?.Path == "/")
                    {
                        await context.Response.WriteAsync("Home Page.");
                    }
                });
            });

        }
    }
}
