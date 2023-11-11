using Chat.Application.Shared;
using Chat.Framework.Extensions;
using Chat.Notification.Infrastructure;
using Chat.Notification.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCommonServices(builder.Configuration) 
    .AddNotifications();

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