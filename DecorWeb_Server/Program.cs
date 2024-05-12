using Decor_Business.Repository;
using Decor_Business.Repository.IRepository;
using Decor_DataAccess.Data;
using DecorWeb_Server.Data;
using DecorWeb_Server.Service;
using DecorWeb_Server.Service.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzIzNDQwNUAzMjM1MmUzMDJlMzBrVzMydmRMUHRuVXdIU2VMeVh0b0E0TWFoL3VZeTl2a3FqcXhKR2JTNFFBPQ==");

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
builder.Services.AddScoped<IFileUpload, FileUpload>();
builder.Services.AddDbContext<ApplicationDBContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
