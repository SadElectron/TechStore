using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using System.Runtime.CompilerServices;

namespace TechStore.Areas.Products.Controllers
{
    [Area("Products")]
    public class CpuController : Controller
    {
        private readonly ILogger<CpuController> _logger;
        private readonly ICPUService _cpuService;

        public CpuController(ILogger<CpuController> logger, ICPUService service)
        {
            _logger = logger;
            _cpuService = service;
        }

        // GET: CpuController
        public async Task<IActionResult> Index()
        {
            var cpus = await _cpuService.GetAllAsync();
            
            return View(cpus);
        }

        // GET: CpuController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

    }
}
