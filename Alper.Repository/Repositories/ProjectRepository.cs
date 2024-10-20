using Alper.Domain.Entities;
using Alper.Infrastructure.Models.MssqlContext;
using Alper.Repository.Abstractions;
using Alper.Repository.Cache;
using Microsoft.EntityFrameworkCore;

namespace Alper.Repository.Repositories;

public sealed class ProjectRepository<T>(AlperProjectContext dbContext, ICacheService cacheService)
    : IProjectRepository<T> where T : class, IEntity
{
    public async Task InsertAsync(T entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheService.Remove($"all-{typeof(T).Name}", cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheService.Remove($"all-{typeof(T).Name}", cancellationToken);
        await cacheService.Remove($"id-{entity.Id}", cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheService.Remove($"id-{entity.Id}", cancellationToken);
        await cacheService.Remove($"all-{typeof(T).Name}", cancellationToken);
    }

    public Task SoftDeleteAsync(T entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var cacheKey = $"id-{id}";

        var cachedEntity = await cacheService.Get<T>(cacheKey, cancellationToken);
        if (cachedEntity != null)
        {
            return cachedEntity;
        }

        var entity = await dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);

        if (entity != null)
        {
            await cacheService.Set(cacheKey, entity, cancellationToken);
        }

        return entity;
    }

    public async Task<T?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var cacheKey = $"email-{email}";

        var cachedEntity = await cacheService.Get<T>(cacheKey, cancellationToken);
        if (cachedEntity != null)
        {
            return cachedEntity; 
        }

        var className = typeof(T).Name;
        var tableName = Char.ToLowerInvariant(className[0]) + className.Substring(1);
        tableName = tableName.Insert(3, "_");
        tableName = tableName + "s";

        var sql = $"SELECT * FROM [{tableName}] WHERE [Email] = '{email}'";

        var result = await dbContext.Set<T>().FromSqlRaw(sql).FirstOrDefaultAsync(cancellationToken);

        if (result != null)
        {
            await cacheService.Set(cacheKey, result, cancellationToken);
        }

        return result;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cacheKey = $"all-{typeof(T).Name}";

        var cachedList = await cacheService.Get<IEnumerable<T>>(cacheKey, cancellationToken);
        if (cachedList != null)
        {
            return cachedList;
        }

        var result = await dbContext.Set<T>().ToListAsync(cancellationToken);

        await cacheService.Set(cacheKey, result, cancellationToken);

        return result;
    }
}




