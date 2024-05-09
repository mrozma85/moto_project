using Microsoft.AspNetCore.Mvc;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.VM;
using Moto_Web.Services;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Windows.Services.Maps;

namespace Moto_Web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IImageService _imageService;

        Uri baseAddress = new Uri("https://localhost:7001/");
        public IHttpClientFactory httpClient { get; set; }
        HttpClient client;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            this.httpClient = httpClient;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(VehicleImages images, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var adId = HttpContext.Session.GetInt32("adid");
                byte[] bytes;

                foreach (var formFile in files)
                {
                    if(formFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            bytes = memoryStream.ToArray();
                        }

                        using var fileContent = new ByteArrayContent(bytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        using var form = new MultipartFormDataContent
                        {
                            { fileContent, "fileContent", Path.GetFileName(formFile.FileName) }
                        };

                        var img = new VehicleImages
                        {
                            Id =0,
                            FileName = formFile.FileName,
                            FileData = bytes,
                            AdId = (int)adId,
                        };

                        images.Id = img.Id;
                        images.FileName = img.FileName;
                        images.FileData = img.FileData;
                        images.AdId = img.AdId;

                        var response = await _imageService.CreateAsync<APIResponse>(images, HttpContext.Session.GetString(SD.SessionToken));
                    }
                }
                TempData["success"] = "Zdjęcia dodano";
                return RedirectToAction("UserIdList", "Ads");
            }
            TempData["error"] = "Błąd";
            return View();
        }

        //[Authorize]
        public async Task<IActionResult> UploadImage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id, VehicleImages images, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var adId1 = HttpContext.Session.GetInt32("adid");
                var id1 = adId1;

                var imagesTemp = new List<VehicleImages>();
                var imagesDelete = new List<VehicleImages>();

                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/AdMauiImages/" + id1).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    imagesTemp = JsonConvert.DeserializeObject<List<VehicleImages>>(data);
                }

                foreach (var temp in imagesTemp)
                {
                    imagesDelete.Add(temp);
                    HttpResponseMessage responseDelete = client.DeleteAsync(client.BaseAddress + "api/AdMauiImages/" + id1).Result;
                }

                byte[] bytes;

                foreach (var formFile in files)
                {
                    var adId = HttpContext.Session.GetInt32("adid");

                    if (formFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            bytes = memoryStream.ToArray();
                        }

                        using var fileContent = new ByteArrayContent(bytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        using var form = new MultipartFormDataContent
                        {
                            { fileContent, "fileContent", Path.GetFileName(formFile.FileName) }
                        };

                        var img = new VehicleImages
                        {
                            Id =0,
                            FileName = formFile.FileName,
                            FileData = bytes,
                            AdId = (int)adId,
                        };

                        images.Id = img.Id;
                        images.FileName = img.FileName;
                        images.FileData = img.FileData;
                        images.AdId = img.AdId;

                        var responsAdd = await _imageService.CreateAsync<APIResponse>(images, HttpContext.Session.GetString(SD.SessionToken));
                    }
                }
                TempData["success"] = "Ogłoszenie zostało zaktualizowane";
                return RedirectToAction("IndexAds", "Ads");
            }
            TempData["error"] = "Błąd";
            return View();
        }
    }
}
