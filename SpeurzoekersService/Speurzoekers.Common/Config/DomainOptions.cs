namespace Speurzoekers.Common.Config
{
    public class DomainOptions
    {
        public string AppName { get; set; }
        public string AppUrlAccountConfirmed { get; set; }
        public string AppUrlResetPasswordConfirmed { get; set; }
        public string AppUrlError { get; set; }
        public string CompanyName { get; set; }
        public string MailAddressFrom { get; set; }
        public string MailAddressNameFrom { get; set; }
        public string[] MailFooter{ get; set; }
        public string SendGridApiKey { get; set; }
    }
}
