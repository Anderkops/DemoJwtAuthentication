using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System;
using System.IO;

namespace DemoJwt.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SchemaFilter<SwaggerExcludePropertiesFilter>();

                options.SwaggerDoc("v1", 
                    
                    new OpenApiInfo
                    { 
                        Version = "v1",
                        Title = "ToDo API",
                        Description = "An ASP.NET Core Web API for managing ToDo items",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Example Contact",
                            Url = new Uri("https://example.com/contact")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Example License",
                            Url = new Uri("https://example.com/license")
                        }
                    });

                options.AddSecurityDefinition("bearer", 
                    new OpenApiSecurityScheme
                    {                        
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "tomsAuth"
                    });
 
                SetXmlDocumentation(options);
            });
        }

        private static void SetXmlDocumentation(SwaggerGenOptions options)
        {
            var xmlDocumentPath = GetXmlDocumentPath();
            var existsXmlDocument = File.Exists(xmlDocumentPath);

            if (existsXmlDocument)
            {
                options.IncludeXmlComments(xmlDocumentPath);
            }
        }
        private static string GetXmlDocumentPath()
        {
            var basePath = AppContext.BaseDirectory;
            var applicationName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            var fileName = System.IO.Path.GetFileName(applicationName + ".xml");             
            return Path.Combine(basePath, $"{applicationName}.xml");
        }
    }
}