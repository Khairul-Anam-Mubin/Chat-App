using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.ServiceInstaller;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}