using AutoMapper;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UploadToolGui.Dto
{
    public class ActivityDto : IMainDto<Guid>, IAvailabilityDto<Guid>, IPictureDto<Guid>
    {
        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;
        private readonly HelperFunctions<Guid> _helper;

        public Guid Id { get; set; }
        object IBase.Id => Id;
        public bool IsNew => Id == Guid.Empty;
        public string Name { get; set; }

        public List<DescriptionText> Texts { get; set; }

        public string[] TagNames { get; set; }
        public Func<Enum>[] AvailabilityListProps { get; }

        public CollectibleRarity Rarity { get; set; }
        public int Points { get; set; }
        public Season Season { get; set; }

        public ActivityDto(SpeurzoekersDbConnection speurzoekersDbCon, IMapper mapper)
        {
            _speurzoekersDbCon = speurzoekersDbCon;
            _mapper = mapper;
            _helper = new HelperFunctions<Guid>();

            Texts = new List<DescriptionText>(1)
            {
                new DescriptionText()
            };
            TagNames = new string[Activity.NrOfPictures];
            AvailabilityListProps = new Func<Enum>[] { () => Season };
        }

        public Task<bool> NameExistOutsideId()
        {
            return _speurzoekersDbCon.ActivityRepository.NameExistOutsideId(Id, Name);
        }

        public async Task<bool[]> LoadPictures(Image[] images)
        {
            bool[] success = new bool[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                try
                {
                    if (TagNames[i] == null)
                    {
                        images[i] = null;
                    }
                    else
                    {
                        using Stream stream =
                            new MemoryStream(await _speurzoekersDbCon.BlobRepository.GetActivity(TagNames[i]));
                        images[i] = Image.FromStream(stream);
                    }

                    success[i] = true;
                }
                catch (ArgumentException)
                { }
            }

            return success;
        }

        public Task<bool> PictureExist(int _)
        {
            return _speurzoekersDbCon.BlobRepository.ActivityPictureExist(TagNames[0]);
        }

        public Task<bool> TagNameExistOutsideId(string tagName)
        {
            return _speurzoekersDbCon.ActivityRepository.TagNameExistOutsideId(Id, tagName);
        }

        public async Task SavePictures(Image[] images, bool[] imgsChanged, string[] orgPictureNames, ImageFormat[] formats)
        {
            await GetSavePicture(images, imgsChanged, orgPictureNames, formats, 0);

            for (int i = 1; i < images.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(TagNames[i]))
                {
                    await GetSavePicture(images, imgsChanged, orgPictureNames, formats, i);
                }
                else if (orgPictureNames[i] != null &&
                    !await _speurzoekersDbCon.ActivityRepository.TagNameExistOutsideId(Id, orgPictureNames[i]))
                {
                    await _speurzoekersDbCon.BlobRepository.RemoveActivityPicture(orgPictureNames[i]);
                }
            }
        }

        public async Task SaveDb()
        {
            Activity act = _mapper.Map<Activity>(this);
            await _speurzoekersDbCon.ActivityRepository.AddOrUpdateActivity(act);
            _mapper.Map(act, this);
        }

        public async Task RemoveItem()
        {
            await _speurzoekersDbCon.ActivityRepository.RemoveActivity(Id);

            if (!await _speurzoekersDbCon.ActivityRepository.TagNameExistOutsideId(Id, TagNames[0]))
            {
                await _speurzoekersDbCon.BlobRepository.RemoveActivityPicture(TagNames[0]);
            }
        }

        private Task GetSavePicture(Image[] images, bool[] imgsChanged, string[] orgPictureNames, ImageFormat[] formats, int index)
        {
            return _helper.SavePicture(this, images[index], imgsChanged[index], TagNames[index], orgPictureNames[index], formats[index],
                _speurzoekersDbCon.BlobRepository.RemoveActivityPicture,
                (Stream s, bool e) => _speurzoekersDbCon.BlobRepository.SaveActivityPicture(TagNames[index], s, e),
                (string o, bool e, bool r) => _speurzoekersDbCon.BlobRepository.CopyActivityPicture(o, TagNames[index], e, r));
        }

        public void SetAvailabilityListProp(int index, IEnumerable<Enum> enums)
        {
            Season = (Season)enums.Sum(x => (long)(Season)x);
        }
    }
}
