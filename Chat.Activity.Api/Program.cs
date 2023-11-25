using Chat.Framework.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddGlobalConfig()
    .AddAllAssemblies()
    .InstallServices();

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