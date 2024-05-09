using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Moto_API.Controllers
{
    [Route("api/VehiclesAPI")]
    [ApiController]
    public class VehiclesAPIController : ControllerBase
    {
        //ZAMIAST STANDARDU dbcontext
        //private readonly MotoDbContext _db;
        // WSKAKUJEMY TO BAZY PRZEZ IREPOSITORY = 
        private readonly IVehicleRepository _dbVehicle;
		private readonly IAdRepository _dbAd;
		private readonly IMapper _mapper;
        protected APIResponse _response;
        public VehiclesAPIController(IVehicleRepository dbVehicle, IAdRepository dbAd, IMapper mapper)
        {
            _dbVehicle= dbVehicle;
			_dbAd= dbAd;
			_mapper = mapper;
            this._response = new(); 
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetVehicles()
        public async Task<ActionResult<APIResponse>> GetVehicles()
        {
            //return Ok(await _db.Vehicles.ToListAsync());

            try { 
            //Automapper
            //IEnumerable<Vehicle> vehicleList = await _db.Vehicles.ToListAsync(); //przez BAZE standard
            IEnumerable<Vehicle> vehicleList = await _dbVehicle.GetAllAsync(); // przez IRepository

            //API 
            _response.Result = _mapper.Map<List<VehicleDTO>>(vehicleList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

            // return przed APIReposne
            //return Ok(_mapper.Map<List<VehicleDTO>>(vehicleList)); // konwetujemy do VehicleDto nasza liste
        }

        [HttpGet("{id:int}", Name = "GetVehicle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVehicle(int id) 
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }
                //var vehicle = await _db.Vehicles.FirstOrDefaultAsync(u => u.Id == id); // przez Baze standard
                var vehicle = await _dbVehicle.GetAsync(u => u.Id == id); // przez IRepository :)
                if (vehicle == null) 
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<VehicleDTO>(vehicle);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

                //return Ok(_mapper.Map<VehicleDTO>(vehicle));
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
        public async Task<ActionResult<APIResponse>> CreateVehicle([FromBody]VehicleDTO vehicleDTO)
        {
            try
            {
                // jak pojazd istnieje o takiej samej nazwie !!!!!
                // dodac await i ASYNC
                //if(await _dbVehicles.GetAsybc(u => u.Title.ToLower() == vehicleDTO.Title.ToLower()) != null)
                //{
                //    ModelState.AddModelError("", "Vehicle already exist");
                //}

                if (vehicleDTO == null)
                {
                    return BadRequest(vehicleDTO);
                }
                if (vehicleDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                // AUTOMAPER

                //Vehicle model = new()
                //{
                //    Title = vehicleDTO.Title,
                //    Description = vehicleDTO.Description,
                //    Price = vehicleDTO.Price,
                //    Model = vehicleDTO.Model,
                //    Year = vehicleDTO.Year,
                //    Engine = vehicleDTO.Engine,
                //    Color = vehicleDTO.Color,
                //    Company = vehicleDTO.Company,
                //    Location = vehicleDTO.Location,
                //    Condition = vehicleDTO.Condition
                //};
                // ZAMIAST POWYZEJ jedna linia _mapper
                //
                Vehicle model = _mapper.Map<Vehicle>(vehicleDTO);
                //

                await _dbVehicle.CreateAsync(model);
                _response.Result = _mapper.Map<VehicleDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVehicle", new { id = model.Id }, _response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
            };
        }
        return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVehicle")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVehicle(int id) 
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }
                var vehicle = await _dbVehicle.GetAsync(u => u.Id == id);
                if(vehicle == null)
                {
                    return NotFound();
                }
                await _dbVehicle.RemoveAsync(vehicle);
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

        [HttpPut("{id:int}", Name = "UpdateVehicle")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVehicle(int id, [FromBody]VehicleDTO vehicleDTO)
        {
            try
            {
                if(vehicleDTO == null || id != vehicleDTO.Id)
                {
                    return BadRequest();
                }

                //Vehicle model = new()
                //{
                //    Id = vehicleDTO.Id,
                //    Title = vehicleDTO.Title,
                //    Description = vehicleDTO.Description,
                //    Price = vehicleDTO.Price,
                //    Model = vehicleDTO.Model,
                //    Year = vehicleDTO.Year,
                //    Engine = vehicleDTO.Engine,
                //    Color = vehicleDTO.Color,
                //    Company = vehicleDTO.Company,
                //    Location = vehicleDTO.Location,
                //    Condition = vehicleDTO.Condition
                //};

                            // ZAMIAST POWYZEJ

                Vehicle model = _mapper.Map<Vehicle>(vehicleDTO);

                await _dbVehicle.UpdateAsync(model);
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
