using Chat.Contact.Application;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Infrastructure.Migrations;
using Chat.Contact.Infrastructure.Repositories;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Infrastructure.Configurations;

public class ContactInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddContactApplication();

        services.AddTransient<IIndexCreator, ContactModelIndexCreator>();
        services.AddSingleton<IContactRepository, ContactRepository>();
        services.AddSingleton<IGroupRepository, GroupRepository>();
        services.AddSingleton<IGroupMemberRepository, GroupMemberRepository>();
    }
}