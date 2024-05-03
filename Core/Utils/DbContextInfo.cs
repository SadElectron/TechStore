using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class DbContextInfo
    {
        private static IHostEnvironment _hostEnvironment;
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
        }
        public static string GetEnvironmentPath()
        {
            return _hostEnvironment.ContentRootPath;
        }
    }
}
