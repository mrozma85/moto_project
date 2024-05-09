using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;

namespace Moto_API.Controllers
{
    [Route("api/UsersListAPI")]
    [ApiController]
    public class ListUsersAPIController : ControllerBase
    {
        private readonly IListUsersRepository _dbList;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MotoDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ListUsersAPIController(IListUsersRepository dbList, UserManager<ApplicationUser> userManager, IMapper mapper, MotoDbContext db)
        {
            _dbList = dbList;
            _mapper = mapper;
            this._response = new();
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetUsersList()
        {
            try
            {
                //IEnumerable<ApplicationUser> usersList = await _dbList.GetAllAsync(includeProperties: "UserRoles");
                
                var users = _userManager.Users.Include(u => u.UserRoles)
                                              .ThenInclude(ur => ur.Role).AsNoTracking();

                IEnumerable<ApplicationUser> userList = users;

                _response.Result = userList;
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

        [HttpGet("{id:Guid}", Name = "GetUserDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var userDetails = await _dbList.GetAsync(u => u.Id == id, includeProperties: "UserRoles");

                var userDetails1 = await _dbList.GetById(id);

                var users = _userManager.Users.Where(u => u.Id == id)
                                              .Include(u => u.UserRoles)
                                              .ThenInclude(ur => ur.Role).AsNoTracking();

                //IEnumerable<ApplicationUser> userList = users;

                if (users == null)
                {
                    return NotFound();
                }
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

        [HttpDelete("{id:Guid}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteUser(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var adType = await _dbList.GetAsync(u => u.Id == id);
                if (adType == null)
                {
                    return NotFound();
                }
                await _dbList.RemoveAsync(adType);
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

        [HttpPut("{id:Guid}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateUser(string id, string roleName, [FromBody]List<ApplicationUser> applicationUser)
        {
            try
            {
                if (applicationUser == null || id != applicationUser[0].Id)
                {
                    return BadRequest();
                }

                List<ApplicationUser> model = applicationUser;

                await _dbList.UpdateAsync(roleName, model);
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
                wb.AddWorksheet(getData, "ListUser Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListUser.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "ListUserData";
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            var list = _db.Users.ToList();
            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.Name, item.Email);
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
                ColumnWidths = "30% 30% 30%",
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
                    FileDownloadName="ListUser.pdf"
                };
            }
        }

    }
}
