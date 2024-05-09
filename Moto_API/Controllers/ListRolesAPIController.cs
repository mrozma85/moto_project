using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;
using System.Data;
using System.Net;

namespace Moto_API.Controllers
{
    [Route("api/RolesListAPI")]
    [ApiController]
    public class ListRolesAPIController : ControllerBase
    {
        private readonly IListRoleRepository _dbList;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly MotoDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ListRolesAPIController(IListRoleRepository dbList, IMapper mapper, MotoDbContext db, RoleManager<ApplicationRole> roleManager)
        {
            _dbList = dbList;
            _mapper = mapper;
            this._response = new();
            _db = db;
            _roleManager = roleManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetRolesList()
        {
            try
            {
                IEnumerable<IdentityRole> usersList = await _dbList.GetAllAsync(includeProperties: "UserRoles");
                
                var roles = _roleManager.Roles;

                _response.Result = roles;
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

        [HttpGet("{id:Guid}", Name = "GetRoleDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRole(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                //var role = await _dbList.GetAsync(u => u.Id == id); // => applicationtionRole
                var role = await _roleManager.FindByIdAsync(new Guid(id).ToString()); //identityRole

                if (role == null)
                {
                    return NotFound();
                }
                _response.Result = role;
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

        [HttpPut("{id:Guid}", Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateRole(string id, [FromBody] ApplicationRole roleDTO)
        {
            try
            {
                if (roleDTO == null || id != roleDTO.Id)
                {
                    return BadRequest();
                }

                //ApplicationRole model = _mapper.Map<ApplicationRole>(roleDTO);

                await _dbList.UpdateAsync(roleDTO);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateRole([FromBody] ApplicationRole applicationRole)
        {
            try
            {
                if (applicationRole == null)
                {
                    return BadRequest(applicationRole);
                }

                //await _dbList.CreateAsync(applicationRole);
                await _dbList.CreateNewTest(applicationRole);
                //_response.Result = _mapper.Map<ApplicationRole>(applicationRole);
                _response.Result = applicationRole;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetRoleDetail", new { id = applicationRole.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
            };
            }
            return _response;
        }

        [HttpDelete("{id:Guid}", Name = "DeleteRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteRole(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                //var role = await _dbList.GetAsync(u => u.Id == id);
                var role = await _roleManager.FindByIdAsync(new Guid(id).ToString());
                if (role == null)
                {
                    return NotFound();
                }
                await _dbList.RemoveAsync(role);
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
                wb.AddWorksheet(getData, "ListRoles Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListRoles.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "ListRolesData";
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("NormalizedName", typeof(string));
            dt.Columns.Add("ConcurrencyStamp", typeof(string));

            var list = _db.Roles.ToList();
            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.Name, item.NormalizedName, item.ConcurrencyStamp);
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
                    FileDownloadName="ListRoles.pdf"
                };
            }
        }
    }
}
