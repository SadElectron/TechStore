using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstract;
using Services.Concrete;
using Services.Validation;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Areas.Identity.Data;


namespace TechStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Identity
            var connectionString = builder.Configuration.GetConnectionString("TechStoreIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'TechStoreIdentityContextConnection' not found.");
            
            builder.Services.AddDbContext<TechStoreIdentityContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddDefaultIdentity<TechStoreUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TechStoreIdentityContext>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            //Identity/

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICPUDal, CPUDal>();
            builder.Services.AddScoped<ICPUService, CPUService>();

            builder.Services.AddScoped<IGPUDal, GPUDal>();
            builder.Services.AddScoped<IGPUService, GPUService>();

            builder.Services.AddScoped<IImageDal, ImageDal>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IEntityFrameworkDal, ProductDal>();
            builder.Services.AddScoped<IEntityFrameworkService, EntityFrameworkService>();

            builder.Services.AddSingleton<CPUValidator>();
            builder.Services.AddSingleton<GPUValidator>();



            var app = builder.Build();


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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "AdminAccess",
                areaName: "Admin",
                pattern: "admin/{controller=Home}/{action=Index}/{id?}");


            app.MapRazorPages();
            app.Run();
        }
    }
}
