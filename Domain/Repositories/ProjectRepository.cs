using Domain.Abstractions;
using Domain.Models.MssqlContext;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public sealed class ProjectRepository<T> : IProjectRepository<T> where T: class
{
    private readonly DenizadarAlperProjectContext _dbContext;

    public ProjectRepository(DenizadarAlperProjectContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task InsertAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task SoftDeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetByEmailAsync(string email)
    {
        var className = typeof(T).Name;

        var tableName = Char.ToLowerInvariant(className[0]) + className.Substring(1);

        tableName = tableName.Insert(3, "_");

        tableName = tableName + "s";

        var sql = $"SELECT * FROM [{tableName}] WHERE [Email] = '{email}'";

        var result = await _dbContext.Set<T>().FromSqlRaw(sql).FirstOrDefaultAsync();

        return result;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }
}

