using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Controller
    {
        Boggle window;
        BoggleInteractor model;
        public Controller(Boggle win)
        {
            window = win;
            model = new BoggleInteractor();

            window.JoinGameEvent += JoinGameHandler;
            window.CancelGameEvent += CancelGameHandler;
        }

        public void JoinGameHandler()
        {

        }

        public void CancelGameHandler()
        {

        }
    }
}
