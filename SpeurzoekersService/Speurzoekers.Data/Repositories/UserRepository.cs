using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SpeurzoekersDbContext _speurzoekersDbContext;
        private readonly AuthenticationOptions _authOptions;

        public UserRepository(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            SpeurzoekersDbContext speurzoekersDbContext, 
            IOptions<AuthenticationOptions> authOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _speurzoekersDbContext = speurzoekersDbContext;
            _authOptions = authOptions.Value;
        }

        public async Task<bool> CheckAuthenticateAsync(string userName, string password)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            return result.Succeeded;
        }

        public async Task<string> GetEmailConfirmationToken(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                return await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            // else
            return null;
        }

        public async Task<bool> ConfirmEmail(string username, string token)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            
            if (user != null)
            {
                IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, token);

                return identityResult.Succeeded;
            }
            // else
            return false;
        }

        public async Task<PasswordReset> GetPasswordResetToken(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                return new PasswordReset
                {
                    Email = user.Email,
                    Token = token
                };
            }
            // else
            return null;
        }

        public async Task<bool> ResetPassword(string username, string token,string newPassword)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                return result.Succeeded;
            }
            // else
            return false;
        }

        public async Task<UserInfo> GetUserInfoByName(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

                return new UserInfo
                {
                    Id = user.Id,
                    Roles = rolesOfUser.ToArray()
                };
            }
            // else
            return null;
        }

        public async Task<ActionResult> RegisterUser(UserRegister userToRegister)
        {
            ApplicationUser user = await  _userManager.FindByNameAsync(userToRegister.Email);
            
            if (user != null)
            {
                if (user.EmailConfirmed || await _speurzoekersDbContext.UserCollectibles
                        .AnyAsync(x => x.ApplicationUserId == user.Id))
                {
                    return new ActionResult
                    {
                        Succeeded = false,
                        Errors = new ActionResultError[]
                        {
                            new ActionResultError
                            {
                                Code = ActionResultError.DuplicateUserNameKey
                            }
                        }
                    };
                }
                // else
                DateTime now = DateTime.Now;
                bool confirmPending = await _speurzoekersDbContext.UserInfos
                    .AnyAsync(x => x.ApplicationUserId == user.Id &&
                        x.RegisterDate.AddMinutes(_authOptions.MailTokenExpirationMinutes) > now);

                if (confirmPending)
                {
                    return new ActionResult
                    {
                        Succeeded = false,
                        Errors = new ActionResultError[]
                        {
                            new ActionResultError
                            {
                                Code = ActionResultError.UserNamePendingKey
                            }
                        }
                    };
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            user = _mapper.Map<ApplicationUser>(userToRegister);
            user.UserInfo.RegisterDate = DateTime.Now;
            IdentityResult result = await _userManager.CreateAsync(user, userToRegister.Password);
            
            await _speurzoekersDbContext.SaveChangesAsync();
            
            return new ActionResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => new ActionResultError
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToArray()
            };
        }

        public async Task<ActionResult> RegisterUser(string email, string name, string password)
        {
            ApplicationUser user = new ApplicationUser
            {
                EmailConfirmed = true,
                Email = email,
                UserName = email,
                UserInfo = new UserInfoEntity
                {
                    DisplayName = name
                }
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            await _speurzoekersDbContext.SaveChangesAsync();

            return new ActionResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => new ActionResultError
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToArray()
            };
        }

        public async Task<string> GetDisplayName(Guid userId)
        {
            return await _speurzoekersDbContext.UserInfos.Where(u => u.ApplicationUserId == userId)
                .Select(x => x.DisplayName).SingleAsync();
        }

        public async Task<List<UserCollectible>> GetUserCollectibles(Guid userId)
        {
            List<UserCollectibleEntity> list = await _speurzoekersDbContext.UserCollectibles.Where(u => u.ApplicationUserId == userId).ToListAsync();
            return _mapper.Map<List<UserCollectibleEntity>, List<UserCollectible>>(list);
        }

        public async Task<TotalUserPoints> GetAllUserPoints(Guid userId)
        {
            int totalCatchPoints = await _speurzoekersDbContext.UserCollectibles.Where(u => u.ApplicationUserId == userId)
                                                                            .SumAsync(x => x.CatchPoints);

            int totalAnswerPoints = await _speurzoekersDbContext.UserCollectibles.Where(u => u.ApplicationUserId == userId)
                                                                            .SumAsync(x => x.AnswerPoints??0);

            long spendPoints = await _speurzoekersDbContext.UserInfos.Where(u => u.ApplicationUserId == userId)
                                                                    .Select(x => x.SpendPoints).SingleAsync();

            TotalUserPoints totalUserPoints = new TotalUserPoints
            {
                TotalAnswerPoints = totalAnswerPoints,
                TotalCatchPoints = totalCatchPoints,
                SpendPoints = spendPoints
            };

            return totalUserPoints;
        }

        public async Task<bool> DoesUserExist(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user != null;
        }

        public async Task<long> GetAvailablePoints(Guid userId)
        {
            int points = await _speurzoekersDbContext.UserCollectibles
                .Where(x => x.ApplicationUserId == userId)
                .SumAsync(x => (x.AnswerPoints ?? 0) + x.CatchPoints);
            long spend = await _speurzoekersDbContext.UserInfos
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.SpendPoints)
                .FirstAsync();

            return points - spend;
        }

        public async Task<int> GetTotalDistance(Guid userId)
        {
            return await _speurzoekersDbContext.UserInfos.Where(u => u.ApplicationUserId == userId)
                                                                                    .Select(x => x.TotalDistance)
                                                                                    .FirstAsync();
        }

        public async Task SetTotalDistance(Guid userId, int dist)
        {
            var result = await _speurzoekersDbContext.UserInfos.Where(u => u.ApplicationUserId == userId).FirstAsync();
            result.TotalDistance = dist;
            await _speurzoekersDbContext.SaveChangesAsync();
        }
    }
}
