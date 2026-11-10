using AutoMapper;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Collectible;
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
    public class CollectibleTypeDto : IMainDto<Guid>, IQuestionDto<Guid>, IAvailabilityDto<Guid>, IPictureDto<Guid>
    {
        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;

        public Guid Id { get; set; }
        object IBase.Id => Id;
        public bool IsNew => Id == Guid.Empty;
        public string Name { get; set; }

        public CollectibleRarity Rarity { get; set; }
        public int Points { get; set; }
        public Season Season { get; set; }
        public List<DescriptionText> Texts { get; set; }

        public int QuestionPoints { get; set; }
        public string QuestionText { get; set; }
        public ICollection<QuestionChoice> QuestionChoices { get; set; }
            

        public Biotope Biotopes { get; set; }
        public CalenderMonth MonthsAvailable { get; set; }

        public string[] TagNames { get; set; }
        public Func<Enum>[] AvailabilityListProps { get; }

        public FilterItem[] FilterList { get; private set; }

        private readonly HelperFunctions<Guid> _helper;

        public CollectibleTypeDto(SpeurzoekersDbConnection speurzoekersDbCon, IMapper mapper)
        {
            _speurzoekersDbCon = speurzoekersDbCon;
            _mapper = mapper;
            _helper = new HelperFunctions<Guid>();

            Rarity = CollectibleRarity.Common;
            Texts = new List<DescriptionText>();
            QuestionChoices = new List<QuestionChoice>();
            TagNames = new string[1];
            AvailabilityListProps = new Func<Enum>[] { () => Biotopes, () => MonthsAvailable };
        }

        public Task<bool> NameExistOutsideId()
        {
            return _speurzoekersDbCon.CollectibleRepository.NameExistOutsideId(Id, Name);
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
                        new MemoryStream(await _speurzoekersDbCon.BlobRepository.GetCollectible(TagNames[0]));

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
            return _speurzoekersDbCon.BlobRepository.CollectiblePictureExist(TagNames[0]);
        }

        public Task<bool> TagNameExistOutsideId(string tagName)
        {
            return _speurzoekersDbCon.CollectibleRepository.TagNameExistOutsideId(Id, tagName);
        }

        public Task SavePictures(Image[] images, bool[] imgsChanged, string[] orgPictureNames, ImageFormat[] formats)
        {
            return _helper.SavePicture(this, images[0], imgsChanged[0], TagNames[0], orgPictureNames[0], formats[0],
                    _speurzoekersDbCon.BlobRepository.RemoveCollectiblePicture,
                    (Stream s, bool e) => _speurzoekersDbCon.BlobRepository.SaveCollectiblePicture(TagNames[0], s, e),
                    (string o, bool e, bool r) => _speurzoekersDbCon.BlobRepository.CopyCollectiblePicture(o, TagNames[0], e, r));
        }

        public async Task SaveDb()
        {
            CollectibleType col = _mapper.Map<CollectibleType>(this);
            await _speurzoekersDbCon.CollectibleRepository.AddOrUpdateCollectible(col);
            _mapper.Map(col, this);
        }

        public async Task RemoveItem()
        {
            await _speurzoekersDbCon.CollectibleRepository.RemoveCollectible(Id);

            if (!await _speurzoekersDbCon.CollectibleRepository.TagNameExistOutsideId(Id, TagNames[0]))
            {
                await _speurzoekersDbCon.BlobRepository.RemoveCollectiblePicture(TagNames[0]);
            }
        }

        public void SetAvailabilityListProp(int index, IEnumerable<Enum> enums)
        {
            if (index == 0)
            {
                Biotopes = (Biotope)enums.Sum(x => (long)(Biotope)x);
            }
            else
            {
                MonthsAvailable = (CalenderMonth)enums.Sum(x => (long)(CalenderMonth)x);
            }
        }
    }
}
