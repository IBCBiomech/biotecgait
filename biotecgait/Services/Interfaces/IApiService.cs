using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biotecgait.Services.Interfaces
{
    public interface IApiService
    {
        public void Scan();
        void Connect(List<string> macs);
        void Disconnect(List<string> macs);
        void Capture();
        void Stop();
        void Pause();
        void Resume();
        float Latency();
    }
}
