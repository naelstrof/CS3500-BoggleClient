using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Controller
    {
        private Timer time = new Timer();
        Boggle window;
        BoggleInteractor model;
        public Controller(Boggle win)
        {
            time.Tick += new EventHandler(HandleUpdateEvent);
            time.Interval = 1000;
            window = win;
            model = new BoggleInteractor();
            window.ConnectionOpenedEvent += HandleConnectionOpenedEvent;
            window.WordEvent += HandleWordEvent;
            window.ExitEvent += HandleExitEvent;
            window.CancelEvent += HandleCancelEvent;
            window.JoinGameEvent += HandleJoinGameEvent;
        }
        public async void HandleConnectionOpenedEvent(string server, string nickname, string time)
        {
            model.state.server = server;
            model.state.timeLimit = Convert.ToInt32(time);
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
            await model.JoinGameASync(model.state.timeLimit);
            time.Start();
            window.Message = model.state.status;
            HandleUpdateEvent(null, null);
        }
        public async void HandleUpdateEvent( object sender, EventArgs e )
        {
            try {
                await model.GameStatusAsync();
            } catch( Exception ex)
            {
                window.Message = ex.Message;
                return;
            }
            window.Log = model.state.log;
            window.Score = ""+model.state.score;
            window.Board = model.state.board;
            window.Message = model.state.status;
            window.TimeLeft = "" + model.state.timeLeft + "/" + model.state.timeLimit;
            if (model.state.state == "completed")
            {
                time.Stop();
                model.PrintEnd();
                window.Log = model.state.log;
            }
        }
        public async void HandleCancelEvent()
        {
            await model.CancelJoinRequestAsync();
            window.Message = model.state.status;
            window.Log = model.state.log;
            window.Score = "0";
            window.TimeLeft = "0";
            window.Log = "";
            window.Board = "                ";
            time.Stop();
        }
    }
}
