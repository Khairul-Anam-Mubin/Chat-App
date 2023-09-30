using Chat.Api.ChatModule.Hubs;
using Chat.Api.Middlewares;
using Chat.Application.Shared;

namespace Chat.Api
{
    public class Startup : AStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment) : base(configuration, hostEnvironment)
        {

        }

        public override void EndpointRouteBuilder(IEndpointRouteBuilder endpointRouteBuilder)
        {
            base.EndpointRouteBuilder(endpointRouteBuilder);
            endpointRouteBuilder.MapHub<ChatHub>("/chatHub");
        }

        public override void UseMiddlewaresBeforeController(IApplicationBuilder app)
        {
            base.UseMiddlewaresBeforeController(app);
            app.UseMiddleware<LastSeenMiddleware>();
        }
    }
}
