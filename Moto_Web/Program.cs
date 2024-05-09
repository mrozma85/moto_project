using Microsoft.AspNetCore.Authentication.Cookies;
using Moto_Web;
using Moto_Web.Services;
using Moto_Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//MAPPER
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Dodawanie/Rejestracja serwisów do naszego projektu
builder.Services.AddHttpClient<IAdService, AdService>();
builder.Services.AddScoped<IAdService, AdService>();

builder.Services.AddHttpClient<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddHttpClient<IAdTypeService, AdTypeService>();
builder.Services.AddScoped<IAdTypeService, AdTypeService>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient<IHotsAdsService, HotsAdsService>();
builder.Services.AddScoped<IHotsAdsService, HotsAdsService>();

builder.Services.AddHttpClient<IUserListService, UserListService>();
builder.Services.AddScoped<IUserListService, UserListService>();

builder.Services.AddHttpClient<IUserRoleListService, UserRoleListService>();
builder.Services.AddScoped<IUserRoleListService, UserRoleListService>();

builder.Services.AddHttpClient<IRoleListService, RoleListService>();
builder.Services.AddScoped<IRoleListService, RoleListService>();

builder.Services.AddHttpClient<IImageService, ImageService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddHttpClient<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddHttpClient<IModelService, ModelService>();
builder.Services.AddScoped<IModelService, ModelService>();

builder.Services.AddHttpClient<IAdNameService, AdNameService>();
builder.Services.AddScoped<IAdNameService, AdNameService>();

builder.Services.AddHttpClient<IMainPageDetailsService, MainPageDetailsService>();
builder.Services.AddScoped<IMainPageDetailsService, MainPageDetailsService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.SlidingExpiration = true;
    });

//dodanie sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
