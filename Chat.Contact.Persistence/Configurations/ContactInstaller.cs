using Chat.Contact.Application;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Infrastructure.Repositories;
using Chat.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Infrastructure.Configurations;

public class ContactInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddContactApplication();
        services.AddSingleton<IContactRepository, ContactRepository>();
    }
}