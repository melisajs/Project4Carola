using Carola.BusinessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class AdminWeAreTheBestController : Controller
    {
        private readonly IWeAreTheBestService _weAreTheBestService;

        public AdminWeAreTheBestController(IWeAreTheBestService weAreTheBestService)
        {
            _weAreTheBestService = weAreTheBestService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _weAreTheBestService.TGetAllAsync();
            var item = list.FirstOrDefault();
            if (item == null)
            {
                item = new WeAreTheBest
                {
                    Title = "Explore The World With Your Own Way Of Driving",
                    Description = "We offer the best car rental services with clean vehicles, round-the-clock support, and convenient branch pick-ups.",
                    Feature1Title = "Free Pick Up & Drop",
                    Feature1Description = "Your convenience matter. Complimentary pick-up and drop-off service for any your vehicle, a stress-free experience.",
                    Feature2Title = "24/7 Road Assistant",
                    Feature2Description = "No matter the time or place, and our 24/7 roadside assistance ensures you're never stranded. Drive confidently with support."
                };
                await _weAreTheBestService.TInsertAsync(item);
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WeAreTheBest model)
        {
            await _weAreTheBestService.TUpdateAsync(model);
            return RedirectToAction("Index");
        }
    }
}
