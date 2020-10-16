using System.Collections.Generic;

namespace retecs.ReteCs.Interfaces
{
    public interface IInputData
    {
        public List<IInputConnectionData> Connections { get; set; }
    }
}