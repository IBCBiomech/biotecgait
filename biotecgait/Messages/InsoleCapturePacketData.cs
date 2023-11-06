using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Messages
{
    public class InsoleCapturePacketData
    {
        public byte handler { get; }
        public List<InsoleCaptureData> data { get; }
        public InsoleCapturePacketData(byte handler, List<InsoleCaptureData> data)
        {
            this.handler = handler;
            this.data = data;
        }
    }
}
