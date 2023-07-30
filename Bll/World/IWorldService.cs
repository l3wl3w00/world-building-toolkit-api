using Bll.World.Dto;

namespace Bll.World;

public interface IWorldService
{
    Task<ICollection<WorldSummaryDto>> GetAll(Dal.Entities.User user);
    Task<WorldDto> Get(Guid guid);
    Task<WorldDto> Create(CreateWorldDto createWorldDto);
}