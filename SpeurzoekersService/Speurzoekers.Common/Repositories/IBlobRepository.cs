using System.IO;
using System.Threading.Tasks;

namespace Speurzoekers.Common.Repositories
{
    public interface IBlobRepository
    {
        /// <summary>Get the collectible picture for the given file name</summary>
        /// <param name="tagName">Name of the file</param>
        /// <returns>Picture</returns>
        Task<byte[]> GetCollectible(string tagName);

        /// <summary>Get the avatar picture for the given file name</summary>
        /// <param name="tagName">Name of the file</param>
        /// <returns>Picture</returns>
        Task<byte[]> GetAvatar(string tagName);

        /// <summary>Get the activity picture for the given file name</summary>
        /// <param name="tagName">Name of the file</param>
        /// <returns>Picture</returns>
        Task<byte[]> GetActivity(string tagName);

        /// <summary>Checks whether the picture exist</summary>
        /// <param name="tagName">Name of the file</param>
        /// <returns>Picture exist</returns>
        Task<bool> CollectiblePictureExist(string tagName);

        /// <summary>Checks whether the picture exist</summary>
        /// <param name="tagName">Name of the file</param>
        Task<bool> AvatarPictureExist(string tagName);

        /// <summary>Checks whether the picture exist</summary>
        /// <param name="tagName">Name of the file</param>
        Task<bool> ActivityPictureExist(string tagName);

        /// <summary>Save (update when erase = true) a collectible picture to the blob storage</summary>
        /// <param name="name">Storage name</param>
        /// <param name="stream">Image stream</param>
        /// <param name="erase">Override existing image</param>
        Task SaveCollectiblePicture(string name, Stream stream, bool erase);

        /// <summary>Save (update when erase = true) an avatar picture to the blob storage</summary>
        /// <param name="name">Storage name</param>
        /// <param name="stream">Image stream</param>
        /// <param name="erase">Override existing image</param>
        Task SaveAvatarPicture(string name, Stream stream, bool erase);

        /// <summary>Save (update when erase = true) an activity picture to the blob storage</summary>
        /// <param name="name">Storage name</param>
        /// <param name="stream">Image stream</param>
        /// <param name="erase">Override existing image</param>
        Task SaveActivityPicture(string name, Stream stream, bool erase);

        /// <summary>Copies the given collectible picture</summary>
        /// <param name="oldName">Name of the file</param>
        /// <param name="newName">Name of the new file</param>
        /// <param name="erase">Override existing image</param>
        /// <param name="removeOld">Delete the old picture</param>
        Task CopyCollectiblePicture(string oldName, string newName, bool erase, bool removeOld);

        /// <summary>Copies the given avatar picture</summary>
        /// <param name="oldName">Name of the file</param>
        /// <param name="newName">Name of the new file</param>
        /// <param name="erase">Override existing image</param>
        /// <param name="removeOld">Delete the old picture</param>
        Task CopyAvatarPicture(string oldName, string newName, bool erase, bool removeOld);

        /// <summary>Copies the given activity picture</summary>
        /// <param name="oldName">Name of the file</param>
        /// <param name="newName">Name of the new file</param>
        /// <param name="erase">Override existing image</param>
        /// <param name="removeOld">Delete the old picture</param>
        Task CopyActivityPicture(string oldName, string newName, bool erase, bool removeOld);

        /// <summary>Removes the given collectible picture from blob storage</summary>
        /// <param name="tagName">Name of the file</param>
        Task RemoveCollectiblePicture(string tagName);

        /// <summary>Removes the given avatar picture from blob storage</summary>
        /// <param name="tagName">Name of the file</param>
        Task RemoveAvatarPicture(string tagName);

        /// <summary>Removes the given activity picture from blob storage</summary>
        /// <param name="tagName">Name of the file</param>
        Task RemoveActivityPicture(string tagName);
    }
}
