using Application.Contracts.Repositories;
using Application.Contracts.Repositories.BaseRepository;
using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Contracts.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.DataAccess;
using Infrastructure.Extensions.Authentication;
using Infrastructure.ImplementationContract.Repositories;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;
using Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;
using Infrastructure.ImplementationContract.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobileApp.HelpersApi.Extensions.Seed;

namespace MobileApp.HelpersApi.Extensions.DI;

public static class RegisterService
{
    public static IServiceCollection AddServices(this WebApplicationBuilder builder)
    {
        //config for authentication in swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.ToString());
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "JWT Authorization header using the Bearer scheme \r\n\r\n" +
                              "Example:\"Bearer 1234aaabbbccc\" "
            });


            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            // Добавляем требования безопасности для всех эндпоинтов
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    new string[] { }
                }
            });
        });


        //registration controller
        builder.Services.AddControllers();


        builder.Services.AddDbContext<DataContext>(x =>
        {
            var connection =
                builder.Configuration["DATABASE_URL"];

            x.UseNpgsql(connection, o =>
                o.EnableRetryOnFailure());
            x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            x.LogTo(Console.WriteLine);
        });

        //fluent validation ? for automatic validation ,need use mediatr.
        //builder.Services.AddValidatorsFromAssembly(typeof(Application.Application).Assembly);


        //add authentication methods
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthenticationService.GetSymmetricSecurityKey(builder.Configuration["JWT:key"]!),
                    ValidateIssuerSigningKey = true,
                };
            });

        //add  authorization
        builder.Services.AddAuthorization();


        //registration generic repository
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped(typeof(IGenericAddRepository<>), typeof(GenericAddRepository<>));
        builder.Services.AddScoped(typeof(IGenericUpdateRepository<>), typeof(GenericUpdateRepository<>));
        builder.Services.AddScoped(typeof(IGenericDeleteRepository<>), typeof(GenericDeleteRepository<>));
        builder.Services.AddScoped(typeof(IGenericFindRepository<>), typeof(GenericFindRepository<>));

        //registration repository
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<ICarDocumentRepository, CarDocumentRepository>();
        builder.Services.AddScoped<ICarPriceRepository, CarPriceRepository>();
        builder.Services.AddScoped<ICarImageRepository, CarImageRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IRentalCompanyRepository, RentalCompanyRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

        //registration services
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<ICarService, CarService>();
        builder.Services.AddScoped<ICarDocumentService, CarDocumentService>();
        builder.Services.AddScoped<ICarImageService, CarImageService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICarPriceService, CarPriceService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<Seeder>();
        builder.Services.AddScoped<IRentalCompanyService, RentalCompanyService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();

        //registration validation
        
        builder.Services.AddValidatorsFromAssemblyContaining<Application.Validations.Booking.Create>();
        builder.Services.AddFluentValidationAutoValidation();
        

        // добавляем сервисы CORS
        builder.Services.AddCors();

        return builder.Services;
    }

    public static async Task<WebApplication> UseMiddlewares(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();

            await db.Database.MigrateAsync(); // ← ВАЖНО

            var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
            await seeder.Initial();
        }

        try
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler("/error");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
            });
            await app.RunAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return app;
    }
}