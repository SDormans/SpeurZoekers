namespace Speurzoekers.Common.Config
{
    public class BlobStorage
    {
        public string ConnectionString { get; set; }
        public string LocalPath { get; set; }
        public string PictureContainer { get; set; }
        public string PictureDirectoryCollectible { get; set; }
        public string PictureDirectoryAvatar { get; set; }
        public string PictureDirectoryActivity { get; set; }
    }
}
