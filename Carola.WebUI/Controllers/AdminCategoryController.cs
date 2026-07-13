using Carola.BusinessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.TGetAllAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _categoryService.TInsertAsync(category);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            await _categoryService.TUpdateAsync(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
