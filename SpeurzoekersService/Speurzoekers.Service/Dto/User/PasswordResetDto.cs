namespace Speurzoekers.Service.Dto.User
{
    public class PasswordResetDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
