using Carola.BusinessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminSliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public AdminSliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.TGetAllAsync();
            return View(sliders);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            slider.Status = true;
            await _sliderService.TInsertAsync(slider);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Slider slider)
        {
            await _sliderService.TUpdateAsync(slider);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
