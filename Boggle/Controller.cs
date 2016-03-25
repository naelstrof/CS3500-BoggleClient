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
            window.WordEvent += HandleWordEvent;
            window.ExitEvent += HandleExitEvent;
            window.CancelEvent += HandleCancelEvent;
            window.JoinGameEvent += HandleJoinGameEvent;
        }
        public async void HandleConnectionOpenedEvent(string server, string nickname)
        {
            model.state.server = server;
            await model.CreateUserASync(nickname);
            window.Message = model.state.status;
        }
        public async void HandleWordEvent( string word )
        {
            await model.PlayWordAsync(word);
            window.Message = model.state.status;
            window.Log = model.state.log;
        }
        public void HandleExitEvent()
        {
            model.CancelJoinRequestAsync();
            window.Close();
        }
        public async void HandleJoinGameEvent()
        {
            window.Message = "Looking for game...";
            await model.JoinGameASync(10);
            window.Message = model.state.status;
        }
        public async void HandleCancelEvent()
        {
            await model.CancelJoinRequestAsync();
            window.Message = model.state.status;
            window.Log = model.state.log;
            window.Score = "Score: 0";
            window.Board = "                ";
        }
    }
}
