using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public interface IOutputData
    {
        public List<IOutputConnectionData> Connections { get; set; }
    }
}