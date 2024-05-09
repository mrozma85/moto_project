using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Data;
using System.Net;
using Windows.System;

namespace Moto_API.Controllers
{
    [Route("api/UserRolesListAPI")]
    [ApiController]
    public class ListUserRolesAPIController : ControllerBase
    {
        private readonly IListUserRolesRepository _dbList;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MotoDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ListUserRolesAPIController(IListUserRolesRepository dbList, UserManager<ApplicationUser> userManager, IMapper mapper, MotoDbContext db)
        {
            _dbList = dbList;
            _mapper = mapper;
            this._response = new();
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetUserRolesList()
        {
            try
            {
                //IEnumerable<ApplicationUserRole> usersList = await _dbList.GetAllAsync(includeProperties: "User");

                var users = _userManager.Users.Include(u => u.UserRoles)
                                              .ThenInclude(ur => ur.Role).AsNoTracking();

                _response.Result = users;
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

        [HttpGet("{id:Guid}", Name = "GetUserRolesDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUserRole(string id)
        {
            try
            {
                var userId = await _userManager.FindByIdAsync(new Guid(id).ToString());
                var roles = await _userManager.GetRolesAsync(userId);

                if (id == null)
                {
                    return BadRequest();
                }
                var userDetails = await _dbList.GetAsync(u => u.UserId == id, includeProperties: "User");
                if (userDetails == null)
                {
                    return NotFound();
                }

                Roles rol = new()
                {
                    User = userDetails.User,
                    RolesNames = roles
                };

                _response.Result = rol;
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

        [HttpDelete("{id:Guid}", Name = "DeleteUserRolesList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteUserRolesList(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var userRole = await _dbList.GetAsync(u => u.UserId == id);
                if (userRole == null)
                {
                    return NotFound();
                }
                await _dbList.RemoveAsync(userRole);
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

        [HttpPut("{id:Guid}", Name = "UpdateUserRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateUserRole(string id, string roleName/*, [FromBody] ApplicationUserRoleDTO applicationUserRoleDTO*/)
        {
            //var user = await _userManager.FindByIdAsync(new Guid(applicationUserRoleDTO.UserId).ToString());
            //var roles = await _userManager.GetRolesAsync(user);
            try
            {
                //if (applicationUserRoleDTO == null || userId != applicationUserRoleDTO.UserId)
                //{
                //    return BadRequest();
                //}

                //ApplicationUserRole model = _mapper.Map<ApplicationUserRole>(applicationUserRoleDTO);

                await _dbList.Update1111Async(id, roleName/*, model*/);
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
                wb.AddWorksheet(getData, "ListUserRoles Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListUserRoles.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "ListUserRolesData";
            dt.Columns.Add("UserId", typeof(string));
            dt.Columns.Add("RoleId", typeof(string));

            var list = _db.UserRoles.ToList();
            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    dt.Rows.Add(item.UserId, item.RoleId);
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
                ColumnWidths = "50% 50%",
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
                    FileDownloadName="ListUserRoles.pdf"
                };
            }
        }

    }
}
