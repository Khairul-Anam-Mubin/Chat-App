using Chat.Api.Middlewares;
using Chat.Application.Shared;
using Chat.Notification;
using Chat.Notification.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddCommonServices(builder.Configuration)
    .AddNotifications();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LastSeenMiddleware>();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.Run();