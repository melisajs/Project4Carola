using System.Collections.Generic;
using CarolaEnditiyLayer.Entities;

namespace Carola.WebUI.Models
{
    public class HomeIndexViewModel
    {
        public List<Slider> Sliders { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public WeAreTheBest WeAreTheBest { get; set; } = new();
        public List<Car> LatestCars { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public List<Location> Branches { get; set; } = new();
    }
}
