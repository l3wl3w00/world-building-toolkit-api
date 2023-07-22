using AutoMapper;
using WorldBuilderBLL.World.Dto;
using WorldEntity = Dal.Entities.World;

namespace Bll.Mapper;

public class WorldBuilderProfile : Profile
{
    public WorldBuilderProfile()
    {
        CreateMap<CreateWorldDto, WorldEntity>();
        CreateMap<WorldDto, WorldEntity>().ReverseMap();
    }
}