using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;
using System.Net;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainPageDetailsController : ControllerBase
    {
        private readonly IMainPageDetailsRepository _dbMain;
        private readonly MotoDbContext _db;
        protected APIResponse _response;

        public MainPageDetailsController(IMainPageDetailsRepository dbMain, MotoDbContext db)
        {
            _dbMain= dbMain;
            this._response = new();
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMains()
        {
            try
            {
                IEnumerable<MainPageDetials> list = await _dbMain.GetAllAsync();

                _response.Result = list;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateMain([FromBody] MainPageDetials main)
        {
            try
            {
                if (main == null)
                {
                    return BadRequest(main);
                }
                if (main.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                MainPageDetials model = main;

                await _dbMain.CreateAsync(model);
                _response.Result = model;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCategory", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
            };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateMain")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateMain(int id, [FromBody] MainPageDetials main)
        {
            try
            {
                if (main == null || id != main.Id)
                {
                    return BadRequest();
                }

                MainPageDetials model = main;

                await _dbMain.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
