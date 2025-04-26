using StakeHolderProject.Repositories;
using StakeHolderProject.Repositories.Interfaces;
using StakeHolderProject.Services;
using StakeHolderProject.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StakeHolderProject.Middleware;

namespace StakeHolderProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddSingleton<IStakeholderRepository, InMemoryStakeholderRepository>();
            builder.Services.AddSingleton<IVisitRepository, InMemoryVisitRepository>();            
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IClaimsService, ClaimsService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStakeholderDataService, StakeholderService>();
            builder.Services.AddScoped<IVisitService, VisitService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Stakeholder Management API", 
                    Version = "v1",
                    Description = "An API for managing stakeholders and their visits"
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stakeholder Management API v1"));
            }

            app.UseHttpsRedirection();
            
            app.UseCors();

            app.UseAuthentication();
            app.UseClaimsMiddleware();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
