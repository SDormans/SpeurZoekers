using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace Speurzoekers.Service.Controllers
{
    public class SpeurzoekersBaseController : ControllerBase
    {
        protected Guid GetUserId()
        {
            JwtSecurityToken token;
            try
            {
                var authheader = Request.Headers["Authorization"].ToString();
                var handler = new JwtSecurityTokenHandler();
                token = handler.ReadToken(authheader.Substring(7)) as JwtSecurityToken;
            }
            catch
            {
                throw new AuthenticationException();
            }

            var isNameIdType = new Func<Claim, bool>(c => c.Type == "nameid");
            if (token?.Claims == null || !token.Claims.Any(isNameIdType))
                throw new AuthenticationException();

            var userId = token.Claims.First(isNameIdType).Value;

            return Guid.Parse(userId);
        }
    }
}
