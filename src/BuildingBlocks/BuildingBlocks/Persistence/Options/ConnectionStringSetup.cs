using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Persistence.Options;

public sealed class ConnectionStringSetup : IConfigureOptions<ConnectionStringOptions>
{
    private const string ConnectionStringName = "parishDB";
    private readonly IConfiguration _configuration;

    public ConnectionStringSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(ConnectionStringOptions options) =>
        options.Value = Environment.GetEnvironmentVariable("DATABASE_URL") ?? _configuration.GetConnectionString(ConnectionStringName);
}
