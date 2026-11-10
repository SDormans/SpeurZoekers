using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using Speurzoekers.Common.Business;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using Speurzoekers.Service.Hubs;
using Speurzoekers.Service.Hubs.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Configuration/Admin")]
    [ApiController]
    public class ConfigurationAdminController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<CollectibleHub, ICollectibleClient> _collectibleHub;
        private readonly IMapGenerator _mapGenerator;

        public ConfigurationAdminController(
            IConfigurationRepository configurationRepository,
            IRegionRepository regionRepository,
            IMapper mapper,
            IHubContext<CollectibleHub, ICollectibleClient> collectibleHub,
            IMapGenerator mapGenerator)
        {
            _configurationRepository = configurationRepository;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _collectibleHub = collectibleHub;
            _mapGenerator = mapGenerator;
        }

        [HttpPost("map/refresh")]
        public async Task<IActionResult> RefreshMapNow()
        {
            return await RefreshMap(DateTime.Now);
        }

        [HttpPost("map/refresh/{dateTime}")]
        public async Task<IActionResult> RefreshMap([FromRoute]DateTime dateTime)
        {
            try
            {
                await _mapGenerator.RefreshMap(dateTime);
                await _collectibleHub.Clients.All.RefreshMapAlert();

                return Ok();
            }
            catch (SynchronizationLockException)
            {
                return new StatusCodeResult(408); // Request Timeout
            }
        }

        [HttpGet("map/refreshrate")]
        public async Task<IActionResult> GetMapRefreshRate()
        {
            return Ok(new { RefreshRate = await _configurationRepository.GetMapRefreshRate() });
        }

        [HttpPost("map/refreshrate")]
        public async Task<IActionResult> SetMapRefreshRate(int? minutes)
        {
            await _configurationRepository.SetMapRefreshRate(minutes);

            return Ok();
        }

        [HttpGet("map/starttime")]
        public async Task<IActionResult> GetMapRefreshStart()
        {
            return Ok(new { StartTime = await _configurationRepository.GetMapRefreshStart() });
        }

        [HttpPost("map/starttime")]
        public async Task<IActionResult> SetMapRefreshStart(int? minutes)
        {
            await _configurationRepository.SetMapRefreshStart(minutes);

            return Ok();
        }

        [HttpPost("catchRange")]
        public async Task<IActionResult> SetCatchRange(float catchRange)
        {
            await _configurationRepository.SetCatchRange(catchRange);
            await _collectibleHub.Clients.All.CatchRangeToClient(catchRange);
            return Ok();
        }

        [HttpPost("spotrange")]
        public async Task<IActionResult> SetSpottingRange(float? range)
        {
            await _configurationRepository.SetSpottingRange(range);

            return Ok();
        }

        [HttpPost("avatarPrice")]
        public async Task<IActionResult> SetAvatarPrice(int? points)
        {
            await _configurationRepository.SetAvatarPrice(points);

            return Ok();
        }
        
        [HttpGet("regions")]
        public async Task<IActionResult> GetRegions()
        {
            Region[] regions = await _regionRepository.GetRegions();
            return Ok(_mapper.Map<Region[], RegionDto[]>(regions));
        }

        [HttpGet("region/{name}")]
        public async Task<IActionResult> GetRegion([FromRoute]string name)
        {
            Region region = await _regionRepository.GetRegion(name);

            if (region == null)
            {
                return NotFound();
            }
            //else
            return Ok(_mapper.Map<RegionDto>(region));
        }

        [HttpPost("region")]
        public async Task<IActionResult> AddRegion(RegionDto regionDto)
        {
            Region region = _mapper.Map<Region>(regionDto);

            if (!region.PolygonVertices.Any())
            {
                region.PolygonVertices = new Coordinate(region.Longitude, region.Latitude)
                                            .GetRectangle(0.015f, 0.009f); // +/- 1 km2
            }

            await _regionRepository.AddRegion(region);

            return Ok(_mapper.Map<RegionDto>(region));
        }

        [HttpPost("region/remove/{name}")]
        public async Task<IActionResult> RemoveRegion([FromRoute]string name)
        {
            await _regionRepository.RemoveRegion(name);
            return Ok();
        }
    }
}
