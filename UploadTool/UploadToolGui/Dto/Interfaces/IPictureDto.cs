using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace UploadToolGui.Dto.Interfaces
{
    public interface IPictureDto
    {
        string[] TagNames { get; set; }

        Task<bool[]> LoadPictures(Image[] images);

        Task<bool> TagNameExistOutsideId(string tagName);

        Task<bool> PictureExist(int index);

        Task SavePictures(Image[] images, bool[] imgsChanged, string[] orgPictureNames, ImageFormat[] formats);
    }

    public interface IPictureDto<ID> : IPictureDto, IBase<ID>
    { }
}
