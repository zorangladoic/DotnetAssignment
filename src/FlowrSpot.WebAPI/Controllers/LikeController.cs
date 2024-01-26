using AutoMapper;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Application.Services;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;
using FlowrSpot.Infrastructure.Repositories;
using FlowrSpot.WebAPI.Authentication.Basic.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowrSpot.WebAPI.Controllers
{
    [ApiController]
    [BasicAuthorization]
    [Route("api/v1/like")]
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService ??
                throw new ArgumentNullException(nameof(likeService));
        }

        /// <summary>
        /// CREATE like endpoint. Creates a like for a sighting.
        /// It refferences logged in user and provided sighting id
        /// </summary>
        /// <param name="like">CreateLikeRequest</param>
        /// <returns>LikeDto</returns>
        [HttpPost]
        public async Task<ActionResult> CreateLike(CreateLikeRequest like)
        {
            var loggedInUser = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(loggedInUser))
            {
                return BadRequest();
            }

            var result = await _likeService.CreateLikeAsync(like.SightingId, loggedInUser);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Created(string.Empty, result.Value);
        }

        /// <summary>
        /// DELETE like endpoint. Only user who created like can delete it.
        /// </summary>
        /// <param name="like">DeleteLikeRequest</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteLike([FromBody] DeleteLikeRequest like)
        {
            // Get logged user
            var loggedInUser = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(loggedInUser))
            {
                return BadRequest();
            }

            if (await _likeService.DeleteLikeAsync(like.SightingId, loggedInUser))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
