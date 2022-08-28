using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Common.Http
{

    public class ApiResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
    public class ApiResponse<T>
    {
        public string status { get; set; }
        public string message { get; set; }
        public T result { get; set; }
    }
}
