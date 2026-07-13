using Carola.BusinessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminLocationController : Controller
    {
        private readonly ILocationService _locationService;

        public AdminLocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var locations = await _locationService.TGetAllAsync();
            return View(locations);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Location location)
        {
            await _locationService.TInsertAsync(location);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Location location)
        {
            await _locationService.TUpdateAsync(location);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _locationService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
