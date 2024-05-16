using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using System.Diagnostics;
using TechStore.Areas.Products.Models;


namespace TechStore.Areas.Products.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICPUService _cpuService;
        private readonly IGPUService _gpuService;

        public HomeController(ILogger<HomeController> logger, ICPUService cpuService, IGPUService gpuService)
        {
            _logger = logger;
            _cpuService = cpuService;
            _gpuService = gpuService;
        }

        public async Task<HomePageModel> Index()
        {
            var homePageModel = new HomePageModel
            {
                Cpus = await _cpuService.GetAllAsync(),
                Gpus = await _gpuService.GetAllAsync()
            };
            return homePageModel;
        }
        public async Task<IActionResult> Details(Guid productId, string type)
        {

            if (productId != Guid.Empty && !string.IsNullOrEmpty(type.Trim().ToLower()))
            {
                ProductDetailsModel productDetailsModel;
                switch (type.Trim().ToLower())
                {
                    case "cpu":
                        productDetailsModel = new (){
                        Cpu = await _cpuService.GetByIdAsync(productId)
                        };
                        return Ok(productDetailsModel);
                    case "gpu":
                        productDetailsModel = new()
                        {
                            Gpu = await _gpuService.GetByIdAsync(productId)
                        };
                        return Ok(productDetailsModel);

                    default:
                        return BadRequest();
                }
            }
            

            return BadRequest();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
