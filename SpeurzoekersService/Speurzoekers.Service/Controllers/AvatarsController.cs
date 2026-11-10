using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto.Avatar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarsController : SpeurzoekersBaseController
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IAvatarRepository _avatarRepository;
        private readonly IMapper _mapper;

        public AvatarsController(IBlobRepository blobRepository,
            IAvatarRepository avatarRepository,
            IMapper mapper)
        {
            _blobRepository = blobRepository;
            _avatarRepository = avatarRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvatars()
        {
            List<AvatarTypeNoText> avatars = await _avatarRepository.GetAvatars();
            return Ok(_mapper.Map<List<AvatarTypeNoTextDto>>(avatars));
        }

        [HttpGet("myCollection")]
        public async Task<IActionResult> GetAvatarsWithCollected()
        {
            Guid userId = GetUserId();
            Dictionary<AvatarTypeNoText, bool> avatars = await _avatarRepository.GetAllAvatarsWithCollected(userId);
            return Ok(_mapper.Map<AvatarTypeCollectionDto[]>(avatars));
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetPicture(ulong index)
        {
            string tagName = await _avatarRepository.GetTagName((long)index);

            try
            {
                var result = await _blobRepository.GetAvatar(tagName);
                return Ok(Convert.ToBase64String(result));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpGet ("AvatarTypeInfo")]
        public async Task<IActionResult> GetAvatarTypeInfo(ulong index)
        {
            AvatarType avatarTypeInfo = await _avatarRepository.GetAvatarTypeInfo((long)index);
            if (avatarTypeInfo != null)
            {
                return Ok(_mapper.Map<AvatarType>(avatarTypeInfo));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("first")]
        public async Task<IActionResult> SetFirstAvatar(ulong index)
        {
            Guid userId = GetUserId();

            try
            {
                await _avatarRepository.SetFirstAvatar(userId, (long)index);
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost("collect")]
        public async Task<IActionResult> BuyAvatar(ulong index, bool setAsCurrent)
        {
            Guid userId = GetUserId();

            try
            {
                if (await _avatarRepository.BuyAvatar(userId, (long)index, setAsCurrent))
                { 
                    return Ok();
                }
                // else
                return BadRequest("Not enough points!");
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}