using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstract;
using Services.Concrete;
using Services.Validation;

namespace TechStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICPUDal, CPUDal>();
            builder.Services.AddScoped<ICPUService, CPUService>();

            builder.Services.AddScoped<IGPUDal, GPUDal>();
            builder.Services.AddScoped<IGPUService, GPUService>();

            builder.Services.AddScoped<IImageDal, ImageDal>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IEntityFrameworkDal, EntityFrameworkDal>();
            builder.Services.AddScoped<IEntityFrameworkService, EntityFrameworkService>();

            builder.Services.AddSingleton<CPUValidator>();

            


            var app = builder.Build();

            DbContextInfo.Initialize(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.MapAreaControllerRoute(
                name: "default",
                areaName: "Products",
                pattern: "{controller=Home}/{action=Index}");

            app.MapAreaControllerRoute(
                name: "AdminAccess",
                areaName: "Admin",
                pattern: "admin/{controller=Home}/{action=Index}/{id?}");



            app.Run();
        }
    }
}
