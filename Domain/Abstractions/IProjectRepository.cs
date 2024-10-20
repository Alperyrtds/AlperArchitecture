namespace Domain.Abstractions;

public interface IProjectRepository<T>
{
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SoftDeleteAsync(T entity);
    Task<T?> GetByIdAsync(string id);
    Task<T?> GetByEmailAsync(string email);
    Task<IEnumerable<T>> GetAllAsync();

}

