using AutoMapper;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowrSpot.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterUserRequest> _validator;

        public UserController(IUserRepository userRepository,
            IMapper mapper,
            IValidator<RegisterUserRequest> validator)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _validator = validator ??
                throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// POST User endpoint, creates new User, doesn't require authentication
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            User userToCreate = _mapper.Map<User>(user);
            await _userRepository.AddUserAsync(userToCreate);
            var userToReturn = _mapper.Map<UserDto>(userToCreate);

            return Created(string.Empty, userToReturn);
        }
    }
}
