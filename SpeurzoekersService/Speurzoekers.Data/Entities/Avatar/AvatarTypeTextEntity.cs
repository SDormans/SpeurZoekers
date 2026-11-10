namespace Speurzoekers.Data.Entities.Avatar
{
    public class AvatarTypeTextEntity : DescriptionTextEntity
    {
        public long AvatarTypeIndex { get; set; }
        public virtual AvatarTypeEntity AvatarType { get; set; }
    }
}
