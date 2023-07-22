using Bll.World.Dto;

namespace Bll.World;

public interface IWorldService
{
    Task<ICollection<WorldDto>> GetAll();
    Task<WorldDto> Get(Guid guid);
    Task<WorldDto> Create(CreateWorldDto createWorldDto);
}