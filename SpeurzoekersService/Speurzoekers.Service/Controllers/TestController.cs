using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using Speurzoekers.Service.Dto.User;
using Speurzoekers.Service.Helpers;
using Speurzoekers.Service.Services;
using System;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
#if DEBUG
    [Authorize(Roles = "Admin")]
    [Route("api/Test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public TestController(IEmailService emailService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpPost("Mail/BeeInternationalMessage")]
        public async Task<IActionResult> TestEmailPasswordReset(ContactEmailDto emailDto)
        {
            try
            {
                await _emailService.TestEmailBeeInternationalMessage(emailDto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Mail/ConfirmAccount")]
        public async Task<IActionResult> TestEmailConfirmAccount()
        {
            try
            {
                await _emailService.TestEmailConfirmAccount("123");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Mail/PasswordReset")]
        public async Task<IActionResult> TestEmailPasswordReset()
        {
            try
            {
                await _emailService.TestEmailPasswordReset(string.Empty);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("User/Create")]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationDto regDto)
        {
            try
            {
                Common.Domain.ActionResult result = await _userRepository
                    .RegisterUser(regDto.Email, regDto.Name, regDto.Password);

                if (result.Succeeded)
                {                    
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("User/Token/{email}")]
        public async Task<IActionResult> GetEmailConfirmationToken([FromRoute] string email)
        {
            return Ok(await _userRepository.GetEmailConfirmationToken(email));
        }
    }
#endif
}
