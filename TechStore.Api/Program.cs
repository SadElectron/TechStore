using AutoMapper.EquivalencyExpression;
using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Abstract;
using Services.Authentication.Jwt;
using Services.Authorization;
using Services.Authorization.Requirements;
using Services.Concrete;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TechStore.Api.MapperProfiles;
using TechStore.Api.Middlewares;
using TechStore.Api.Models.Product;
using TechStore.Api.Validation.Utils;
using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechStore.Api
{
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
                    Scheme = JwtBearerDefaults.AuthenticationScheme

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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", p => p.Requirements.Add(new RoleRequirement("Admin")));
            });


            builder.Services.AddControllers(options =>
            {
                options.ModelValidatorProviders.Clear();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errorMessages = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
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

            builder.Services.AddScoped<TokenProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductModelValidator>(ServiceLifetime.Scoped);

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IDetailService, DetailService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IProductDal, ProductDal>();
            builder.Services.AddScoped<IPropertyDal, PropertyDal>();
            builder.Services.AddScoped<ICategoryDal, CategoryDal>();
            builder.Services.AddScoped<IDetailDal, DetailDal>();
            builder.Services.AddScoped<IImageDal, ImageDal>();
            builder.Services.AddScoped<IAuthDal, AuthDal>();
            builder.Services.AddScoped<IRefreshTokenDal, RefreshTokenDal>();
            builder.Services.AddScoped(typeof(GenericValidator<,>));

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.UseErrorMiddleware();
            app.Run();
        }
    }
}
