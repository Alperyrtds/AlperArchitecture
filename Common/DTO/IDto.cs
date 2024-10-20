using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO;

public interface IDto
{
    bool IsEmpty();
    string InsertSql();
    string UpdateSql();
    string DeleteSql();
    string CacheKey();
    IDto Empty();
}
