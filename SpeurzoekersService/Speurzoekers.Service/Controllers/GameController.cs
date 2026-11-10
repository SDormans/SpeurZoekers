using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using Speurzoekers.Service.Dto.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : SpeurzoekersBaseController
    {
        private readonly IUserCoordinatesRepository _userCoordinatesRepository;
        private readonly ICollectibleRepository _collectibleRepository;
        private readonly IMapper _mapper;

        public GameController(
            IUserCoordinatesRepository userCoordinatesRepository, 
            ICollectibleRepository collectibleRepository,
            IMapper mapper
            )
        {
            _userCoordinatesRepository = userCoordinatesRepository;
            _collectibleRepository = collectibleRepository;
            _mapper = mapper;
        }

        [HttpPost("Location")]
        public async Task<IActionResult> Location(LocationDto location)
        {
            var userId = GetUserId();
            await _userCoordinatesRepository.SaveCoordinatesAsync(userId, location.Longitude, location.Latitude);
            return Ok();
        }

        [HttpPost("map")]
        public async Task<IActionResult> GetCollectiblePositions(LocationDto location)
        {
            List<CollectiblePosition> list = await _collectibleRepository
                        .GetCollectiblePositions(location.Latitude, location.Longitude);

            return Ok(_mapper.Map<List<CollectiblePosition>, List<CollectiblePositionDto>>(list));
        }
    }
}