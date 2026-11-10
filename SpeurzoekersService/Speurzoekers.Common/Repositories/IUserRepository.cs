using System;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.User;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Speurzoekers.Common.Repositories
{
    public interface IUserRepository
    {
        /// <summary>Authenticate a given user</summary>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns>Authentication succeeded</returns>
        Task<bool> CheckAuthenticateAsync(string userName, string password);

        /// <summary>Get user ID and role names for a given user</summary>
        /// <param name="userName">User name</param>
        /// <returns>User ID and role names</returns>
        Task<UserInfo> GetUserInfoByName(string userName);

        /// <summary>Register a new user</summary>
        /// <param name="userToRegister">User data</param>
        /// <returns>Registration result</returns>
        Task<ActionResult> RegisterUser(UserRegister userToRegister);

        /// <summary>Register a new user, without the need for mail confirmation</summary>
        /// <param name="email">Mail address</param>
        /// <param name="name">Display name</param>
        /// <param name="password">Password</param>
        /// <returns>Registration result</returns>
        Task<ActionResult> RegisterUser(string email, string name, string password);

        /// <summary>Get the displayname name for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>Display name</returns>
        Task<string> GetDisplayName(Guid userId);

        /// <summary>Get the collected collectibles for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>List of usercollectibles</returns>
        Task<List<UserCollectible>> GetUserCollectibles(Guid userId);

        /// <summary>Gets and sums all CatchPoints and AnswerPoints</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>CatchPoints, AnswerPoints and SpendPoints</returns>
        Task<TotalUserPoints> GetAllUserPoints(Guid userId);

        /// <summary>Quick check if user exists</summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DoesUserExist(Guid userId);

        /// <summary>Get the total points - spend points for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>Available points</returns>
        Task<long> GetAvailablePoints(Guid userId);

        /// <summary>Get the total distance traveled (in meters) for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>Total distance</returns>
        Task<int> GetTotalDistance(Guid userId);

        /// <summary>Set the total distance traveled (in meters) for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <param name="dist">total distance</param>
        Task SetTotalDistance(Guid userId, int dist);
        /// <summary>Gets an email confirmation token</summary>
        /// <param name="username">User name</param>
        /// <returns>Confirmation token</returns>
        Task<string> GetEmailConfirmationToken(string username);

        /// <summary>Confirm email confirmation token</summary>
        /// <param name="username">User name</param>
        /// <param name="token">Token</param>
        /// <returns>Token validates</returns>
        Task<bool> ConfirmEmail(string username, string token);

        /// <summary>Gets a password reset token</summary>
        /// <param name="username">User name</param>
        /// <returns>Password reset token</returns>
        Task<PasswordReset> GetPasswordResetToken(string username);

        /// <summary>Reset password if token validates</summary>
        /// <param name="username">User name</param>
        /// <param name="token">Password reset token</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Reset succeeded</returns>
        Task<bool> ResetPassword(string username, string token, string newPassword);
    }
}
