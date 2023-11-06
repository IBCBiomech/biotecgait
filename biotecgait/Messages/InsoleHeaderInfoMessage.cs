using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace biotecgait.Messages
{
    public class InsoleHeaderInfoMessage
    {
        public string MAC { get; }
        public string fwVersion { get; }
        public byte battery { get; }
        public InsoleHeaderInfoMessage(string MAC, string fwVersion, byte battery) 
        { 
            this.MAC = MAC;
            this.fwVersion = fwVersion;
            this.battery = battery;
        }
    }
}
