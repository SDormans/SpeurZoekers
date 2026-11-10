using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speurzoekers.Common.Repositories
{
    public interface IAvatarRepository
    {
        /// <summary>Get the active avatar index for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>Avatar index</returns>
        Task<long?> GetAvatarIndex(Guid userId);


        /// <summary>Get the avatar tag name for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>Avatar tag name</returns>
        Task<string> GetTagName(Guid userId);

        /// <summary>Get the tag name for the given avatar</summary>
        /// <param name="index">Avatar index</param>
        /// <returns>Tag name</returns>
        Task<string> GetTagName(long index);

        /// <summary>Get all avatars</summary>
        /// <returns>List of avatars</returns>
        Task<List<AvatarTypeNoText>> GetAvatars();

        /// <summary>Get the avatars for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>List of avatars</returns>
        Task<List<AvatarTypeNoText>> GetAvatars(Guid userId);

        /// <summary>Get all avatars, with 'collected tag' for the given user</summary>
        /// <param name="userId">User-ID</param>
        /// <returns>List of avatars, with bool for 'is collected by user'</returns>
        Task<Dictionary<AvatarTypeNoText, bool>> GetAllAvatarsWithCollected(Guid userId);

        /// <summary>Add an avatar</summary>
        /// <param name="avatar">Avatar</param>
        Task AddAvatar(AvatarType avatar);

        /// <summary>Remove the given avatar</summary>
        /// <param name="index">Avatar index</param>
        Task RemoveAvatar(long index);

        /// <summary>Set the current avatar for the given user</summary>
        /// <param name="userId">User ID</param>
        /// <param name="index">Avatar index</param>
        Task SetUserAvatar(Guid userId, long index);

        /// <summary>Set intial avatar collection and current avatar for the given user,
        /// if no avatar is collected yet</summary>
        /// <param name="userId">User ID</param>
        /// <param name="index">Avatar index</param>
        Task SetFirstAvatar(Guid userId, long index);

        /// <summary>Add avatar to avatars collected for the given user, if points suffice</summary>
        /// <param name="userId">User ID</param>
        /// <param name="index">Avatar index</param>
        /// <param name="setAsCurrent">Set bought avatar as current</param>
        /// <returns>Purchase successful</returns>
        Task<bool> BuyAvatar(Guid userId, long index, bool setAsCurrent);

        ///<summary>(Re-)Attach avatar including subclasses</summary>
        /// <param name="index">Avatar index</param>
        /// <returns>Avatar</returns>
        Task<AvatarType> FetchAvatar(long index);

        /// <summary>Get a summary of all avatars sorted on name</summary>
        /// <returns> List of avatar indexes and names</returns>
        Task<Dictionary<long, string>> GetAvatarSummary();

        /// <summary>Get a summary of filtered avatars sorted on name</summary>
        /// <param name="criteria">List of filter criteria</param>
        /// <param name="orConcatenation">Join criteria by OR, otherwise AND</param>
        /// <param name="sorting">Sorting</param>
        /// <returns> List of avatars IDs and names</returns>
        Task<Dictionary<long, string>> GetAvatarSummary(
            IEnumerable<FilterCriterion> criteria, bool orConcatenation, Sorting sorting);

        /// <summary>Check whether the given tag name exists outside the given index</summary>
        /// <param name="index">Avatar index</param>
        /// <param name="tagName">File name</param>
        /// <returns>Tag name exists</returns>
        Task<bool> TagNameExistOutsideId(long index, string tagName);

        /// <summary>Add or update an avatar</summary>
        /// <param name="avatar">Avatar</param>
        Task AddOrUpdateAvatar(AvatarType avatar);

        /// <summary>Check whether the given avatar name exists outside the given index</summary>
        /// <param name="index">Avatar index</param>
        /// <param name="name">Avatar name</param>
        /// <returns>Name exists</returns>
        Task<bool> NameExistOutsideId(long index, string name);

        ///<summary>Get avatar info </summary>
        /// <param name="index">avatar index</param>
        /// <returns>avatar type</returns>
        Task<AvatarType> GetAvatarTypeInfo(long index);
    }
}
