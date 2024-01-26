
using FlowrSpot.Application.Mapping;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Application.Services;
using FlowrSpot.Dtos;
using FlowrSpot.Infrastructure.Data;
using FlowrSpot.Infrastructure.Repositories;
using FlowrSpot.WebAPI.Authentication.Basic;
using FlowrSpot.WebAPI.Common.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FlowrSpot.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(BasicAuthenticationDefaults.AuthenticationSchemes,
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = BasicAuthenticationDefaults.AuthenticationSchemes,
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Basic authorization header"
                    });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = BasicAuthenticationDefaults.AuthenticationSchemes
                            }
                        },
                        new string[] { "Basic " }
                    }
                });
            });

            builder.Services.AddDbContext<DataContext>(option =>
            {
                option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
            });

            builder.Services.AddScoped<IFlowerRepository, FlowerRepository>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<ISightingRepository, SightingRepository>();

            builder.Services.AddScoped<ISightingRepository, SightingRepository>();

            builder.Services.AddScoped<ILikeRepository, LikeRepository>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();

            builder.Services.AddScoped<ISightingService, SightingService>();

            builder.Services.AddScoped<ILikeService, LikeService>();

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddAuthentication()
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
                   BasicAuthenticationDefaults.AuthenticationSchemes, null);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
