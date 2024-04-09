using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.Utils;

public class ApiResponse
{
    public string status { get; set; }
    public string errorMessage { get; set; }
    public object data { get; set; }
    public ApiResponse()
    {

    }
    public ApiResponse(string _status, string _errorMessage, object _data)
    {
        status = _status;
        errorMessage = _errorMessage;
        data = _data;
    }
}
