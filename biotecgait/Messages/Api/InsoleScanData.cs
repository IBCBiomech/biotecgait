using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Messages.Api
{
    public class InsoleScanData
    {
        public string name { get; }
        public string MAC { get; }
        public InsoleScanData(string name, string MAC)
        {
            this.name = name;
            this.MAC = MAC;
        }
    }
}
