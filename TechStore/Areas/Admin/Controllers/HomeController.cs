using Entities.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Models;
using System.IO;

namespace TechStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IEntityFrameworkService _entityFrameworkService;
        public HomeController(IEntityFrameworkService entityFrameworkService)
        {
            _entityFrameworkService = entityFrameworkService;
        }
        public async Task<ActionResult> Index()
        {
            var model = new AdminIndexModel(
                new Dictionary<string, int>()
                );

            model.RecordCount["CPU"] = await _entityFrameworkService.GetRecordCountAsync<CPU>();
            model.RecordCount["GPU"] = await _entityFrameworkService.GetRecordCountAsync<GPU>();
            model.RecordCount["Images"] = await _entityFrameworkService.GetRecordCountAsync<Image>();
            return View(model);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEntity entity)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public  IActionResult Update(Guid id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
