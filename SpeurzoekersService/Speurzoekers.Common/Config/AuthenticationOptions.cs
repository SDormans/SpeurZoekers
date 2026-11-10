namespace Speurzoekers.Common.Config
{
    public class AuthenticationOptions
    {
        public string Secret { get; set; }
        public int ExpirationTimeMinutes { get; set; }
        public int MailTokenExpirationMinutes { get; set; }
    }
}
