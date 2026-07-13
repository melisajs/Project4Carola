using System.Globalization;

namespace Carola.WebUI.Services;

public sealed class TranslationService
{
    public string Translate(string turkish, string english)
    {
        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en" ? english : turkish;
    }

    public string Data(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || CultureInfo.CurrentUICulture.TwoLetterISOLanguageName != "en")
        {
            return value ?? string.Empty;
        }

        return value.Trim() switch
        {
            "Spor" => "Sport",
            "Ekonomik" => "Economy",
            "Kamyonet" => "Pickup",
            "Benzin" => "Petrol",
            "Dizel" => "Diesel",
            "Elektrik" => "Electric",
            "Hibrit" => "Hybrid",
            "Otomatik" => "Automatic",
            "Manuel" => "Manual",
            "Müsait" => "Available",
            "Bekliyor" => "Pending",
            "Onaylandı" => "Approved",
            "İptal" => "Cancelled",
            "İstanbul Havalimanı" => "Istanbul Airport",
            "Antalya Havalimanı" => "Antalya Airport",
            "İzmir Havalimanı" => "Izmir Airport",
            "Antalya Konyaaltı Şubesi" => "Antalya Konyaalti Branch",
            _ => value
        };
    }
}
