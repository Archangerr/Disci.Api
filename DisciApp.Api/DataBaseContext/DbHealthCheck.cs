using Devart.Data.PostgreSql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace DisciApp.Api.DataBaseContext
{
    public class DbHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var sqlConnection = new SqlConnection("Data Source=DESKTOP-KK8B4QE\\SQLEXPRESS01;Initial Catalog=SummerSchool;User id=sa;Password=1234qqqQ;TrustServerCertificate=True"))
                {
                    if (sqlConnection.State != System.Data.ConnectionState.Open)
                        sqlConnection.Open();

                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnection.Close();
                        return Task.FromResult(
                        HealthCheckResult.Healthy("The database is up and running."));
                    }
                }

                return Task.FromResult(
                    new HealthCheckResult(
                    context.Registration.FailureStatus, "The database is down."));
            }
            catch (Exception)
            {
                return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus, "The database is down."));
            }
        }
    }
}
