using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Messages
{
    public class InsolesScanMessage
    {
        public List<InsoleScanData> Insoles { get; }
        public InsolesScanMessage(List<InsoleScanData> insoles)
        {
            Insoles = insoles;
        }
    }
}
