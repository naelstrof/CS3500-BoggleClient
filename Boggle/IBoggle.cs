using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public interface IBoggle
    {
        event Action<string> ConnectionOpenedEvent;
        event Action<string> GameOpenedEvent;
    }
}
