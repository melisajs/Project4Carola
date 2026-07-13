using Carola.BusinessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminBrandController : Controller
    {
        private readonly IBrandService _brandService;

        public AdminBrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.TGetAllAsync();
            return View(brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            brand.Status = true;
            await _brandService.TInsertAsync(brand);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            await _brandService.TUpdateAsync(brand);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
