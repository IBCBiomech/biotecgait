using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Messages.Api
{
    public class InsoleDisconnectedMessage
    {
        public string MAC { get; }
        public InsoleDisconnectedMessage(string MAC)
        {
            this.MAC = MAC;
        }
    }
}
