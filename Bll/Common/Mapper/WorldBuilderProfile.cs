using AutoMapper;
using Dal.Entities;
using Bll.Auth.Dto;
using Bll.Continent.Dto;
using Bll.Planet.Dto;
using Bll.Region.Dto;

namespace Bll.Common.Mapper;

public class WorldBuilderProfile : Profile
{
    public WorldBuilderProfile()
    {
        Planet();
        Continent();
        Region();
        PlanetCoordinate();
        UserIdentity();
    }

    private void UserIdentity()
    {
        CreateMap<RegisterDto, Dal.Entities.User>();
        CreateMap<Dal.Entities.User, UserIdentityDto>().ReverseMap();
    }

    private void Planet()
    {
        CreateMap<CreatePlanetDto, Dal.Entities.Planet>();
        CreateMap<PlanetDto, Dal.Entities.Planet>().ReverseMap();
        CreateMap<Dal.Entities.Planet, PlanetSummaryDto>();
    }
    private void Continent()
    {
        CreateMap<CreateContinentDto, Dal.Entities.Continent>();
        CreateMap<Dal.Entities.Continent, ContinentDto>().ReverseMap();
        CreateMap<ContinentPatchDto, Dal.Entities.Continent>()
            .ForMember(dest => dest.Name, opt => opt.PreCondition(src => src.Name is not null))
            .ForMember(dest => dest.Description, opt => opt.PreCondition(src => src.Description is not null))
            .ForMember(dest => dest.Inverted, opt => opt.PreCondition(src => src.Inverted is not null))
            .ReverseMap();
    }

    private void Region()
    {
        CreateMap<CreateRegionDto, Dal.Entities.Region>();
        CreateMap<Dal.Entities.Region, RegionDto>().ReverseMap();
        CreateMap<PlanetPatchDto, Dal.Entities.Planet>()
            .ForMember(dest => dest.Name, opt => opt.PreCondition(src => src.Name is not null))
            .ForMember(dest => dest.Description, opt => opt.PreCondition(src => src.Description is not null))
            .ForMember(dest => dest.AntiLandColor, opt => opt.PreCondition(src => src.AntiLandColor is not null))
            .ForMember(dest => dest.LandColor, opt => opt.PreCondition(src => src.LandColor is not null))
            .ReverseMap();
    }
    
    private void PlanetCoordinate()
    {
        CreateMap<PlanetCoordinate, PlanetCoordinateDto>().ReverseMap();
    }
}