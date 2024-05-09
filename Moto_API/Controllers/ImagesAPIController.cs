using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Helpers;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Net;

namespace Moto_API.Controllers
{
    [Authorize]
    [Route("api/ImageAPI")]
    [ApiController]
    public class ImagesAPIController : ControllerBase
    {
        private readonly IImageRepository _dbImages;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesAPIController(IImageRepository dbImages, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _dbImages = dbImages;
            _mapper = mapper;
            this._response = new();
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("PostSingleImage")]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> PostSingleImage([FromForm] FileUploadModel fileDetails)
        {
            try
            {
                if (fileDetails == null)
                {
                    return BadRequest();
                }

                await _dbImages.PostFileAsync(fileDetails.FileDetails);
                _response.Result = fileDetails.FileDetails;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("PostMultipleFile")]
        public async Task<ActionResult<APIResponse>> PostMultipleFiles([FromForm] List<IFormFile> fileDetails)
        {
            try
            {
                if (fileDetails == null)
                {
                    return BadRequest();
                }

                await _dbImages.PostMultiFileAsync(fileDetails);
                _response.Result = fileDetails;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        //[Route("CreateUserProfilePic")]
        //[HttpPost]
        //public async Task<ActionResult<APIResponse>> PostImage([FromForm] Image model)
        //{
        //    try
        //    {
        //        if (model.File.Length > 0)
        //        {
        //            string path1 = _hostEnvironment.WebRootPath + "\\Utility\\";
        //            if (!Directory.Exists(path1))
        //            {
        //                Directory.CreateDirectory(path1);
        //            }

        //            using (FileStream fileStream = System.IO.File.Create(path1 + model.File.FileName))
        //            {
        //                model.File.CopyTo(fileStream);
        //                fileStream.Flush();
        //                return Ok();
        //            }
        //        }
        //        //else
        //        //{
        //        //    return BadRequest();
        //        //}


        //        Image imageModel = _mapper.Map<Image>(model);

        //        await _dbImages.CreateAsync(model);
        //        _response.Result = model;
        //        _response.StatusCode = HttpStatusCode.Created;
        //    }
        //    catch (Exception ex)
        //    {

        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString()
        //                };
        //    }
        //    //return _response;
        //    return Ok();
        //}

        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult<APIResponse>> PostImages([FromForm] Image model, IFormFile file)
        //{
        //    try
        //    {

        //        Image testViewModel = new Image()
        //        {
        //            ImageUrl = model.ImageUrl,
        //            File = model.File,
        //        };
        //        var formContent = new MultipartFormDataContent();
        //        formContent.Add(new StringContent(testViewModel.ImageUrl), "ImageUrl");
        //        formContent.Add(new StreamContent(testViewModel.File.OpenReadStream()), "File", Path.GetFileName(testViewModel.File.FileName));
        //        var response = hc.PostAsync(url, formContent).Result;
        //        return Ok();

        //        var stream = new MemoryStream(model.File);
        //        var guid = Guid.NewGuid().ToString();
        //        var file = $"{guid}.jpg";
        //        var folder = "Utility/Images";
        //        var fullPath = $"{folder}/{file}";
        //        var imageFullPath = fullPath.Remove(0, 7);
        //        var response = FilesHelper.UploadPhoto(stream, folder, file);
        //        if (response)
        //        {
        //            model.ImageUrl = imageFullPath;
        //            var image = new AdVehiclePicture()
        //            {
        //                ImageUrl = model.ImageUrl,
        //                VehicleId = model.VehicleId,
        //            };

        //            await _dbImages.CreateAsync(model);
        //            _response.Result = model;
        //            _response.StatusCode = HttpStatusCode.Created;

        //            //return CreatedAtRoute("GetAdType", new { id = image.Id }, _response);
        //            return Ok(_response);

        //        }
        //            catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString()
        //            };
        //    }
        //    return _response;
        //}
    }
}
