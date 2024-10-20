namespace Alper.Repository.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task Commit(CancellationToken cancellationToken);
    Task Rollback(CancellationToken cancellationToken = default);

}
