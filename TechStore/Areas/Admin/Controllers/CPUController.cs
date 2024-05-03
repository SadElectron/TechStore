using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Validation;
using TechStore.Models;

namespace TechStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CPUController : Controller
    {
        private readonly ICPUService _cpuService;
        private readonly ILogger<CPUController> _logger;
        private readonly CPUValidator _cpuValidator;
        public CPUController(ICPUService cpuService, ILogger<CPUController> logger, CPUValidator cpuValidator)
        {
            _cpuService = cpuService;
            _logger = logger;
            _cpuValidator = cpuValidator;
        }
        // GET: Admin/CPUController
        public async Task<IActionResult> Index()
        {
            var cpus = await _cpuService.GetAllAsync();
            return View(cpus);
        }


        // GET: Admin/CPUController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CPUController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CPU cpu)
        {
            var validationResult = _cpuValidator.Validate(cpu);
            if (validationResult.IsValid)
            {                
                await _cpuService.AddAsync(cpu);
                return RedirectToAction("Details", cpu);
            }
            return BadRequest(validationResult.ToString());
            
        }

        // GET: Admin/CPU/Update/5
        public async Task<IActionResult> Update(Guid id)
        {
            
            var cpu = await _cpuService.GetByIdAsync(id);
            if(cpu == null)
            {
                return NotFound();
            }
            
            return View(cpu);
        }

        // POST: Admin/CPUController/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CPU cpu)
        {
            var validationResult = _cpuValidator.Validate(cpu);
            if (validationResult.IsValid)
            {
                await _cpuService.UpdateAsync(cpu);
                return RedirectToAction("Update", cpu);
            }
            return BadRequest(validationResult.ToString());
        }

        // GET: Admin/CPUController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var cpu = await _cpuService.GetByIdAsync(id);
            if (cpu == null)
            {
                return NotFound();
            }

            return View(cpu);
        }

        // POST: Admin/CPUController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CPU cpu)
        {
            var validationResult = _cpuValidator.Validate(cpu);
            if (validationResult.IsValid)
            {
                await _cpuService.DeleteAsync(cpu);
                return RedirectToAction("Index");
            }
            return BadRequest(validationResult.ToString());
        }
    }
}
