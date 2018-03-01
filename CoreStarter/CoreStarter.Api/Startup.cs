using CoreStarter.Api.Controllers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreStarter.Api
{
    /// <summary>
    /// OWIN configuration and setup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes new instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetSection("connectionStrings")["shopisticaApi"];

            // Register your types
            services.AddTransient<IValueService, ValueService>();

            // Refer to this article if you require more information on CORS
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            void build(CorsPolicyBuilder b) { b.WithOrigins("*").WithMethods("*").WithHeaders("*").AllowCredentials().Build(); };
            services.AddCors(options => { options.AddPolicy("AllowAllPolicy", build); });

            services.AddMvc(
                // Refer to this article for more details on how to properly set the caching for your needs
                // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response
                options => 
                    options.CacheProfiles.Add(
                        "default", 
                        new CacheProfile
                        {
                            Duration = 600,
                            Location = ResponseCacheLocation.None
                        }));

            services.AddResponseCaching();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Name Surname", Email = "email@gmail.com", Url = "" }
                });

                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "CoreStarter.Api.xml"));
                c.DescribeAllEnumsAsStrings();
            });

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler(options =>
                {
                    options.Run(
                        async context =>
                        {
                            context.Request.EnableRewind();
                            string body;

                            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                            {
                                body = await reader.ReadToEndAsync().ConfigureAwait(false);
                            }

                            context.Request.Body.Position = 0;

                            var telemetry = new TelemetryClient();
                            telemetry.TrackTrace("Request Info", SeverityLevel.Information, new Dictionary<string, string> { { "Body", body } });
                        });
                });
            }

            app.UseCors("AllowAllPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }

        private static CorsPolicy BuildPolicy()
        {
            // These are not good values for production environment since you will allow everything.
            return new CorsPolicyBuilder().WithOrigins("*").WithMethods("*").WithHeaders("*").AllowCredentials().Build();
        }
    }
}
