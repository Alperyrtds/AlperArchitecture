using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public record AlperCmd
{
    public string IslemYapanKullanici;

    protected AlperCmd()
    {
        IslemYapanKullanici = string.Empty;
    }
}

