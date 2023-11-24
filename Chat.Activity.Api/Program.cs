using Chat.Framework;
using Chat.Framework.ServiceInstaller;

var builder = WebApplication.CreateBuilder(args);

AssemblyCache.Instance.AddAllAssemblies("Chat");

builder.Services
    .InstallServices(builder.Configuration, AssemblyCache.Instance.GetAddedAssemblies());

var app = builder.Build();

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
});

app.Run();