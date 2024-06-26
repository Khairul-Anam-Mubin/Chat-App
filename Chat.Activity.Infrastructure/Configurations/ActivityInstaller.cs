﻿using Chat.Activity.Application;
using Chat.Activity.Domain.Repositories;
using Chat.Activity.Infrastructure.Migrations;
using Chat.Activity.Infrastructure.Repositories;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Infrastructure.Configurations;

public class ActivityInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddActivityApplication()
            .AddScoped<IPresenceRepository, PresneceRepository>();

        services.AddTransient<IIndexCreator, PresenceIndexCreator>();
    }
}