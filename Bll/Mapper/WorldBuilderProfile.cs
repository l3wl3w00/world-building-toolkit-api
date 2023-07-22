using AutoMapper;
using Dal.Entities;
using WorldBuilderBLL.Auth.Dto;
using WorldBuilderBLL.World.Dto;
using WorldEntity = Dal.Entities.World;

namespace Bll.Mapper;

public class WorldBuilderProfile : Profile
{
    public WorldBuilderProfile()
    {
        World();
        UserIdentity();
    }

    private void UserIdentity()
    {
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserIdentityDto>().ReverseMap();
    }

    private void World()
    {
        CreateMap<CreateWorldDto, WorldEntity>();
        CreateMap<WorldDto, WorldEntity>().ReverseMap();
    }
}