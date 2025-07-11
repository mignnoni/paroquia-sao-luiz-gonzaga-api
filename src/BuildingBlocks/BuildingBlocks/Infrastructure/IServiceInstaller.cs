using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
