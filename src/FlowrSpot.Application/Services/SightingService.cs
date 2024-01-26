using Ardalis.Result;
using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;
using FlowrSpot.Quote.Entities;
using Newtonsoft.Json;

namespace FlowrSpot.Application.Services
{
    public class SightingService : ISightingService
    {
        private readonly ISightingRepository _sightingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFlowerRepository _flowerRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly HttpClient httpClient = new();
        public SightingService(ISightingRepository sightingRepository,
            IUserRepository userRepository,
            IFlowerRepository flowerRepository,
            ILikeRepository likeRepository)
        {
            _sightingRepository = sightingRepository ??
                throw new ArgumentNullException(nameof(sightingRepository));
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _flowerRepository = flowerRepository ??
                throw new ArgumentNullException(nameof(flowerRepository));
            _likeRepository = likeRepository ??
                throw new ArgumentNullException(nameof(likeRepository));
        }

        public async Task<Result<CreateSightingDto>> CreateSightingAsync(Sighting sighting, string username, string apiSercret)
        {
            var flower = await _flowerRepository.GetFlowerAsync(sighting.FlowerId);
            if (flower == null)
            {
                return Result<CreateSightingDto>.Error();
            }

            var user = await _userRepository.GetUserAsync(username);
            if (user == null)
            {
                return Result<CreateSightingDto>.Error();
            }
            sighting.UserId = user.Id;

            var quote = await GetRandomQuoteFromApiAsync(apiSercret);
            if ((string.IsNullOrEmpty(quote)))
            {
                return Result<CreateSightingDto>.Error();
            }

            await _sightingRepository.AddSightingAsync(sighting);

            return Result<CreateSightingDto>.Success(
                new CreateSightingDto
                {
                    Id = sighting.Id,
                    Longitude = sighting.Longitude,
                    Latitude = sighting.Latitude,
                    UserId = sighting.UserId,
                    FlowerId = sighting.FlowerId,
                    ImageUrl = flower.ImageUrl,
                    LikeCounter = 0,
                    Quote = quote
                });
        }

        public async Task DeleteSightingAsync(Sighting sighting)
        {
            await _sightingRepository.DeleteSightingAsync(sighting);
            var likesToDelete = await _likeRepository.GetLikesBySightingIdAsync(sighting.Id);
            foreach (var likeToDelete in likesToDelete)
            {
                await _likeRepository.DeleteLikeAsync(likeToDelete);
            }
        }

        public async Task<Result<SightingDto>> GetSightingAsync(Guid id)
        {
            var sighting = await _sightingRepository.GetSightingAsync(id);
            if (sighting == null)
            {
                return Result<SightingDto>.NotFound();
            }

            var flower = await _flowerRepository.GetFlowerAsync(sighting.FlowerId);
            if (flower == null)
            {
                return Result<SightingDto>.NotFound();
            }

            int likesCounter = await _likeRepository.GetSightingLikeCounterAsync(sighting.Id);

            return Result<SightingDto>.Success(
                new SightingDto
                {
                    Id = sighting.Id,
                    Latitude = sighting.Latitude,
                    Longitude = sighting.Longitude,
                    UserId = sighting.UserId,
                    FlowerId = sighting.FlowerId,
                    ImageUrl = flower.ImageUrl,
                    LikeCounter = likesCounter
                });
        }

        public async Task<IEnumerable<SightingDto>> GetSightingsAsync()
        {
            var sightings = await _sightingRepository.GetSightingsAsync();

            List<SightingDto> sigtingsToReturn = [];

            foreach (var sighting in sightings)
            {
                var flower = await _flowerRepository.GetFlowerAsync(sighting.FlowerId);
                if (flower == null)
                {
                    return new List<SightingDto>();
                }
                int likesCounter = await _likeRepository.GetSightingLikeCounterAsync(sighting.Id);
                sigtingsToReturn.Add(new SightingDto
                {
                    Id = sighting.Id,
                    Latitude = sighting.Latitude,
                    Longitude = sighting.Longitude,
                    UserId = sighting.UserId,
                    FlowerId = sighting.FlowerId,
                    ImageUrl = flower.ImageUrl,
                    LikeCounter = likesCounter
                });
            }

            return sigtingsToReturn;
        }

        public async Task<bool> IsSightingCreatedByUser(Sighting sighting, string username)
        {
            var user = await _userRepository.GetUserAsync(username);
            if ((user == null) ||
                (sighting.UserId != user.Id))
            {
                return false;
            }
            return true;
        }

        #region Helper Methods
        private async Task<string> GetRandomQuoteFromApiAsync(string apiSecret)
        {
            try
            {
                var req = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"http://quotes.rest/qod.json?api_key={apiSecret}"),
                    Method = HttpMethod.Get,
                };

                HttpResponseMessage response = await httpClient.SendAsync(req);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseBody))
                {
                    return string.Empty;
                }

                QuoteViewModel quoteResponse = JsonConvert.DeserializeObject<QuoteViewModel>(responseBody);
                if (quoteResponse == null)
                {
                    return string.Empty;
                }

                var quote = quoteResponse.Contents.Quotes.First().QuoteMsg;
                quote += " " + quoteResponse.Contents.Quotes.First().Author;

                return quote;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
