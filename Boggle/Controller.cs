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
            window.ConnectionOpenedEvent += HandleConnectionOpenedEvent;
        }
        public async void HandleConnectionOpenedEvent(string server, string nickname)
        {
            model.state.server = server;
            await model.CreateUserASync(nickname);
            window.Message = model.state.status;
        }
    }
}
