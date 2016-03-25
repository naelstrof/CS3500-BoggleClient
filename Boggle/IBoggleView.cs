﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public interface IBoggle
    {
        event Action<string,string> ConnectionOpenedEvent;
        event Action<string> WordEvent;
        event Action ExitEvent;
        event Action CancelEvent;
        string Board { set; }
        string Message { set; }
        string Log { set; }
        string Score { set; }
    }
}
