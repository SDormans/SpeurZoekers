using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Repositories;

namespace Speurzoekers.Data.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly CloudBlobContainer _blobContainer;
        private readonly string _collectibleDirectory, _avatarDirectory, _activityDirectory;

        public BlobRepository(IOptions<BlobStorage> blobStorage)
        { 
            _blobContainer = GetCloudBlobContainer(
                blobStorage.Value.ConnectionString, 
                blobStorage.Value.PictureContainer);
            _collectibleDirectory = blobStorage.Value.PictureDirectoryCollectible;
            _avatarDirectory = blobStorage.Value.PictureDirectoryAvatar;
            _activityDirectory = blobStorage.Value.PictureDirectoryActivity;
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

            CloudBlockBlob blob = _blobContainer
                .GetDirectoryReference(directory)
                .GetBlockBlobReference(tagName);

            if (!blob.ExistsAsync().Result)
            {
                throw new ArgumentException(tagName);
            }
            using (MemoryStream stream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(stream);
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        private async Task<bool> PictureExist(string directory, string tagName)
        {
            CloudBlockBlob blob = _blobContainer
                .GetDirectoryReference(directory)
                .GetBlockBlobReference(tagName);

            return await blob.ExistsAsync();
        }

        private async Task SavePicture(string directory, string name, Stream stream, bool erase)
        {
            CloudBlockBlob blob = _blobContainer
                .GetDirectoryReference(directory)
                .GetBlockBlobReference(name);

            if (await blob.ExistsAsync())
            {
                if (erase)
                {
                    await blob.DeleteAsync();
                }
                else
                {
                    return;
                }
            }

            string ext = Path.GetExtension(name).TrimStart('.');
            blob.Properties.ContentType = $"image/{ext}";

            await blob.UploadFromStreamAsync(stream);
        }

        private async Task CopyPicture(string directory, string oldName, string newName, bool erase, bool removeOld)
        {
            CloudBlockBlob source =  _blobContainer.GetDirectoryReference(directory).GetBlockBlobReference(oldName);
            CloudBlockBlob target = _blobContainer.GetDirectoryReference(directory).GetBlockBlobReference(newName);

            if (erase || !await target.ExistsAsync())
            {
                await target.StartCopyAsync(source);

                while (target.CopyState.Status == CopyStatus.Pending)
                {
                    await Task.Delay(100);
                }

                if (target.CopyState.Status != CopyStatus.Success)
                {
                    throw new Exception("Rename failed: " + target.CopyState.Status);
                }
            }

            if (removeOld)
            {
                await source.DeleteAsync();
            }
        }

        private async Task RemovePicture(string directory, string tagName)
        {
            CloudBlockBlob blob = _blobContainer
                .GetDirectoryReference(directory)
                .GetBlockBlobReference(tagName);

            await blob.DeleteAsync();
        }

        private CloudBlobContainer GetCloudBlobContainer(string blobDbConnectionString, string blobDbPictureContainer)
        {
            if (CloudStorageAccount.TryParse(blobDbConnectionString, out CloudStorageAccount storageAccount))
            {
                try
                {
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer blobContainer = cloudBlobClient.GetContainerReference(blobDbPictureContainer);

                    if (blobContainer.ExistsAsync().Result)
                    { return blobContainer; }
                }
                catch
                { }
            }

            throw new IOException("Geen connectie met de blob opslag.");
        }
    }
}
