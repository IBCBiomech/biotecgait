using biotecgait.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Services.Implementations
{
    public class ApiService : IApiService
    {
        public void Scan()
        {
            Trace.WriteLine("Scan");
        }
    }
}
