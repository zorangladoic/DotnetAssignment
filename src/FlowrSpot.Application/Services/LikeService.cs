using Ardalis.Result;
using AutoMapper;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;

namespace FlowrSpot.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ISightingRepository _sightingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public LikeService(ISightingRepository sightingRepository,
            IUserRepository userRepository,
            ILikeRepository likeRepository,
            IMapper mapper)
        {
            _sightingRepository = sightingRepository ??
                throw new ArgumentNullException(nameof(sightingRepository));
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _likeRepository = likeRepository ??
                throw new ArgumentNullException(nameof(likeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<Result<LikeDto>> CreateLikeAsync(Guid sightingId, string loggedInUser)
        {
            // check is sigting exists
            var sighting = await _sightingRepository.GetSightingAsync(sightingId);
            if (sighting == null)
            {
                return Result<LikeDto>.Error();
            }

            var user = await _userRepository.GetUserAsync(loggedInUser);
            if (user == null)
            {
                return Result<LikeDto>.Error();
            }

            // check is sigting is already liked by the same user
            var existingLike = await _likeRepository.GetLikeByIdAsync(sighting.Id, user.Id);
            if (existingLike != null)
            {
                return Result<LikeDto>.Error();
            }

            var likeToCreate = new Like
            {
                SightingId = sighting.Id,
                UserId = user.Id
            };

            await _likeRepository.AddLikeAsync(likeToCreate);

            return Result<LikeDto>.Success(_mapper.Map<LikeDto>(likeToCreate));
        }

        public async Task<bool> DeleteLikeAsync(Guid sightingId, string loggedInUser)
        {
            var user = await _userRepository.GetUserAsync(loggedInUser);
            if (user == null)
            {
                return false;
            }

            var likeToDelete = await _likeRepository.GetLikeByIdAsync(sightingId, user.Id);
            if (likeToDelete == null)
            {
                return false;
            }

            await _likeRepository.DeleteLikeAsync(likeToDelete);
            return true;
        }
    }
}
