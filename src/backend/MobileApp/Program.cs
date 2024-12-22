using MobileApp.HelpersApi.Extensions.DI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServices();

WebApplication app = builder.Build();

app.UseMiddlewares();