using Chat.Framework.Extensions;
using Chat.Notification.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

#region Service Configuration

builder
    .AddGlobalConfig()
    .AddAllAssemblies()
    .InstallServices();

#endregion

var app = builder.Build();

#region StartUp Services

app.DoCreateIndexes();
app.DoMigration();
app.StartInitialServices();

#endregion

#region Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

#endregion

app.Run();