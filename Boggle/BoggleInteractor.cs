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
    class BoggleAPI
    {
        public string playerName { set; get; }
        public string playerID { set; get; }
        public string gameID { set; get; }
        public double score { set; get; }
        public string server { set; get; }

        public BoggleAPI()
        {
            server = "http://bogglecs3500s16.azurewebsites.net/";
            playerName = "Loque";
        }
        public HttpClient CreateClient()
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(this.server);
            return client;
        }
        async Task<int> CreateUserASync()
        {
            using (HttpClient client = CreateClient())
            {
                dynamic data = new ExpandoObject();
                data.Nickname = playerName;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/BoggleService.svc/users", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic player = JsonConvert.DeserializeObject(result);
                    playerID = player.UserToken;
                    return (int)response.StatusCode;
                }
                else
                {
                    throw new Exception("Error creating user: " + response.StatusCode + ", " + response.ReasonPhrase);
                }
            }
        }
    }
}