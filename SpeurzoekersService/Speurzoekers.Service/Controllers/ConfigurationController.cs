using Microsoft.AspNetCore.Mvc;
using Speurzoekers.Common.Repositories;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationController(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        [HttpGet("catchrange")]
        public async Task<IActionResult> GetCatchRange()
        {
            return Ok(new { CatchRange = await _configurationRepository.GetCatchRange() });
        }

        [HttpGet("spotrange")]
        public async Task<IActionResult> GetSpottingRange()
        {
            return Ok(new { SpottingRange = await _configurationRepository.GetSpottingRange() });
        }

        [HttpGet("avatarPrice")]
        public async Task<IActionResult> GetAvatarPrice()
        {
            return Ok(new { AvatarPrice = await _configurationRepository.GetAvatarPrice() });
        }
    }
}
