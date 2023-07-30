using AutoMapper;
using Dal.Entities;
using Bll.Auth.Dto;
using Bll.World.Dto;
using WorldEntity = Dal.Entities.World;

namespace Bll.Common.Mapper;

public class WorldBuilderProfile : Profile
{
    public WorldBuilderProfile()
    {
        World();
        UserIdentity();
    }

    private void UserIdentity()
    {
        CreateMap<RegisterDto, Dal.Entities.User>();
        CreateMap<Dal.Entities.User, UserIdentityDto>().ReverseMap();
    }

    private void World()
    {
        CreateMap<CreateWorldDto, WorldEntity>();
        CreateMap<WorldDto, WorldEntity>().ReverseMap();
    }
}