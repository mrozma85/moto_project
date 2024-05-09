//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moto_API.Data;
//using Moto_API.Models;
//using Moto_API.Models.Dto;
//using Moto_API.Repository.IRepository;
//using System.Net;
//using System.Runtime.InteropServices;

//namespace Moto_API.Controllers
//{
//    [Route("api/TEST_API")]
//    [ApiController]
//    public class TEST_APIController : ControllerBase
//    {
//        private readonly MotoDbContext _db;
//        //private readonly IListUsersRepository _dbList;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IMapper _mapper;
//        protected APIResponse _response;

//        public TEST_APIController(MotoDbContext db, IListUsersRepository dbList, IMapper mapper, 
//                    RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
//        {
//            //_dbList = dbList;
//            _db = db;
//            _mapper = mapper;
//            this._response = new();
//            _roleManager = roleManager;
//            _userManager = userManager;
//        }

//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult<APIResponse>> GetUsersList()
//        {
//            try
//            {
//                IEnumerable<ApplicationUser> usersList = await _db.ApplicationUsers.GetAllAsync(includeProperties: "ApplicationUserRoles");
//                //IEnumerable<ApplicationUser> usersList = await _db.Users.Include(u => u.UserRoles).ToListAsync();
//                //var usersList = _db.ApplicationUserRoles.Include(c => c.User).Include(c => c.Role).ToArrayAsync();

//                //IEnumerable<ApplicationUser> usersList = await _db.Users.Include(u => u.UserRoles).ThenInclude(x => x.Role).ToListAsync();
//                //var roles = _db.Roles.Include(r => r.Users).Where

//                //ToListAsync();
//                //GetAllAsync(includeProperties: "UserRoles");

//                //var roles = _roleManager.Roles.ToList();
//                //var userRoles = _userManager.UsersRoles.ToList();

//                //IEnumerable<ApplicationUser> usersList = await _db.ApplicationUsers.Include(x => x.UserRoles).ThenInclude(x => x.Role).ToListAsync(); // .GetAllAsync(includeProperties: "UserRoles");

//                //var model = _db.ApplicationUsers.Include(x => x.UserRoles).ThenInclude(x => x.Role).ToListAsync();
//                //IEnumerable<ApplicationUser> usersList = await _dbList.UpdateAsync1();

//                //if (!string.IsNullOrEmpty(idUser))
//                //{
//                //    usersList = await _dbList.GetAllAsync(u => u.Id == idUser);
//                //}
//                //else
//                //{
//                //    BadRequest();
//                //}

//                //

//                _response.Result = usersList;
//                _response.StatusCode = HttpStatusCode.OK;
//                return Ok(_response);
//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.ErrorMessages = new List<string>() { ex.ToString() };
//            }
//            return _response;
//        }

//        //[HttpGet("{id:Guid}", Name = "GetUserDetails")]
//        //[ProducesResponseType(StatusCodes.Status200OK)]
//        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
//        //[ProducesResponseType(StatusCodes.Status404NotFound)]
//        //public async Task<ActionResult<APIResponse>> GetUser(string id)
//        //{
//        //    try
//        //    {
//        //        if (id == null)
//        //        {
//        //            return BadRequest();
//        //        }
//        //        var userDetails = await _dbList.GetAsync(u => u.Id == id, includeProperties: "UserRoles");
//        //        if (userDetails == null)
//        //        {
//        //            return NotFound();
//        //        }
//        //        _response.Result = userDetails;
//        //        _response.StatusCode = HttpStatusCode.OK;
//        //        return Ok(_response);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _response.IsSuccess = false;
//        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
//        //    }
//        //    return _response;
//        //}

//        //[HttpPost]
//        //[ProducesResponseType(StatusCodes.Status200OK)]
//        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
//        //[ProducesResponseType(StatusCodes.Status404NotFound)]
//        //public async Task<ActionResult<APIResponse>> CreateUser([FromBody] ApplicationUser adTypeDTO)
//        //{
//        //    try
//        //    {
//        //        if (adTypeDTO == null)
//        //        {
//        //            return BadRequest(adTypeDTO);
//        //        }
//        //        //if (adTypeDTO.Id > 0)
//        //        //{
//        //        //    return StatusCode(StatusCodes.Status500InternalServerError);
//        //        //}

//        //        //AdType model = _mapper.Map<AdType>(adTypeDTO);

//        //        await _dbList.CreateAsync(adTypeDTO);
//        //        _response.Result = _mapper.Map<ApplicationUser>(adTypeDTO);
//        //        _response.StatusCode = HttpStatusCode.Created;

//        //        return CreatedAtRoute("GetAdType", new { id = adTypeDTO.Id }, _response);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _response.IsSuccess = false;
//        //        _response.ErrorMessages = new List<string>() { ex.ToString()
//        //    };
//        //    }
//        //    return _response;
//        //}

//        //[HttpDelete("{id:Guid}", Name = "DeleteUser")]
//        //[ProducesResponseType(StatusCodes.Status204NoContent)]
//        //[ProducesResponseType(StatusCodes.Status404NotFound)]
//        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
//        //public async Task<ActionResult<APIResponse>> DeleteAdType(string id)
//        //{
//        //    try
//        //    {
//        //        if (id == null)
//        //        {
//        //            return BadRequest();
//        //        }
//        //        var adType = await _dbList.GetAsync(u => u.Id == id);
//        //        if (adType == null)
//        //        {
//        //            return NotFound();
//        //        }
//        //        await _dbList.RemoveAsync(adType);
//        //        _response.StatusCode = HttpStatusCode.NoContent;
//        //        _response.IsSuccess = true;
//        //        return Ok(_response);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _response.IsSuccess = false;
//        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
//        //    }
//        //    return _response;
//        //}

//        //[HttpPut("{id:Guid}", Name = "UpdateUser")]
//        //[ProducesResponseType(StatusCodes.Status204NoContent)]
//        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
//        //public async Task<ActionResult<APIResponse>> UpdateUser(string id, [FromBody] ApplicationUser applicationUser)
//        //{
//        //    try
//        //    {
//        //         if (applicationUser == null || id != applicationUser.Id)
//        //        {
//        //            return BadRequest();
//        //        }

//        //        await _dbList.UpdateAsync(applicationUser);
//        //        _response.StatusCode = HttpStatusCode.NoContent;
//        //        _response.IsSuccess = true;
//        //        return Ok(_response);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _response.IsSuccess = false;
//        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
//        //    }
//        //    return _response;
//        //}

//    }
//}
