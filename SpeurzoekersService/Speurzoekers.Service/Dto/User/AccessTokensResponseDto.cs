using System;

namespace Speurzoekers.Service.Dto.User
{
    public class AccessTokensResponseDto
    {
        public AccessTokensResponseDto(string token, TimeSpan expireTime)
        {
            Access_token = token;
            Token_type = "Bearer";
            Expires_in = expireTime.TotalSeconds;
        }

        public string Access_token { get; set; }
        public string Refresh_token { get; set; }
        public string Token_type { get; set; }
        public double Expires_in { get; set; }
    }
}
