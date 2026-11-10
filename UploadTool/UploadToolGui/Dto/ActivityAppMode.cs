using AutoMapper;
using UploadToolGui.Dto.Enums;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Helper.Enums;
using Speurzoekers.Common.UploadTools;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities;
using Speurzoekers.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadToolGui.Dto
{
    public class ActivityAppMode : IAppMode, IFilter, IAvailability, IPicture
    {
        public ControlOption ControlOptions => ControlOption.LoadAvailability | ControlOption.LoadPicture | 
            ControlOption.Filter |ControlOption.MultiplePicturesOptional;
        public string[] PictureNames => new[] { "Plaatje 1", "Plaatje 2" };
        public string[] AvailabilityListNames => new[] { "Seizoenen" };
        public EnumDetail[][] AvailabilityLists { get; }
        public int NameMaxLength => RepositoryHelper.GetColumnMaxStringLength<ActivityEntity>(x => x.Name);
        public int[] TagNamesMaxLength => new int[]
            {
                RepositoryHelper.GetColumnMaxStringLength<ActivityEntity>(x => x.TagName1),
                RepositoryHelper.GetColumnMaxStringLength<ActivityEntity>(x => x.TagName2)
            };

        public int? MaxWidth => null;
        public int? MaxHeight => null;
        public int? RequiredWidth => null;
        public int? RequiredHeight => null;
        public bool PreferLandscape => true;

        public FilterItem[] FilterList => new[]
        {
            FilterItem.Create<ActivityEntity, string>("Naam", (ent) => ent.Name, FilterType.String),
            FilterItem.Create<ActivityEntity, Season>("Seizoenen", (ent) => ent.Season, FilterType.Enum),
            FilterItem.Create<ActivityEntity, Season>("Aantal seizoenen", (ent) => ent.Season, FilterType.EnumCount),
            FilterItem.Create<ActivityEntity, string>("Afbeeldingsnaam 1", (ent) => ent.TagName1, FilterType.String),
            FilterItem.Create<ActivityEntity, string>("Afbeeldingsnaam 2", (ent) => ent.TagName2, FilterType.String)
        };
        public SortItem[] SortingList => new[]
        {
            SortItem.Create<ActivityEntity, string>("Naam", (ent) => ent.Name),
            SortItem.Create<ActivityEntity, Season>("Eerste seizoen", (ent) => ent.Season, SortingMethod.EnumLow),
            SortItem.Create<ActivityEntity, Season>("Laatste seizoen", (ent) => ent.Season, SortingMethod.EnumHigh)
        };

        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;
        private readonly HelperFunctions<Guid> _helper;

        public ActivityAppMode(SpeurzoekersDbConnection speurzoekersDbCon, IMapper mapper)
        {
            _speurzoekersDbCon = speurzoekersDbCon;
            _mapper = mapper;
            _helper = new HelperFunctions<Guid>();

            EnumDetail[] seasons = Enum.GetValues(typeof(Season))
                .Cast<Season>()
                .Select((value, index) =>
                    new EnumDetail<Season>(value, value.Description()))
                .ToArray();

            AvailabilityLists = new EnumDetail[][] { seasons };
        }

        public Task<ListItem[]> GetItems()
        {
            return _helper.GetItemList(_speurzoekersDbCon.ActivityRepository.GetActivitiesSummary);
        }

        public Task<ListItem[]> GetItems(
            IEnumerable<FilterCriterion> filters,
            bool orConcatenation,
            Sorting sorting)
        {
            return _helper.GetItemList(() => 
            _speurzoekersDbCon.ActivityRepository.GetActivitiesSummary(filters, orConcatenation, sorting));
        }

        public IBase NewItem()
        {
            return new ActivityDto(_speurzoekersDbCon, _mapper);
        }

        public async Task<IBase> GetItem(object id)
        {
            ActivityDto actDto = new ActivityDto(_speurzoekersDbCon, _mapper);
            Activity act = await _speurzoekersDbCon.ActivityRepository.FetchActivity((Guid)id);
            _mapper.Map(act, actDto);

            return actDto;
        }

        public bool IsNewItem(object id)
        {
            return id as Guid? == Guid.Empty;
        }
    }
}
