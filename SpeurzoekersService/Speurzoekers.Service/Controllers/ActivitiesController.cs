using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : SpeurzoekersBaseController
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public ActivitiesController(IBlobRepository blobRepository,
            IActivityRepository activityRepository,
            IMapper mapper)
        {
            _blobRepository = blobRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivities(Season season)
        {
            Activity[] activities;

            if (season == 0 || season >= EnumExtensions.GetAllSeasons())
            {
                activities = await _activityRepository.GetActivities();
            }
            else
            {
                activities = await _activityRepository.GetActivities(season);
            }

            return Ok(_mapper.Map<ActivityDto[]>(activities));
        }

        [HttpGet("nameandseason")]
        public async Task<IActionResult> GetActivitiesNameAndSeason()
        {
            ActivitySummary[] activities;
            activities = await _activityRepository.GetActivitiesNameAndSeason();
            return Ok(_mapper.Map<ActivitySummaryDto[]>(activities));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity([FromRoute]Guid id)
        {
            Activity activity = await _activityRepository.FetchActivity(id);

            if (activity != null)
            {
                return Ok(_mapper.Map<ActivityDto>(activity));
            }
            // else
            return NotFound();
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetActivityInfo(Guid id)
        {
            Activity activity = await _activityRepository.FetchActivity(id);

            if (activity != null)
            {
                return Ok(_mapper.Map<ActivityDto>(activity));
            }
            // else
            return NotFound();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetActivitiesSummary(Season season)
        {
            Dictionary<Guid, string> activities;

            if (season == 0 || season >= EnumExtensions.GetAllSeasons())
            {
                activities = await _activityRepository.GetActivitiesSummary();
            }
            else
            {
                activities = await _activityRepository.GetActivitiesSummary(season);
            }

            return Ok(activities.Select(x => new { id = x.Key, name = x.Value }));
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetPicture(Guid id, int? index)
        {
            if (index < 0 || index >= Activity.NrOfPictures)
            {
                return BadRequest();
            }

            string tagName = await _activityRepository.GetTagName(id, index ?? 0);

            try
            {
                var result = await _blobRepository.GetActivity(tagName);
                return Ok(Convert.ToBase64String(result));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}