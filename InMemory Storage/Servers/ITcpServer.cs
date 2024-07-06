using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Server
{
    public interface ITcpServer
    {
        public void Start();
        public void Stop();
    }
}
