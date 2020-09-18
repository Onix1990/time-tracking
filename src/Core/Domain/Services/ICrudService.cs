using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Domain.Services {
    public interface ICrudService<
        TEntity, in TId,
        in TCreateDto, in TUpdateDto, TOutDto
    > where TEntity : IEntity<TId> {
        Task<TOutDto> GetByIdAsync(TId id);
        Task<IList<TOutDto>> GetAllAsync();
        Task<TOutDto> CreateAsync(TCreateDto inDto);
        Task<TOutDto> UpdateAsync(TId id, TUpdateDto inDto);
        Task DeleteAsync(TId id);
    }
}