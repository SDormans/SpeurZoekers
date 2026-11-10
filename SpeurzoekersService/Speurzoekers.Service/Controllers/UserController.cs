using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using Speurzoekers.Service.Dto.Avatar;
using Speurzoekers.Service.Dto.User;
using Speurzoekers.Service.Helpers;
using Speurzoekers.Service.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : SpeurzoekersBaseController
    {
        private readonly AuthenticationOptions _authenticationSettings;
        private readonly IUserRepository _userRepository;
        private readonly IAvatarRepository _avatarRepository;
        private readonly IMapper _mapper;
        private readonly IBlobRepository _blobRepository;
        private readonly IEmailService _emailService;

        public UserController(
            IOptions<AuthenticationOptions> authenticationSettings,
            IUserRepository userRepository,
            IAvatarRepository avatarRepository,
            IMapper mapper,
            IBlobRepository blobRepository,
            IEmailService emailService)
        {
            _authenticationSettings = authenticationSettings.Value;
            _userRepository = userRepository;
            _avatarRepository = avatarRepository;
            _mapper = mapper;
            _blobRepository = blobRepository;
            _emailService = emailService;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateBody([FromBody] AuthenticateDto auth)
        {
            UserIdDto user = await Authenticate(auth);

            if (user != null)
            {               
                return Ok(user);
            }            
            // else
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> AuthenticateForm([FromForm]AuthenticateDto auth) // Used for Swagger
        {
            UserIdDto user = await Authenticate(auth);

            if (user != null)
            {
                TimeSpan expireTime = new TimeSpan(0, _authenticationSettings.ExpirationTimeMinutes, 0);

                return Ok(new AccessTokensResponseDto(user.Token, expireTime));
            }
            //else
            return Unauthorized();
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationDto regDto)
        {
            try
            {
                UserRegister user = _mapper.Map<UserRegister>(regDto);
                Common.Domain.ActionResult result = await _userRepository.RegisterUser(user);

                if (result.Succeeded)
                {
                    string token = await _userRepository.GetEmailConfirmationToken(user.Email);

                    await _emailService.SendEmailConfirmAccount(user.Email, token);

                    return Ok();
                }
                else if (result.Errors[0].Code == ActionResultError.UserNamePendingKey)
                {
                    return BadRequest("Er is iets misgegaan, probeer het later opnieuw, of neem contact op met De Speurzoekers");
                }           
            }
            finally
            { }
            // else
            return BadRequest("Er is iets misgegaan, neem contact op met De Speurzoekers");
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmailRequest")]
        public async Task<IActionResult> ConfirmEmailRequest(string email)
        {
            try
            {
                string token = await _userRepository.GetEmailConfirmationToken(email);

                await _emailService.SendEmailConfirmAccount(email, token);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            bool tokenFailed;

            try
            {
                if (await _userRepository.ConfirmEmail(email, token))
                {
                    return Ok();
                }
                else
                {
                    tokenFailed = true;
                }
            }
            catch (InvalidOperationException) // user not found
            {
                tokenFailed = true;
            }
            catch
            {
                tokenFailed = false;
            }

            if (tokenFailed)
            {
                string time = Helper.MinutesToString(_authenticationSettings.MailTokenExpirationMinutes);

                return BadRequest("Kan dit account niet bevestigen, het account bestaat niet " +
                    $"of je bent te laat met bevestigen (dit moet binnen {time}).");
            }
            else
            {
                return BadRequest("Er is iets misgegaan, neem contact op met De Speurzoekers");
            }
        }

        [AllowAnonymous]
        [HttpGet("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordReset(string username)
        {
            try
            {
                PasswordReset reset = await _userRepository.GetPasswordResetToken(username);

                await _emailService.SendEmailPasswordReset(reset.Email, reset.Token);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("PasswordReset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDto passwordResetDto)
        {
            try
            {
                bool result = await _userRepository.ResetPassword(passwordResetDto.Username,
                    passwordResetDto.Token, passwordResetDto.NewPassword);

                if (result)
                {
                    return Ok();
                }
                // else
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("avatarimage")]
        public async Task<IActionResult> GetMyAvatar()
        {
            Guid userId = GetUserId();
            string tagName = await _avatarRepository.GetTagName(userId);

            try
            {
                if (tagName != null)
                {
                    byte[] result = await _blobRepository.GetAvatar(tagName);
                    return Ok(Convert.ToBase64String(result));
                }
            }
            catch (ArgumentException)
            { }

            return NotFound();
        }

        [HttpGet("avatarindex")]
        public async Task<IActionResult> GetAvatarIndex()
        {
            Guid userId = GetUserId();
            long? index = await _avatarRepository.GetAvatarIndex(userId);
            if(index != null)
            {
                return Ok(index);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> SetMyAvatar(ulong index)
        {
            Guid userId = GetUserId();

            try
            {
                await _avatarRepository.SetUserAvatar(userId, (long)index);
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckToken()
        {
            try
            {
                var result = await _userRepository.DoesUserExist(GetUserId());
                return Ok(new BooleanResultDto { Result = result });
            }
            catch
            {
                return Ok(new { result = false });
            }

        }

        [HttpGet("avatars")]
        public async Task<IActionResult> GetMyAvatars()
        {
            Guid userId = GetUserId();
            List<AvatarTypeNoText> list = await _avatarRepository.GetAvatars(userId);

            return Ok(_mapper.Map<List<AvatarTypeNoTextDto>>(list));
        }

        [HttpGet("displayname")]
        public async Task<IActionResult> GetMyDisplayName()
        {
            var userId = GetUserId();
            try
            {
                var displayName = await _userRepository.GetDisplayName(userId);
                return Ok(displayName);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("usercollectibles")]
        public async Task<IActionResult> GetMyUserCollectibles()
        {
            var userId = GetUserId();
            try
            {
                var userCollectibles = await _userRepository.GetUserCollectibles(userId);
                return Ok(userCollectibles);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("userpoints")]
        public async Task<IActionResult> GetUserPoints()
        {
            var userId = GetUserId();
            var result = await _userRepository.GetAllUserPoints(userId);
            return Ok(result);
        }

        private async Task<UserIdDto> Authenticate(AuthenticateDto auth)
        {
            bool result = await _userRepository.CheckAuthenticateAsync(auth.UserName, auth.Password);

            if (result)
            {
                UserInfo user = await _userRepository.GetUserInfoByName(auth.UserName);

                if (user != null)
                {
                    return new UserIdDto
                    {
                        Token = CreateToken(user)
                    };
                }
            }
            // else
            return null;
        }

        private string CreateToken(UserInfo user)
        {
            byte[] key = System.Text.Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // UserManager uses ClaimTypes.NameIdentifier for user id'ing
                }),
                Expires = DateTime.UtcNow.AddMinutes(_authenticationSettings.ExpirationTimeMinutes),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            tokenDescriptor.Subject.AddClaims(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        [HttpGet("totaldistance")]
        public async Task<IActionResult> GetTotalDistance()
        {
            var userId = GetUserId();
            var result = await _userRepository.GetTotalDistance(userId);
            return Ok(result);
        }

        [HttpPost("settotaldistance")]
        public async Task<IActionResult> SetTotalDistance(int dist)
        {
            var userId = GetUserId();
            await _userRepository.SetTotalDistance(userId, dist);
            return Ok();
        }


    }
}
