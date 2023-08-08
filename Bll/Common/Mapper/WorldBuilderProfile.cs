using AutoMapper;
using Dal.Entities;
using Bll.Auth.Dto;
using Bll.Continent.Dto;
using Bll.Planet.Dto;

namespace Bll.Common.Mapper;

public class WorldBuilderProfile : Profile
{
    public WorldBuilderProfile()
    {
        Planet();
        Continent();
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
        CreateMap<Dal.Entities.Continent, ContinentDto>();
    }
    
    private void PlanetCoordinate()
    {
        CreateMap<PlanetCoordinate, PlanetCoordinateDto>().ReverseMap();
    }
}