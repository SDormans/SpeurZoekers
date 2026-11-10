using AutoMapper;
using UploadToolGui.Dto.Enums;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.Helper.Enums;
using Speurzoekers.Common.UploadTools;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Collectible;
using Speurzoekers.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadToolGui.Dto
{
    public class CollectibleTypeAppMode : IAppMode, IFilter, IAvailability, IPicture
    {
        public ControlOption ControlOptions =>
            ControlOption.LoadQuestion | ControlOption.LoadAvailability | ControlOption.LoadPicture | ControlOption.Filter |
            ControlOption.MultipleTexts | ControlOption.Points | ControlOption.Rarity;
        public string[] PictureNames => new[] { string.Empty };
        public string[] AvailabilityListNames => new[] { "Biotopen", "Kalender" };
        public EnumDetail[][] AvailabilityLists { get; }
        public int NameMaxLength => RepositoryHelper.GetColumnMaxStringLength<CollectibleTypeEntity>(x => x.Name);
        public int[] TagNamesMaxLength => new int[]
            {
                RepositoryHelper.GetColumnMaxStringLength<CollectibleTypeEntity>(x => x.TagName)
            };

        public int? MaxWidth => null;
        public int? MaxHeight => null;
        public int? RequiredWidth => 250;
        public int? RequiredHeight => 250;
        public bool PreferLandscape => false;

        public FilterItem[] FilterList => new FilterItem[]
        {
            FilterItem.Create<CollectibleTypeEntity, string>("Naam", (ent) => ent.Name, FilterType.String),
            FilterItem.Create<CollectibleTypeEntity, CollectibleRarity>("Zeldzaamheid", (ent) => ent.Rarity, FilterType.Enum),
            FilterItem.Create<CollectibleTypeEntity, int>("Punten", (ent) => ent.Points, FilterType.Number),
            FilterItem.Create<CollectibleTypeEntity, string>("Vraag", (ent) => ent.QuestionText, FilterType.String),
            FilterItem.Create<CollectibleTypeEntity, int>("Vraag punten", (ent) => ent.QuestionPoints, FilterType.Number),
            FilterItem.Create<CollectibleTypeEntity, Biotope>("Biotopen", (ent) => ent.Biotopes, FilterType.Enum),
            FilterItem.Create<CollectibleTypeEntity, Biotope>("Aantal biotopen", 
                (ent) => ent.Biotopes, FilterType.EnumCount),
            FilterItem.Create<CollectibleTypeEntity, CalenderMonth>("Maanden", (ent) => ent.MonthsAvailable, FilterType.Enum),
            FilterItem.Create<CollectibleTypeEntity, CalenderMonth>("Aantal maanden", 
                (ent) => ent.MonthsAvailable, FilterType.EnumCount),
            FilterItem.Create<CollectibleTypeEntity, string>("Afbeeldingsnaam", (ent) => ent.TagName, FilterType.String)
        };
        public SortItem[] SortingList => new[]
        {
            SortItem.Create<CollectibleTypeEntity, string>("Naam", (ent) => ent.Name),
            SortItem.Create<CollectibleTypeEntity, CollectibleRarity>("Zeldzaamheid", (ent) => ent.Rarity),
            SortItem.Create<CollectibleTypeEntity, int>("Punten", (ent) => ent.Points),
            SortItem.Create<CollectibleTypeEntity, int>("Vraag punten", (ent) => ent.QuestionPoints),
            SortItem.Create<CollectibleTypeEntity, CalenderMonth>("Eerste maand", 
                (ent) => ent.MonthsAvailable, SortingMethod.EnumLow),
            SortItem.Create<CollectibleTypeEntity, CalenderMonth>("Laatste maand", 
                (ent) => ent.MonthsAvailable, SortingMethod.EnumHigh)
        };

        private readonly SpeurzoekersDbConnection _speurzoekersDbCon;
        private readonly IMapper _mapper;
        private readonly HelperFunctions<Guid> _helper;

        public CollectibleTypeAppMode(SpeurzoekersDbConnection speurzoekersDbCon, IMapper mapper)
        {
            _speurzoekersDbCon = speurzoekersDbCon;
            _mapper = mapper;
            _helper = new HelperFunctions<Guid>();

            EnumDetail[] biotopes = Enum.GetValues(typeof(Biotope))
                .Cast<Biotope>()
                .Select((value, index) =>
                    new EnumDetail<Biotope>(value, value.Description()))
                .ToArray();
            EnumDetail[] months = Enum.GetValues(typeof(CalenderMonth))
                .Cast<CalenderMonth>()
                .Select((value, index) =>
                    new EnumDetail<CalenderMonth>(value, value.Description()))
                .ToArray();

            AvailabilityLists = new EnumDetail[][] { biotopes, months };
        }

        public Task<ListItem[]> GetItems()
        {
            return _helper.GetItemList(_speurzoekersDbCon.CollectibleRepository.GetCollectibleTypeSummary);
        }

        public Task<ListItem[]> GetItems(
            IEnumerable<FilterCriterion> filters,
            bool orConcatenation,
            Sorting sorting)
        {
            return _helper.GetItemList(() =>
                _speurzoekersDbCon.CollectibleRepository
                    .GetCollectibleTypeSummary(filters, orConcatenation, sorting));
        }

        public IBase NewItem()
        {
            return new CollectibleTypeDto(_speurzoekersDbCon, _mapper);
        }

        public async Task<IBase> GetItem(object id)
        {
            CollectibleTypeDto colDto = new CollectibleTypeDto(_speurzoekersDbCon, _mapper);
            CollectibleType col = await _speurzoekersDbCon.CollectibleRepository.FetchCollectibleType((Guid)id);
            _mapper.Map(col, colDto);

            return colDto;
        }

        public bool IsNewItem(object id)
        {
            return id as Guid? == Guid.Empty;
        }
    }
}
