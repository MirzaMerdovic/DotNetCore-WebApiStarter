using CoreStarter.Api.Configuration;
using CoreStarter.Api.Middleware;
using CoreStarter.Api.Observability;
using CoreStarter.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace CoreStarter.Api
{
    /// <summary>
    /// OWIN configuration and setup.
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Initializes new instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(_env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

            services.AddLogging();
            services.AddHealthChecks().AddCheck<BasicHealthCheck>("basic", tags: new[] { "ready", "live" });

            // Register your types
            services.AddTransient<IFooService, FooService>();

            // Refer to this article if you require more information on CORS
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            void build(CorsPolicyBuilder b) { b.WithOrigins("*").WithMethods("*").WithHeaders("*").Build(); };
            services.AddCors(options => { options.AddPolicy("AllowAllPolicy", build); });

            services
                .AddMvc(
                    options =>
                    {
                        // Refer to this article for more details on how to properly set the caching for your needs
                        // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response
                        options.CacheProfiles.Add(
                            "default",
                            new CacheProfile
                            {
                                Duration = 600,
                                Location = ResponseCacheLocation.None
                            });

                        //var jsonInputFormatter = options.InputFormatters.OfType<JsonInputFormatter>().First();
                        //jsonInputFormatter.SupportedMediaTypes.Add("multipart/form-data");
                    })
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 2048;
                options.UseCaseSensitivePaths = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(
                    async context =>
                    {
                        var loggerFactory = app.ApplicationServices.GetService<ILogger>();

                        context.Request.Body.Position = 0;

                        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                        {
                            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                            var exceptionHandler = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

                            loggerFactory.LogError(exceptionHandler.Error, $"Failed request: {body}",null);
                        }
                    });
            });

            app.UseCors("AllowAllPolicy");

            app.UseMiddleware<RequestValidationMiddleware>();
            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };

                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new [] { "Accept-Encoding" };

                await next();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = HealthReportWriter.WriteResponse
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = HealthReportWriter.WriteResponse
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
