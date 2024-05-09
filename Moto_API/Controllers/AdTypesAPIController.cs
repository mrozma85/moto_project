using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Data;
using System.Net;

namespace Moto_API.Controllers
{
    [Route("api/AdTypesAPI")]
    [ApiController]
    public class AdTypesAPIController : ControllerBase
    {
        private readonly IAdTypeRepository _dbAdType;
        private readonly MotoDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public AdTypesAPIController(IAdTypeRepository dbAdType, IMapper mapper, MotoDbContext db)
        {
            _dbAdType = dbAdType;
            _mapper = mapper;
            this._response = new();
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAdTypes()
        {
            try
            {
                IEnumerable<AdType> adTypesList = await _dbAdType.GetAllAsync(); // przez IRepository

                //API 
                _response.Result = _mapper.Map<List<AdTypeDTO>>(adTypesList);
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

        [HttpGet("{id:int}", Name = "GetAdType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAdType(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var adTypes = await _dbAdType.GetAsync(u => u.Id == id);
                if (adTypes == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<AdTypeDTO>(adTypes);
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
        public async Task<ActionResult<APIResponse>> CreateAdTypes([FromBody] AdTypeDTO adTypeDTO)
        {
            try
            {
                if (adTypeDTO == null)
                {
                    return BadRequest(adTypeDTO);
                }
                if (adTypeDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                AdType model = _mapper.Map<AdType>(adTypeDTO);

                await _dbAdType.CreateAsync(model);
                _response.Result = _mapper.Map<AdTypeDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetAdType", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
            };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteAdType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteAdType(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var adType = await _dbAdType.GetAsync(u => u.Id == id);
                if (adType == null)
                {
                    return NotFound();
                }
                await _dbAdType.RemoveAsync(adType);
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

        [HttpPut("{id:int}", Name = "UpdateAdType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateAdType(int id, [FromBody] AdTypeDTO adTypeDTO)
        {
            try
            {
                if (adTypeDTO == null || id != adTypeDTO.Id)
                {
                    return BadRequest();
                }

                AdType model = _mapper.Map<AdType>(adTypeDTO);

                await _dbAdType.UpdateAsync(model);
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

        [HttpGet("ExportExcel")]
        public ActionResult ExportExcel()
        {
            var getData = GetData();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(getData, "AdTypes Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdTypes.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "AdTypessData";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nazwa", typeof(string));
            dt.Columns.Add("Data utworzenia", typeof(DateTime));

            var list = _db.AdTypes.ToList();
            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.Name, item.CreatedDate);
                });
            }
            return dt;
        }

    }
}
