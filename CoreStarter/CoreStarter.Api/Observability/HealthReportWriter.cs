using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreStarter.Api.Observability
{
    /// <summary>
    /// Contains utility methods for configuring health checks
    /// </summary>
    public static class HealthReportWriter
    {
        /// <summary>
        /// Tries to write the health check result data into HTTP response message.
        /// </summary>
        /// <param name="httpContext">Instance of <see cref="HttpContext"/>.</param>
        /// <param name="result">Instance of <see cref="HealthReport"/>.</param>
        /// <returns>A task.</returns>
        public static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            _ = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _ = result ?? throw new ArgumentNullException(nameof(result));

            httpContext.Response.ContentType = "application/json";

            var payload = new
            {
                Status = result.Status.ToString(),
                HealthChecks = result.Entries.Select(pair => new
                {
                    Name = pair.Key,
                    Report = new
                    {
                        Status = pair.Value.Status.ToString(),
                        Descriptions = pair.Value.Description,
                        ElapsedMilliseconds = pair.Value.Duration.TotalMilliseconds,
                        Tags = pair.Value.Tags,
                        Data = pair.Value.Data.Select(kvp => new
                        {
                            Key = kvp.Key,
                            Value = kvp.Value
                        })
                    }
                })
            };

            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(payload, Formatting.Indented));
        }
    }
}