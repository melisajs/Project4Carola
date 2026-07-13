using Carola.BusinessLayer.Abstract;
using Carola.BusinessLayer.Concrete;
using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.EntitiyFrameWork;
using Carola.WebUI.Hubs;
using Carola.WebUI.Services;
using Microsoft.EntityFrameworkCore;
using CarolaEnditiyLayer.Entities;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();
builder.Services.AddSingleton<TranslationService>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };
    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

// Register SQL Server EF Core Context
builder.Services.AddDbContext<CaraloContext>();

// Register Repositories (DALs)
builder.Services.AddScoped<IBrandDal, EfBrandDal>();
builder.Services.AddScoped<ICarDal, EfCarDal>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICustomerDal, EfCustomerDal>();
builder.Services.AddScoped<ILocationDal, EfLocationDal>();
builder.Services.AddScoped<IReservationDal, EfReservation>();
builder.Services.AddScoped<ISliderDal, EfSliderDal>();
builder.Services.AddScoped<IWeAreTheBestDal, EfWeAreTheBestDal>();

// Register Services (BLL Managers)
builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<ILocationService, LocationManager>();
builder.Services.AddScoped<IReservationService, ReservationManager>();
builder.Services.AddScoped<ISliderService, SliderManager>();
builder.Services.AddScoped<IWeAreTheBestService, WeAreTheBestManager>();

// Register Helper Services
builder.Services.AddScoped<EmailService>();

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Create and seed the database only when it does not exist. Existing bookings are preserved.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CaraloContext>();
    context.Database.EnsureCreated();

    if (!context.Customers.Any())
    {
        context.Customers.Add(new Customer
        {
            FirstName = "Sena",
            LastName = "Karataş",
            Email = "sena@example.com",
            Phone = "+90 555 555 55 55",
            DriverLicenseNumber = "TR-8493021-B",
            BirthDate = new DateTime(1994, 5, 12)
        });
        context.SaveChanges();
    }

    if (!context.Reservations.Any())
    {
        var customerId = context.Customers.Select(c => c.CustomerId).First();
        context.Reservations.Add(new Reservation
        {
            CarId = 6,
            CustomerId = customerId,
            PickupDate = DateTime.Today.AddDays(5),
            ReturnDate = DateTime.Today.AddDays(8),
            PickupLocationId = 4,
            ReturnLocationId = 4,
            TotalPrice = 7500,
            Description = "OCR doğrulandı | Ehliyet sınıfı: B | Demo rezervasyon",
            Status = "Bekliyor"
        });
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map SignalR Chat Hub
app.MapHub<ChatHub>("/chatHub");

app.Run();
