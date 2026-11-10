namespace Speurzoekers.Common.Config
{
    public static class ConfigurationKeys
    {
        public const string AuthenticationSection = "AuthenticationOptions",
            DomainOptionsSection = "DomainOptions",
            BlobStorageSection = "BlobStorage", BlobLocalPathKey = "BlobLocalPath",
            DbConnectionStringWin = "DatabaseConnectionStringWin", 
            DbConnectionStringUnix = "DatabaseConnectionStringUnix";
    }
}
