using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Messages.Api
{
    public class InsoleConnectedMessage
    {
        public string MAC { get; }
        public InsoleConnectedMessage(string MAC)
        {
            this.MAC = MAC;
        }
    }
}
