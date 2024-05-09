using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moto_API;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository;
using Moto_API.Repository.IRepository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//baza
builder.Services.AddDbContext<MotoDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
.AddEntityFrameworkStores<MotoDbContext>();

//mapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
//repository
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IAdRepository, AdRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAdTypeRepository, AdTypeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotsAdsRepository, HotsAdsRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IListUsersRepository, ListUsersRepository>();
builder.Services.AddScoped<IListUserRolesRepository, ListUserRolesRepository>();
builder.Services.AddScoped<IListRoleRepository, ListRoleRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepository>();
builder.Services.AddScoped<IAdNameRepository, AdNameRepository>();
builder.Services.AddScoped<IMainPageDetailsRepository, MainPageDetailsRepository>();

//Authentication token

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience= false,
        };
    });

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization header using the Bearer scheme. .\r\n\r\n " +
        "Enter 'Bearer' [space] and then your token in the next input below.\r\n\r\n " +
        "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header

            },
            new List<string>()
        }
    });
});

builder.Services.AddCors(o => {
    o.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(@"\\server\path"),
//    RequestPath = new PathString("/MyPath"),
//    EnableDirectoryBrowsing = false
//});

app.MapControllers();

app.UseCors("AllowAll");

app.MapPost("/uploadFile", async (HttpRequest httpRequest) =>
{
    if (!httpRequest.HasFormContentType)
    {
        return Results.BadRequest();
    }

    try
    {
        var formCollection = await httpRequest.ReadFormAsync();

        var iFormFile = formCollection.Files["fileContent"];

        if (iFormFile is null || iFormFile.Length == 0)
        {
            return Results.BadRequest();
        }

        using var stream = iFormFile.OpenReadStream();

        var localFilePath = Path.Combine("Images", iFormFile.FileName);

        using var localFileStream = File.OpenWrite(localFilePath);

        await stream.CopyToAsync(localFileStream);

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex.Message);
        return Results.BadRequest();
    }
})
    .Produces(StatusCodes.Status201Created)
    .WithName("UploadFile")
    .WithOpenApi();

app.Run();
