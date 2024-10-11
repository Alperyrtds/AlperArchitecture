namespace Application.Commands;

public record AlperCmd
{
    public string IslemYapanKullanici;

    protected AlperCmd()
    {
        IslemYapanKullanici = string.Empty;
    }
}

