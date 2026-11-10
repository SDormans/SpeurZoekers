using AutoMapper;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace UploadToolGui.Dto
{
    public class AvatarTypeDto : IMainDto<long>, IPictureDto<long>
    {
        public long Id { get; set; }
        object IBase.Id => Id;
        public bool IsNew => Id == 0;
        public string Name { get; set; }

        public CollectibleRarity Rarity { get; set; }
        public int Points { get; set; }
        public Season Season { get; set; }
        public List<DescriptionText> Texts { get; set; }

        public string[] TagNames { get; set; }

        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;
        private readonly HelperFunctions<long> _helper;

        public AvatarTypeDto(SpeurzoekersDbConnection speurzoekersDbCon, IMapper mapper, int defaultPrice)
        {
            _speurzoekersDbCon = speurzoekersDbCon;
            _mapper = mapper;
            _helper = new HelperFunctions<long>();

            Texts = new List<DescriptionText>();
            TagNames = new string[1];
            Points = defaultPrice;
        }

        public Task<bool> NameExistOutsideId()
        {
            return _speurzoekersDbCon.AvatarRepository.NameExistOutsideId(Id, Name);
        }

        public async Task<bool[]> LoadPictures(Image[] images)
        {
            try
            {
                if (TagNames[0] == null)
                {
                    images[0] = null;
                }
                else
                {
                    using Stream stream =
                        new MemoryStream(await _speurzoekersDbCon.BlobRepository.GetAvatar(TagNames[0]));

                    images[0] = Image.FromStream(stream);
                }

                return new bool[] { true };
            }
            catch (ArgumentException)
            {
                return new bool[] { false };
            }
        }

        public Task<bool> PictureExist(int _)
        {
            return _speurzoekersDbCon.BlobRepository.AvatarPictureExist(TagNames[0]);
        }

        public Task<bool> TagNameExistOutsideId(string tagName)
        {
            return _speurzoekersDbCon.AvatarRepository.TagNameExistOutsideId(Id, tagName);
        }

        public Task SavePictures(Image[] images, bool[] imgsChanged, string[] orgPictureNames, ImageFormat[] formats)
        {
            return _helper.SavePicture(this, images[0], imgsChanged[0], TagNames[0], orgPictureNames[0], formats[0],
                _speurzoekersDbCon.BlobRepository.RemoveAvatarPicture,
                (Stream s, bool e) => _speurzoekersDbCon.BlobRepository.SaveAvatarPicture(TagNames[0], s, e),
                (string o, bool e, bool r) => _speurzoekersDbCon.BlobRepository.CopyAvatarPicture(o, TagNames[0], e, r));
        }

        public async Task SaveDb()
        {
            AvatarType avatar = _mapper.Map<AvatarType>(this);
            await _speurzoekersDbCon.AvatarRepository.AddOrUpdateAvatar(avatar);
            _mapper.Map(avatar, this);
        }

        public async Task RemoveItem()
        {
            await _speurzoekersDbCon.AvatarRepository.RemoveAvatar(Id);

            if (!await _speurzoekersDbCon.AvatarRepository.TagNameExistOutsideId(Id, TagNames[0]))
            {
                await _speurzoekersDbCon.BlobRepository.RemoveAvatarPicture(TagNames[0]);
            }
        }
    }
}
