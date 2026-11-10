using Microsoft.Extensions.Options;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Speurzoekers.Data.Repositories
{
    public class LocalBlobRepository : IBlobRepository
    {
        private readonly string _path, _collectibleDirectory, _avatarDirectory, _activityDirectory;

        public LocalBlobRepository(IOptions<BlobStorage> blobStorage)
        {
            _path = Path.Combine(blobStorage.Value.LocalPath, blobStorage.Value.PictureContainer);
            _collectibleDirectory = blobStorage.Value.PictureDirectoryCollectible;
            _avatarDirectory = blobStorage.Value.PictureDirectoryAvatar;
            _activityDirectory = blobStorage.Value.PictureDirectoryActivity;

            try
            {
                using (FileStream _ = File.Create(
                    Path.Combine(_path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                { }
            }
            catch
            {
                throw new IOException("Kan niet naar lokale (blob) map schrijven.");
            }
        }

        public async Task<byte[]> GetCollectible(string tagName)
        {
            return await GetPicture(_collectibleDirectory, tagName);
        }

        public async Task<byte[]> GetAvatar(string tagName)
        {
            return await GetPicture(_avatarDirectory, tagName);
        }

        public async Task<byte[]> GetActivity(string tagName)
        {
            return await GetPicture(_activityDirectory, tagName);
        }

        public async Task<bool> CollectiblePictureExist(string tagName)
        {
            return await PictureExist(_collectibleDirectory, tagName);
        }

        public async Task<bool> AvatarPictureExist(string tagName)
        {
            return await PictureExist(_avatarDirectory, tagName);
        }

        public async Task<bool> ActivityPictureExist(string tagName)
        {
            return await PictureExist(_activityDirectory, tagName);
        }

        public async Task SaveCollectiblePicture(string tagName, Stream stream, bool erase)
        {
            await SavePicture(_collectibleDirectory, tagName, stream, erase);
        }

        public async Task SaveAvatarPicture(string tagName, Stream stream, bool erase)
        {
            await SavePicture(_avatarDirectory, tagName, stream, erase);
        }

        public async Task SaveActivityPicture(string tagName, Stream stream, bool erase)
        {
            await SavePicture(_activityDirectory, tagName, stream, erase);
        }

        public async Task CopyCollectiblePicture(string oldName, string newName, bool erase, bool removeOld)
        {
            await CopyPicture(_collectibleDirectory, oldName, newName, erase, removeOld);
        }

        public async Task CopyAvatarPicture(string oldName, string newName, bool erase, bool removeOld)
        {
            await CopyPicture(_avatarDirectory, oldName, newName, erase, removeOld);
        }

        public async Task CopyActivityPicture(string oldName, string newName, bool erase, bool removeOld)
        {
            await CopyPicture(_activityDirectory, oldName, newName, erase, removeOld);
        }

        public async Task RemoveCollectiblePicture(string tagName)
        {
            await RemovePicture(_collectibleDirectory, tagName);
        }

        public async Task RemoveAvatarPicture(string tagName)
        {
            await RemovePicture(_avatarDirectory, tagName);
        }

        public async Task RemoveActivityPicture(string tagName)
        {
            await RemovePicture(_activityDirectory, tagName);
        }

        private async Task<byte[]> GetPicture(string directory, string tagName)
        {
            if (tagName == null)
            {
                throw new ArgumentException(tagName);
            }

            string file = Path.Combine(_path, directory, tagName);

            if (!File.Exists(file))
            {
                throw new ArgumentException(tagName);
            }
            // else
            return await File.ReadAllBytesAsync(file);
        }

        private Task<bool> PictureExist(string directory, string tagName)
        {
            string file = Path.Combine(_path, directory, tagName);

            return Task.FromResult(File.Exists(file));
        }

        private async Task SavePicture(string directory, string name, Stream stream, bool erase)
        {
            string fileName = Path.Combine(_path, directory, name);

            Directory.CreateDirectory(Path.Combine(_path, directory));

            if (File.Exists(fileName))
            {
                if (erase)
                {
                    File.Delete(fileName);
                }
                else
                {
                    return;
                }
            }

            using (FileStream file = File.Create(fileName))
            {
                await stream.CopyToAsync(file);
            }
        }

        private Task CopyPicture(string directory, string oldName, string newName, bool erase, bool removeOld)
        {
            string oldFile = Path.Combine(_path, directory, oldName), 
                newFile = Path.Combine(_path, directory, newName);

            if (removeOld)
            {
                if (erase || !File.Exists(newFile))
                {
                    File.Move(oldFile, newFile);
                }
                else
                {
                    File.Delete(newFile);
                }
            }
            else
            {
                if (erase || !File.Exists(newFile))
                {
                    File.Copy(oldFile, newFile);
                }
            }

            return Task.FromResult<object>(null);
        }

        private Task RemovePicture(string directory, string tagName)
        {
            string file = Path.Combine(_path, directory, tagName);

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            return Task.FromResult<object>(null);
        }
    }
}
