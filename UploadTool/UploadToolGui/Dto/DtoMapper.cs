using AutoMapper;
using UploadToolGui.Dto;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Avatar;
using Speurzoekers.Common.Domain.Collectible;
using System.Collections.Generic;
using System.Linq;

namespace CollectibleUploadToolCon
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<CollectibleTypeChoice, QuestionChoice>();

            CreateMap<ActivityDto, Activity>()
                .ForMember(dest => dest.TagName1, opt => opt.MapFrom(src => src.TagNames[0]))
                .ForMember(dest => dest.TagName2, opt => opt.MapFrom(src => src.TagNames[1]))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Texts[0].Text));
            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => new[] { src.TagName1, src.TagName2 }))
                .ForMember(dest => dest.Texts, opt => opt.MapFrom(src => new List<DescriptionText>(1) 
                { 
                    new DescriptionText
                    {
                        Text = src.Text
                    }
                }));

            CreateMap<AvatarTypeDto, AvatarType>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.TagNames[0]))
                .ForMember(dest => dest.AvatarTypeTexts, opt => opt.MapFrom(src => src.Texts
                    .Select(t => new AvatarTypeText
                    {
                        AvatarTypeIndex = src.Id,
                        Id = t.Id,
                        TextId = t.TextId,
                        Text = t.Text
                    }
                )));
            CreateMap<AvatarType, AvatarTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Index))
                .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => new[] { src.TagName }))
                .ForMember(dest => dest.Texts, opt => opt.MapFrom(src => src.AvatarTypeTexts));

            CreateMap<CollectibleTypeDto, CollectibleType>()
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.TagNames[0]))
                .ForMember(dest => dest.CollectibleTypeChoices, opt => opt.MapFrom(src => src.QuestionChoices
                    .Select(q => new CollectibleTypeChoice
                        {
                            CollectibleTypeId = src.Id,
                            Id = q.Id,
                            IsAnswer = q.IsAnswer,
                            Text = q.Text
                        }
                )))
                .ForMember(dest => dest.CollectibleTypeTexts, opt => opt.MapFrom(src => src.Texts
                    .Select(t => new CollectibleTypeText
                        {
                            CollectibleTypeId = src.Id,
                            Id = t.Id,
                            TextId = t.TextId,
                            Text = t.Text
                        }
                )));
            CreateMap<CollectibleType, CollectibleTypeDto>()
                .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => new[] { src.TagName }))
                .ForMember(dest => dest.QuestionChoices, opt => opt.MapFrom(src => src.CollectibleTypeChoices))
                .ForMember(dest => dest.Texts, opt => opt.MapFrom(src => src.CollectibleTypeTexts));
        }
    }
}
