using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using SQLitePCL;
using Services.Validation;

namespace TechStore.Areas.Admin.Controllers
{
    public class GPUController : Controller
    {
        private readonly IGPUService _gpuService;
        private readonly GPUValidator _gpuValidator;

        public GPUController(IGPUService gPUService)
        {
            _gpuService = gPUService;
        }

        // GET: GPU
        public async Task<IActionResult> Index()
        {
            var gpus = await _gpuService.GetAllAsync();
            return View();
        }

        // GET: GPU/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gPU = await _gpuService.GetByIdAsync(id);

            if (gPU == null)
            {
                return NotFound();
            }

            return View(gPU);
        }

        // GET: GPU/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GPU/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GPU gpu)
        {
            var validationResult = _gpuValidator.Validate(gpu);
            if (validationResult.IsValid)
            {
                await _gpuService.AddAsync(gpu);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(validationResult.ToString());
        }

        // GET: GPU/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gPU = await _gpuService.GetByIdAsync(id);
            if (gPU == null)
            {
                return NotFound();
            }
            return View(gPU);
        }

        // POST: GPU/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Brand,ModelName,Chipset,VramSize,VramType,BaseClockSpeed,BoostClockSpeed,MemoryBusWidth,Tdp,PowerConnectors,Interface,DisplayPorts,LaunchDate,Price")] GPU gPU)
        {
            if (id != gPU.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gpuService.UpdateAsync(gPU);       
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GPUExists(gPU.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gPU);
        }

        // GET: GPU/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gPU = await _gpuService.GetByIdAsync(id);
            if (gPU == null)
            {
                return NotFound();
            }

            return View(gPU);
        }

        // POST: GPU/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var gPU = await _gpuService.GetByIdAsync(id);
            if (gPU != null)
            {
                await _gpuService.DeleteAsync(gPU);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GPUExists(Guid id)
        {
            var gpu = _gpuService.GetByIdAsync(id);
            if (gpu == null)
            {  
                return false; 
            }

            return true;
        }
    }
}
