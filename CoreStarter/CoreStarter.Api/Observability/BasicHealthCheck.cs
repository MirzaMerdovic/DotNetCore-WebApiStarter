using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreStarter.Api.Observability
{
    /// <summary>
    /// Basic health check implementation.
    /// </summary>
    public class BasicHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Executes the health check logic
        /// </summary>
        /// <param name="context">Instance of <see cref="HealthCheckContext"/>.</param>
        /// <param name="cancellationToken">Instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="HealthCheckResult"/>.</returns>
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var shoredinger = new Random().Next();

            return
                shoredinger % 2 == 0
                    ? Task.FromResult(HealthCheckResult.Healthy("Alive."))
                    : Task.FromResult(HealthCheckResult.Unhealthy("Dead"));
        }
    }
}