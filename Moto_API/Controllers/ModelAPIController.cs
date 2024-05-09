using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto.Category;
using Moto_API.Repository.IRepository;
using System.Data;
using System.Net;

namespace Moto_API.Controllers
{
    [Route("api/ModelAPI")]
    [ApiController]
    public class ModelAPIController : ControllerBase
    {
        private readonly IModelRepository _dbModel;
		private readonly MotoDbContext _db;
		private readonly IMapper _mapper;
        protected APIResponse _response;

        public ModelAPIController(IModelRepository dbModel, IMapper mapper, MotoDbContext db)
        {
            _dbModel= dbModel;
            _mapper = mapper;
            this._response = new();
			_db = db;
		}

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetModels()
        {
            try
            {
                IEnumerable<Model> modelList = await _dbModel.GetAllAsync();

                //API 
                _response.Result = modelList;
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

        [HttpGet("{id:int}", Name = "GetModel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetModel(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var model = await _dbModel.GetAsync(u => u.Id == id);
                if (model == null)
                {
                    return NotFound();
                }
                _response.Result = model;
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
        public async Task<ActionResult<APIResponse>> CreateModel([FromBody] Model model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(model);
                }
                if (model.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Model modelList = model;

                await _dbModel.CreateAsync(modelList);
                _response.Result = modelList;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetModel", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
            };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteModel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteModel(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var model = await _dbModel.GetAsync(u => u.Id == id);
                if (model == null)
                {
                    return NotFound();
                }
                await _dbModel.RemoveAsync(model);
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

        [HttpPut("{id:int}", Name = "UpdateModel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateModel(int id, [FromBody] Model model)
        {
            try
            {
                if (model == null || id != model.Id)
                {
                    return BadRequest();
                }

                Model modelList = model;

                await _dbModel.UpdateAsync(modelList);
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
				wb.AddWorksheet(getData, "Categories Records");
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
			dt.TableName = "CategoriesData";
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Nazwa", typeof(string));
			dt.Columns.Add("Data utworzenia", typeof(DateTime));
			dt.Columns.Add("Data aktualizacji", typeof(DateTime));

			var list = _db.Categories.ToList();
			if (list.Count > 0)
			{
				list.ForEach(item =>
				{
					dt.Rows.Add(item.Id, item.Name, item.CreatedDate, item.UpdatedDate);
				});
			}
			return dt;
		}

		[HttpGet("ExportPDF")]
		public ActionResult ExportPDF()
		{
			var getData = GetData();
			var document = new Document
			{
				PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
			};
			var pdfpage = document.Pages.Add();
			Table table = new Table
			{
				ColumnWidths = "10% 25% 25% 25%",
				DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
				Border = new BorderInfo(BorderSide.All, .2f, Color.Black),
			};
			table.ImportDataTable(getData, true, 0, 0);
			document.Pages[1].Paragraphs.Add(table);

			using (var streamout = new MemoryStream())
			{
				document.Save(streamout);
				return new FileContentResult(streamout.ToArray(), "application/pdf")
				{
					FileDownloadName="Categories.pdf"
				};
			}
		}

	}
}
