using Chat.Framework.Extensions;

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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#endregion

app.Run();