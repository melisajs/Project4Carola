using CarolaEnditiyLayer.Entities;

namespace Carola.WebUI.Models;

public class AboutPageViewModel
{
    public WeAreTheBest Story { get; set; } = new();
    public int CarCount { get; set; }
    public int BrandCount { get; set; }
    public int BranchCount { get; set; }
}
