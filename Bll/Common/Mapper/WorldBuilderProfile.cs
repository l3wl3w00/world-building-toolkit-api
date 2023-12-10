using AutoMapper;
using Dal.Entities;
using Bll.Auth.Dto;
using Bll.Calendar_.Dto;
using Bll.Continent_.Dto;
using Bll.Event_.Dto;
using Bll.Planet_.Dto;
using Bll.Region_.Dto;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Bll.Common.Mapper;

public class WorldBuilderProfile : Profile
{
    public WorldBuilderProfile()
    {
        Planet();
        Calendar();
        Event();
        Continent();
        Region();
        PlanetCoordinate();
        UserIdentity();
    }

    private void UserIdentity()
    {
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserIdentityDtoWithToken>().ReverseMap();
        CreateMap<User, UserIdentityDto>().ReverseMap();
    }

    private void Planet()
    {
        CreateMap<CreatePlanetDto, Planet>()
            .ForMember(dest => dest.DayTicks, src => src.MapFrom(p => p.DayLength.Ticks));
        CreateMap<PlanetDto, Planet>().ReverseMap();
        CreateMap<PlanetDto, PlanetForAiPromptDto>();
        CreateMap<Planet, PlanetSummaryDto>();
    }
    private void Continent()
    {
        CreateMap<CreateContinentDto, Continent>();
        CreateMap<Continent, ContinentDto>().ReverseMap();
        CreateMap<ContinentDto, ContinentForAiPromptDto>();
        CreateMap<ContinentPatchDto, Continent>()
            .ForMember(dest => dest.Name, opt => opt.PreCondition(src => src.Name is not null))
            .ForMember(dest => dest.Description, opt => opt.PreCondition(src => src.Description is not null))
            .ForMember(dest => dest.Inverted, opt => opt.PreCondition(src => src.Inverted is not null))
            .ReverseMap();
    }

    private void Region()
    {
        CreateMap<CreateRegionDto, Region>();
        CreateMap<RegionPatchDto, Region>();
        CreateMap<Region, RegionDto>()
            .ForMember(dest => dest.Events, opt => opt.MapAtRuntime())
            .ReverseMap();
        CreateMap<RegionDto, RegionForAiPromptDto>();
        CreateMap<PlanetPatchDto, Planet>()
            .ForMember(dest => dest.Name, opt => opt.PreCondition(src => src.Name is not null))
            .ForMember(dest => dest.Description, opt => opt.PreCondition(src => src.Description is not null))
            .ForMember(dest => dest.AntiLandColor, opt => opt.PreCondition(src => src.AntiLandColor is not null))
            .ForMember(dest => dest.LandColor, opt => opt.PreCondition(src => src.LandColor is not null))
            .ReverseMap();
    }

    private void Calendar()
    {
        CreateMap<Calendar, CalendarDto>().ReverseMap();
        CreateMap<CreateCalendarDto, Calendar>();
        CreateMap<RelativeTimeInstance, DateInstanceDto>().ReverseMap();
    }
    
    private void Event()
    {
        CreateMap<CreateHistoricalEventDto, HistoricalEvent>();
        CreateMap<HistoricalEvent, HistoricalEventDto>()
            .ReverseMap()
                .ForMember(dest => dest.Start, opt => opt.Ignore())
                .ForMember(dest => dest.End, opt => opt.Ignore());
    }
    
    private void PlanetCoordinate()
    {
        CreateMap<PlanetCoordinate, PlanetCoordinateDto>().ReverseMap();
    }
}