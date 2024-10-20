namespace Domain.Models;

public partial class TblEmployee
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? StartJobDate { get; set; }

    public string? TransactionUser { get; set; }
    public int Status { get; set; }
}
