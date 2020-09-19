using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Repositories;
using Core.Domain.Validators;
using FluentValidation;

namespace Core.Domain.Services {
    public abstract class CrudService<
        TEntity, TId,
        TRepository,
        TCreateDto, TUpdateDto, TOutDto,
        TCreateValidator, TUpdateValidator
    > : ICrudService<TEntity, TId, TCreateDto, TUpdateDto, TOutDto>
        where TEntity : IEntity<TId>
        where TRepository : ICrudRepository<TEntity, TId>
        where TCreateValidator : CreateValidator<TCreateDto>
        where TUpdateValidator : UpdateValidator<TEntity, TId, TUpdateDto> {
        protected TRepository Repository { get; }
        protected IMapper Mapper { get; }
        protected TCreateValidator CreateValidator { get; }
        protected TUpdateValidator UpdateValidator { get; }

        protected CrudService(
            TRepository repository,
            IMapper mapper,
            TCreateValidator createValidator,
            TUpdateValidator updateValidator
        ) {
            Repository = repository;
            Mapper = mapper;
            CreateValidator = createValidator;
            UpdateValidator = updateValidator;
        }

        public async Task<TOutDto> GetByIdAsync(TId id) {
            var entity = await Repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (entity is null) {
                throw new ObjectNotFoundException();
            }

            return Mapper.Map<TOutDto>(entity);
        }

        public virtual async Task<IList<TOutDto>> GetAllAsync() {
            var entities = await Repository.GetAllAsync().ConfigureAwait(false);
            return Mapper.Map<IList<TOutDto>>(entities);
        }

        public virtual async Task<TOutDto> CreateAsync(TCreateDto inDto) {
            await CreateValidator.ValidateAndThrowAsync(inDto)
                .ConfigureAwait(false);
            var entity = Mapper.Map<TEntity>(inDto);
            await Repository.SaveAsync(entity).ConfigureAwait(false);
            return Mapper.Map<TOutDto>(entity);
        }

        public virtual async Task<TOutDto>
            UpdateAsync(TId id, TUpdateDto inDto) {
            var entity = await Repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (entity is null) {
                throw new ObjectNotFoundException();
            }

            UpdateValidator.EntityToUpdate = entity;
            await UpdateValidator.ValidateAndThrowAsync(inDto)
                .ConfigureAwait(false);

            Mapper.Map(inDto, entity);

            await Repository.UpdateAsync(entity).ConfigureAwait(false);

            return Mapper.Map<TOutDto>(entity);
        }

        public virtual async Task DeleteAsync(TId id) {
            var entity = await Repository.GetByIdAsync(id)
                .ConfigureAwait(false);

            if (entity is null) {
                throw new ObjectNotFoundException();
            }

            await Repository.DeleteAsync(entity).ConfigureAwait(false);
        }
    }
}