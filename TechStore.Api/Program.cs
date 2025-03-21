using AutoMapper.EquivalencyExpression;
using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Abstract;
using Services.Authorization;
using Services.Authorization.Requirements;
using Services.Concrete;
using System.Text;
using TechStore.Api.MapperProfiles;
using TechStore.Api.Middlewares;
using TechStore.Api.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Core.Entities.Concrete;
using DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace TechStore.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.



        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:5173")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });
        builder.Services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo { Title = "TechStore.API", Version = "v1" });
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "JWT Token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"

            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[]{}
                }
            });
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role",
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero

                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        var revokedTokens = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
                        var jti = context.Principal?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                        if (!string.IsNullOrEmpty(jti) && await revokedTokens.IsTokenBlacklistedAsync(jti))
                        {
                            context.Fail("Token has been revoked.");
                        }
                    },
                    
                };

            });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", p => p.Requirements.Add(new RoleRequirement("Admin")));
        });

        builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionString")));
        builder.Services.AddIdentityCore<CustomIdentityUser>(o =>
        {
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 4;
            o.Password.RequiredUniqueChars = 0;
            o.SignIn.RequireConfirmedEmail = false;
            o.SignIn.RequireConfirmedPhoneNumber = false;
            o.SignIn.RequireConfirmedAccount = false;

        })
        .AddRoles<CustomIdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>();

        
        builder.Services.AddControllers(options =>
        {
            options.ModelValidatorProviders.Clear();
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errorMessages = actionContext.ModelState
                    .Where(e => e.Value!.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorResponse = new
                {
                    message = "Validation failed.",
                    errors = errorMessages
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddCollectionMappers();
            cfg.AddProfile<DtoMappingProfile>();
        });

        builder.Services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductModelValidator>(ServiceLifetime.Scoped);

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IPropertyService, PropertyService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IDetailService, DetailService>();
        builder.Services.AddScoped<IImageService, ImageService>();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

        builder.Services.AddScoped<IProductDal, ProductDal>();
        builder.Services.AddScoped<IPropertyDal, PropertyDal>();
        builder.Services.AddScoped<ICategoryDal, CategoryDal>();
        builder.Services.AddScoped<IDetailDal, DetailDal>();
        builder.Services.AddScoped<IImageDal, ImageDal>();

        builder.Services.AddHostedService<BlacklistRemovalService>();
        
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error");
        }
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseErrorMiddleware();
        app.MapControllers();

        app.Run();
    }
}
