namespace Speurzoekers.Common.Domain.User
{
    public class PasswordReset
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
