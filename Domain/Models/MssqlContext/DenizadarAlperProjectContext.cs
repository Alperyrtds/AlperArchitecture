using Microsoft.EntityFrameworkCore;

namespace Domain.Models.MssqlContext;

public partial class DenizadarAlperProjectContext : DbContext
{
    public DenizadarAlperProjectContext()
    {
    }

    public DenizadarAlperProjectContext(DbContextOptions<DenizadarAlperProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => 
            //optionsBuilder.UseSqlServer("Server=sql.bsite.net\\MSSQL2016 ;Initial Catalog=denizadar_alperProject;Persist Security Info=False;User ID=denizadar_alperProject;Password=8520Alper!?1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            optionsBuilder.UseSqlServer("Server=MSI;Database=TestArchitecture;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employees");

            entity.ToTable("tbl_Employees");

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
