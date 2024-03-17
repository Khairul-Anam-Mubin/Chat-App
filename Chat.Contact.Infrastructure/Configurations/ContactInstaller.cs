using Chat.Contacts.Application;
using Chat.Contacts.Domain.Repositories;
using Chat.Contacts.Infrastructure.Migrations;
using Chat.Contacts.Infrastructure.Repositories;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contacts.Infrastructure.Configurations;

public class ContactInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddContactApplication();

        services.AddTransient<IIndexCreator, ContactModelIndexCreator>();
        services.AddSingleton<IContactRepository, ContactRepository>();
        services.AddSingleton<IGroupRepository, GroupRepository>();
    }
}