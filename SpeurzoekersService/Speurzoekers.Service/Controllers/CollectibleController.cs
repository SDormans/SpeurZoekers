using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using Speurzoekers.Service.Dto.Collectible;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectibleController : SpeurzoekersBaseController
    {
        private readonly IBlobRepository _blobRepository;
        private readonly ICollectibleRepository _collectibleRepository;
        private readonly IMapper _mapper;

        public CollectibleController(IBlobRepository blobRepository,
            ICollectibleRepository collectibleRepository,
            IMapper mapper)
        {
            _blobRepository = blobRepository;
            _collectibleRepository = collectibleRepository;
            _mapper = mapper;
        }

        [HttpGet("collectibletypeimage")]
        public async Task<IActionResult> GetPicture(Guid collectibleTypeId)
        {
            var tagName = await _collectibleRepository.GetTagName(collectibleTypeId);

            try
            {
                var result = await _blobRepository.GetCollectible(tagName);
                return Ok(Convert.ToBase64String(result));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost("caughtcollectible")]
        public async Task<IActionResult> CaughtCollectible(Guid collectiblePositionId)
        {
            try
            {
                var userId = GetUserId();
                               
                if (await _collectibleRepository.CollectiblePositionExists(collectiblePositionId)
                    && !await _collectibleRepository.CollectiblePositionAlreadyCaught(collectiblePositionId, userId))
                {
                    return Ok(await _collectibleRepository.SaveCaughtCollectible(collectiblePositionId, userId));
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("collectibletypeinfo")]
        public async Task<IActionResult> GetCollectibleType(Guid collectibleTypeId)
        {
            CollectibleType cols = await _collectibleRepository.GetCollectibleType(collectibleTypeId);

            if (cols != null)
            {
                CollectibleTypeDto colDtos = _mapper.Map<CollectibleTypeDto>(cols);
                return Ok(colDtos);
            }

            return NotFound();
        }

        [HttpGet("collectibletypenameandpoints")]
        public async Task<IActionResult> GetCollectibleTypeNameAndPoints(Guid collectibleTypeId)
        {
            CollectibleTypeNameAndPoints cols = await _collectibleRepository.GetCollectibleTypeNameAndPoints(collectibleTypeId);

            if (cols != null)
            {
                return Ok(cols);
            }

            return NotFound();
        }


        [HttpGet("collectibletypes")]
        public async Task<IActionResult> GetCollectibleTypes()
        {
            var result = await _collectibleRepository.GetCollectibleTypeIds();
            return Ok(result.Select(g => new { Id = g }));
        }

        [HttpGet("collectibletypesatbiotope")]
        public async Task<IActionResult> GetCollectibleTypeIdsAtBiotope(Biotope biotope)
        {
            var result = await _collectibleRepository.GetCollectibleTypeIdsAtBiotope(biotope);
            return Ok(result.Select(g => new { Id = g }));
        }

        [HttpPost("usercollectibleanswer")]
        public async Task<IActionResult> UserCollectibleAnswer(AnswerQuestionDto answerQuestionDto)
        {
            try
            {
                if (await _collectibleRepository.CheckQuestionAnswered(answerQuestionDto.UserCollectibleId) == false)
                {
                    await _collectibleRepository.WriteQuestionAway(answerQuestionDto.UserCollectibleId, answerQuestionDto.AnsweredCorrect);
                    return Ok();
                }
                else
                {
                    return BadRequest("De vraag is al beantwoord");
                }

            }
            catch
            {
                return BadRequest();
            }
        }

       
    }
}
