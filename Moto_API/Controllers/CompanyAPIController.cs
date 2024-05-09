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
    [Route("api/CompanyAPI")]
    [ApiController]
    public class CompanyAPIController : ControllerBase
    {
        private readonly ICompanyRepository _dbCompany;
		private readonly MotoDbContext _db;
		private readonly IMapper _mapper;
        protected APIResponse _response;

        public CompanyAPIController(ICompanyRepository dbCompany, IMapper mapper, MotoDbContext db)
        {
            _dbCompany= dbCompany;
            this._response = new();
			_db = db;
		}

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCompanies()
        {
            try
            {
                IEnumerable<Company> companyList = await _dbCompany.GetAllAsync();

                var list = _db.Companies.ToList();
                //API 
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

        [HttpGet("{id:int}", Name = "GetCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCompany(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var company = await _dbCompany.GetAsync(u => u.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
                _response.Result = company;
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
        public async Task<ActionResult<APIResponse>> CreateCompany([FromBody] Company company)
        {
            try
            {
                if (company == null)
                {
                    return BadRequest(company);
                }
                if (company.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Company model = company;

                await _dbCompany.CreateAsync(model);
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

        [HttpDelete("{id:int}", Name = "DeleteCompany")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCompany(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var company = await _dbCompany.GetAsync(u => u.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
                await _dbCompany.RemoveAsync(company);
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

        [HttpPut("{id:int}", Name = "UpdateCompany")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCompany(int id, [FromBody] Company company)
        {
            try
            {
                if (company == null || id != company.Id)
                {
                    return BadRequest();
                }

                Company model = company;

                await _dbCompany.UpdateAsync(model);
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
