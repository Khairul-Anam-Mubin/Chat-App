using Chat.Contacts.Application;
using Chat.Contacts.Domain.Repositories;
using Chat.Contacts.Infrastructure.Migrations;
using Chat.Contacts.Infrastructure.Repositories;
using KCluster.Framework.ORM.Interfaces;
using KCluster.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contacts.Infrastructure.Configurations;

public class ContactInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddContactApplication();

        services.AddTransient<IIndexCreator, ContactModelIndexCreator>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
    }
}