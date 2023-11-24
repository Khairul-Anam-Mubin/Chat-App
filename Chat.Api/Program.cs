using Chat.Framework;
using Chat.Framework.Extensions;
using Chat.Framework.ServiceInstaller;
using Chat.Notification.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.AddGlobalConfig();

AssemblyCache.Instance.AddAllAssemblies("Chat");

builder.Services
    .InstallServices(builder.Configuration, AssemblyCache.Instance.GetAddedAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.StartInitialServices();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/NotificationHub");
});

app.Run();