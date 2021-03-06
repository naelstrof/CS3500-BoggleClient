﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public interface IBoggleView
    {
        event Action<string,string,string> ConnectionOpenedEvent;
        event Action<string> WordEvent;
        event Action ExitEvent;
        event Action CancelEvent;
        event Action JoinGameEvent;

        string Board { set; }
        string Message { set; }
        string Log { set; }
        string Score { set; }
        string TimeLeft { set; }
    }
}
