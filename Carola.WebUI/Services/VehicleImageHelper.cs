namespace Carola.WebUI.Services;

public static class VehicleImageHelper
{
    public static string Resolve(string? source, string? model)
    {
        if (!string.IsNullOrWhiteSpace(source) && !source.StartsWith("/carola", StringComparison.OrdinalIgnoreCase))
        {
            return source;
        }

        return model switch
        {
            "320i" => "https://images.unsplash.com/photo-1549399542-7e3f8b79c341?auto=format&fit=crop&q=80&w=600",
            "C180" => "https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?auto=format&fit=crop&q=80&w=600",
            "Clio" => "https://images.unsplash.com/photo-1541899481282-d53bffe3c35d?auto=format&fit=crop&q=80&w=600",
            "911 Carrera" => "https://images.unsplash.com/photo-1614162692292-7ac56d7f7f1e?auto=format&fit=crop&q=80&w=600",
            "Corolla" => "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&q=80&w=600",
            "C 200" => "https://images.unsplash.com/photo-1583121274602-3e2820c69888?auto=format&fit=crop&q=80&w=600",
            "Hilux" => "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?auto=format&fit=crop&q=80&w=600",
            _ => "/images/car-placeholder.svg"
        };
    }
}
