namespace UploadToolGui.Dto.Interfaces
{
    public interface IPicture
    {
        string[] PictureNames { get; }
        int[] TagNamesMaxLength { get; }

        int? MaxWidth { get; }
        int? MaxHeight { get; }
        int? RequiredWidth { get; }
        int? RequiredHeight { get; }
        bool PreferLandscape { get; }
    }
}
