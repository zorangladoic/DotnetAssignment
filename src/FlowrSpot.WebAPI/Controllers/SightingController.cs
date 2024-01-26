using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Application.Services;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;
using FlowrSpot.WebAPI.Authentication.Basic.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowrSpot.WebAPI.Controllers
{
    [ApiController]
    [BasicAuthorization]
    [Route("api/v1/sighting")]
    public class SightingController : Controller
    {
        private readonly ISightingRepository _sightingRepository;
        private readonly ISightingService _sightingService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SightingController(ISightingRepository sightingRepository,
            ISightingService sightingService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _sightingRepository = sightingRepository ??
                throw new ArgumentNullException(nameof(sightingRepository));
            _sightingService = sightingService ??
                throw new ArgumentNullException(nameof(sightingService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// GET Sighting endpoint, retrieving of all Sightings, secured with autentication 
        /// </summary>
        /// <returns>List<SightingDto></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var sightings = await _sightingService.GetSightingsAsync();
            return Ok(sightings);
        }

        /// <summary>
        /// GET Sighting endpoint, retrieve Sighting by Id, secured with basic autentication 
        /// </summary>
        /// <returns>SightingDto</returns>
        [TranslateResultToActionResult]
        [HttpGet("{id}")]
        public async Task<Result<SightingDto>> GetSightingById(Guid id)
        {
            return await _sightingService.GetSightingAsync(id);
        }

        /// <summary>
        /// DELETE Sighting endpoint, removes Sighting, secured with basic autentication  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSighting(Guid id)
        {
            var loggedInUser = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(loggedInUser))
            {
                return BadRequest();
            }

            var sighting = await _sightingRepository.GetSightingAsync(id);
            if (sighting == null)
            {
                return NotFound();
            }

            if (await _sightingService.IsSightingCreatedByUser(sighting, loggedInUser))
            {
                await _sightingService.DeleteSightingAsync(sighting);
                return NoContent(); 
            }

            return Forbid();
        }

        /// <summary>
        /// POST Sighting endpoint, creates new Sighting, secured with basic autentication 
        /// </summary>
        /// <param name="sighting"></param>
        /// <returns>SightingDto</returns>
        [HttpPost]
        public async Task<ActionResult> CreateSighting([FromBody] CreateSightingRequest sighting)
        {
            Sighting sightingToCreate = _mapper.Map<Sighting>(sighting);

            var loggedInUser = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(loggedInUser))
            {
                return BadRequest();
            }

            var apiSecret = _configuration["TheySaidSoAPISecret"];
            if( string.IsNullOrEmpty(apiSecret))
            {
                return BadRequest();
            }

            var result = await _sightingService.CreateSightingAsync(sightingToCreate, loggedInUser, apiSecret);
            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Created(string.Empty, result.Value);
        }
    }
}
