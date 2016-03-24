﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    struct Player
    {
        string nickname;
        int score;
    }
    struct GameState
    {
        public int gameID;
        public string server;
        public string nickname;
        public string state;
        public string board;
        public int timeLimit;
        public int timeLeft;
        public string userToken;
        public string status;
        public List<Player> players;
    }
    class BoggleAPI
    {
        public GameState state;
        public BoggleAPI()
        {
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
        async Task<bool> CreateUserASync( GameState foo, string nickname )
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
                    state.status = "Error Joining game: " + response.StatusCode + ", " + response.ReasonPhrase;
                    return false;
                }
            }
        }
    }
}
