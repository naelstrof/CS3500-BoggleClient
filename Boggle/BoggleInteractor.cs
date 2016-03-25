using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Word
    {
        public int score;
        public string word;
        public Word( string w, int s )
        {
            score = s;
            word = w;
        }
    }
    class Player
    {
        public string nickname;
        public int score;
        public List<Word> words;
        public Player( string name, int s )
        {
            nickname = name;
            score = s;
            words = new List<Word>();
        }
        public void addWord( string word, int score )
        {
            words.Add(new Word( word, score ));
        }
    }
    class GameState
        {
        public int gameID;
        public string server;
        public string nickname;
        public string board;
        public string state;
        public int timeLimit;
        public int timeLeft;
        public int score;
        public string log;
        public string userToken;
        public string status;
        public List<Player> players;
        public GameState()
        {
            server = "http://bogglecs3500s16.azurewebsites.net/";
            nickname = "Loque";
            status = "OK.";
            gameID = 0;
            state = "";
            log = "";
            board = "";
            timeLimit = 0;
            timeLeft = 0;
            userToken = "";
            players = new List<Player>();
        }
    }
    class BoggleInteractor
    {
        public GameState state;
        public BoggleInteractor()
        {
            state = new GameState();
            state.server = "http://bogglecs3500s16.azurewebsites.net/";
            state.nickname = "Loque";
            state.status = "OK.";
        }
        public HttpClient CreateClient()
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(this.state.server);
            return client;
        }
        async Task<bool> CreateUserASync( string nickname )
        {
            using (HttpClient client = CreateClient())
            {
                dynamic data = new ExpandoObject();
                data.Nickname = nickname;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/BoggleService.svc/users", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic player = JsonConvert.DeserializeObject(result);
                    state.userToken = player.UserToken;
                    state.status = "Created new user " + state.nickname + " successfully!";
                    return true;
                }
                else
                {
                    state.status = "Error creating user: " + response.StatusCode + ", " + response.ReasonPhrase;
                    return false;
                }
            }
        }
        async Task<bool> JoinGameASync( int timeLimit )
        {
            using (HttpClient client = CreateClient())
            {
                dynamic data = new ExpandoObject();
                data.UserToken = state.userToken;
                data.TimeLimit = timeLimit;

                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/BoggleService.svc/games", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic game = JsonConvert.DeserializeObject(result);
                    state.gameID = game.GameID;
                    state.status = "Joined game " + state.gameID + " successfully.";
                    return true;
                }
                else
                {
                    state.status = "Error joining game: " + response.StatusCode + ", " + response.ReasonPhrase;
                    return false;
                }
            }
        }

        async Task<bool> CancelJoinRequestAsync()
        {
            using (HttpClient client = CreateClient())
            {
                dynamic data = new ExpandoObject();
                data.UserToken = state.userToken;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("/BoggleService.svc/games", content);
                if (response.IsSuccessStatusCode)
                {
                    state = new GameState();
                    return true;
                }
                state.status = "Error puting cancel: " + response.StatusCode + ", " + response.ReasonPhrase;
                return false;
            }
        }

        async Task<bool> PlayWordAsync(string word)
        {
            using (HttpClient client = CreateClient())
            {
                dynamic data = new ExpandoObject();
                data.UserToken = state.userToken;
                data.Word = word;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("/BoggleService.svc/games/" + state.gameID, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic result2 = JsonConvert.DeserializeObject(result);
                    state.score += result2.Score;
                    state.log = state.log + word + "(Score: " + result2.Score + ")\n";
                    return true;
                }
                state.status = "Error puting word: " + response.StatusCode + ", " + response.ReasonPhrase;
                return false;
            }
        }
        async Task<bool> GameStatusAsync()
        {
            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.GetAsync("/BoggleService.svc/games/" + state.gameID);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic gamestate = JsonConvert.DeserializeObject(result);
                    state.state = gamestate.GameState;
                    state.timeLeft = gamestate.TimeLeft;
                    state.board = gamestate.Board;
                    state.timeLimit = gamestate.TimeLimit;
                    Player p1 = new Player(gamestate.Player1.Nickname, gamestate.Player1.Score);
                    Player p2 = new Player(gamestate.Player2.Nickname, gamestate.Player2.Score);
                    if (state.state == "completed")
                    {
                        foreach (dynamic foo in gamestate.Player1.WordsPlayed)
                        {
                            p1.addWord(foo.Word, foo.Score);
                        }
                        foreach (dynamic foo in gamestate.Player2.WordsPlayed)
                        {
                            p2.addWord(foo.Word, foo.Score);
                        }
                    }
                    state.players = new List<Player>();
                    state.players.Add(p1);
                    state.players.Add(p2);
                    return true;
                }
                state.status = "Error getting game status: " + response.StatusCode + ", " + response.ReasonPhrase;
                return false;
            }
        }
    } 
}
