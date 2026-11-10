using AutoMapper;
using UploadToolGui.Dto.Enums;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Helper;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Avatar;
using Speurzoekers.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UploadToolGui.Dto
{
    public class AvatarTypeAppMode : IAppMode, IFilter, IPicture
    {
        public ControlOption ControlOptions => ControlOption.LoadPicture |
            ControlOption.Filter | ControlOption.MultipleTexts;
        public string[] PictureNames => new[] { string.Empty };
        public int NameMaxLength => RepositoryHelper.GetColumnMaxStringLength<AvatarTypeEntity>(x => x.Name);
        public int[] TagNamesMaxLength => new int[]
            {
                RepositoryHelper.GetColumnMaxStringLength<AvatarTypeEntity>(x => x.TagName)
            };

        public int? MaxWidth => null;
        public int? MaxHeight => null;
        public int? RequiredWidth => null;
        public int? RequiredHeight => null;
        public bool PreferLandscape => false;

        public FilterItem[] FilterList => new[]
        {
            FilterItem.Create<AvatarTypeEntity, string>("Naam", (ent) => (ent).Name, FilterType.String),
            FilterItem.Create<AvatarTypeEntity, string>("Afbeeldingsnaam", (ent) => ent.TagName, FilterType.String)
        };
        public SortItem[] SortingList => new[]
        {
            SortItem.Create<AvatarTypeEntity, string>("Naam", (ent) => ent.Name)
        };

        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;
        private readonly HelperFunctions<long> _helper;
        private readonly int _defaultPrice;

        public AvatarTypeAppMode(SpeurzoekersDbConnection speurzoekersDbCon, IMapper mapper, int defaultPrice)
        {
            _speurzoekersDbCon = speurzoekersDbCon;
            _mapper = mapper;
            _defaultPrice = defaultPrice;
            _helper = new HelperFunctions<long>();
        }

        public Task<ListItem[]> GetItems()
        {
            return _helper.GetItemList(_speurzoekersDbCon.AvatarRepository.GetAvatarSummary);
        }

        public Task<ListItem[]> GetItems(
            IEnumerable<FilterCriterion> filters,
            bool orConcatenation,
            Sorting sorting)
        {
            return _helper.GetItemList(() =>
                _speurzoekersDbCon.AvatarRepository.GetAvatarSummary(filters, orConcatenation, sorting));
        }

        public IBase NewItem()
        {
            return new AvatarTypeDto(_speurzoekersDbCon, _mapper, _defaultPrice);
        }

        public async Task<IBase> GetItem(object id)
        {
            AvatarTypeDto avatarDto = new AvatarTypeDto(_speurzoekersDbCon, _mapper, _defaultPrice);
            AvatarType avatar = await _speurzoekersDbCon.AvatarRepository.FetchAvatar((long)id);
            _mapper.Map(avatar, avatarDto);

            return avatarDto;
        }

        public bool IsNewItem(object id)
        {
            return id as long? == 0;
        }
    }
}
