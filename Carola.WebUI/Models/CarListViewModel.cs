using System;
using System.Collections.Generic;
using CarolaEnditiyLayer.Entities;

namespace Carola.WebUI.Models
{
    public class CarListViewModel
    {
        public List<Car> Cars { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public List<Location> Branches { get; set; } = new();

        // Filters state to persist in UI
        public int? SelectedCategoryId { get; set; }
        public int? SelectedBrandId { get; set; }
        public int? SelectedLocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SelectedSeatCount { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SearchTerm { get; set; } = string.Empty;

        // Pagination
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
