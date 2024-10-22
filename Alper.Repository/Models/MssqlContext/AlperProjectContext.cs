using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Alper.Infrastructure.Models.MssqlContext;

public partial class AlperProjectContext : DbContext, IUnitOfWork
{
    public AlperProjectContext()
    {
    }

    public AlperProjectContext(DbContextOptions<AlperProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUsers> TblUsers { get; set; }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }

    // Rollback metodu, işlemi geri almak için ChangeTracker'ı sıfırlar.
    public Task Rollback(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
        return Task.CompletedTask;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        =>
            optionsBuilder.UseSqlServer("Server=DESKTOP-UMKSOMS\\MSSQLSERVER01;Database=AlperArchitecture;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUsers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_Users");

            entity.ToTable("tbl_Users");

            entity.Property(e => e.Id).HasMaxLength(30);
            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.StartJobDate).HasColumnType("datetime");
            entity.Property(e => e.Surname).HasMaxLength(50);
            entity.Property(e => e.TransactionUser).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

