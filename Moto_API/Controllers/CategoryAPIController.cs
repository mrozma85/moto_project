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
    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryRepository _dbCategory;
		private readonly MotoDbContext _db;
		private readonly IMapper _mapper;
        protected APIResponse _response;

        public CategoryAPIController(ICategoryRepository dbCategory, IMapper mapper, MotoDbContext db)
        {
            _dbCategory= dbCategory;
            _mapper = mapper;
            this._response = new();
			_db = db;
		}

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            try
            {
                IEnumerable<Category> categoryList = await _dbCategory.GetAllAsync(); // przez IRepository

                //API 
                _response.Result = _mapper.Map<List<CategoryDTO>>(categoryList);
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

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var category = await _dbCategory.GetAsync(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<CategoryDTO>(category);
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
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                if (categoryCreateDTO == null)
                {
                    return BadRequest(categoryCreateDTO);
                }
                if (categoryCreateDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Category model = _mapper.Map<Category>(categoryCreateDTO);

                await _dbCategory.CreateAsync(model);
                _response.Result = _mapper.Map<CategoryDTO>(model);
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

        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var category = await _dbCategory.GetAsync(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                await _dbCategory.RemoveAsync(category);
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

        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                if (categoryUpdateDTO == null || id != categoryUpdateDTO.Id)
                {
                    return BadRequest();
                }

                Category model = _mapper.Map<Category>(categoryUpdateDTO);

                await _dbCategory.UpdateAsync(model);
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
