using AutoMapper;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;
using FlowrSpot.WebAPI.Authentication.Basic.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowrSpot.WebAPI.Controllers
{
    [ApiController]
    [BasicAuthorization]
    [Route("api/v1/flower")]
    public class FlowerController : Controller
    {
        private readonly IFlowerRepository _flowerRepository;
        private readonly IMapper _mapper;

        public FlowerController(IFlowerRepository flowerRepository,
            IMapper mapper)
        {
            _flowerRepository = flowerRepository ??
                throw new ArgumentNullException(nameof(flowerRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// GET Flowers endpoint, retrieving of all flowers, doesn't require authentication
        /// </summary>
        /// <returns>List<FlowersDTO></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var flowers = await _flowerRepository.GetFlowersAsync();
            return Ok(_mapper.Map<List<FlowerDto>>(flowers));
        }

        /// <summary>
        /// GET Flower endpoint, retrieve Flower by Id, secured with basic autentication 
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>FlowerDto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFlowerById(Guid id)
        {
            var flower = await _flowerRepository.GetFlowerAsync(id);

            if (flower == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlowerDto>(flower));
        }

        /// <summary>
        /// POST Flower endpoint, creates new Flower, secured with basic autentication 
        /// </summary>
        /// <param name="flower">CreateFlowerRequest</param>
        /// <returns>FlowerDto</returns>
        [HttpPost]
        public async Task<ActionResult> CreateFlower([FromBody] CreateFlowerRequest flower)
        {
            Flower flowerToCreate = _mapper.Map<Flower>(flower);

            await _flowerRepository.AddFlowerAsync(flowerToCreate);

            var flowerToReturn = _mapper.Map<FlowerDto>(flowerToCreate);

            return Created(string.Empty, flowerToReturn);
        }
    }
}
