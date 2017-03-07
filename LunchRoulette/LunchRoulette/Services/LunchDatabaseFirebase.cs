using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Newtonsoft.Json;

namespace LunchRoulette.Services
{
    public class LunchDatabaseFirebase : ILunchDatabase
    {
        private string authUrl = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={0}";
        private string firebaseLunchListUrl = "https://lunch-f2cf1.firebaseio.com/lunch.json?auth={0}";
        private string firebaseLunchItemUrl = "https://lunch-f2cf1.firebaseio.com/lunch/{0}.json?auth={1}";
        private HttpClient client;
        private GoogleVerifyPassword googleVerifyPassword;
        private string idToken;
        private bool authenticated = false;
        private DateTime authtime;

        public LunchDatabaseFirebase()
        {
            this.client = new HttpClient();
                        
            Task.Run(Authorize);
        }

        private async Task Authorize()
        {
            var firebaseAuthObject = new { email = Config.GoogleEmail, password = Config.GooglePassword, returnSecureToken = true };
            var content = new StringContent(JsonConvert.SerializeObject(firebaseAuthObject), Encoding.UTF8,"application/json");
            var response = await this.client.PostAsync(string.Format(authUrl, Config.GoogleAuthKey), content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStringAsync();
                this.googleVerifyPassword = JsonConvert.DeserializeObject<GoogleVerifyPassword>(stream);
                idToken = googleVerifyPassword.idToken;
                this.authenticated = true;
                authtime = DateTime.Now;
            }
        }

        public void DropTable()
        {
        }

        public async Task<List<Lunch>> GetItemsAsync()
        {
            if (!this.authenticated) return new List<Lunch>();

            try
            {
                var response = await this.client.GetAsync(string.Format(firebaseLunchListUrl, this.idToken));
                if (response.StatusCode == HttpStatusCode.OK)
                {                    
                    var stream = await response.Content.ReadAsStringAsync();
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Lunch>>(stream);
                    if (dictionary != null)
                    {
                        var list = dictionary.Select(d => d.Value).ToList();
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {                
            }

            return new List<Lunch>();
        }

        public Task<List<Lunch>> GetItemsNotDoneAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Lunch> GetItemAsync(string id)
        {
            var response = await this.client.GetAsync(string.Format(firebaseLunchItemUrl, id, this.idToken));
            throw new NotImplementedException();
        }

        public async Task<int> SaveItemAsync(Lunch lunch)
        {
            if (!this.authenticated)
            {
                return 0;
            }

            if (string.IsNullOrEmpty(lunch.Id))
            {
                lunch.Id = Guid.NewGuid().ToString();
            }

            var content = new StringContent(JsonConvert.SerializeObject(lunch));
        
            var response = await this.client.PatchAsync(new Uri(string.Format(firebaseLunchItemUrl, lunch.Id, this.idToken)), content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return 1;
            }
            return 0;
        }

        public async Task<int> DeleteItemAsync(Lunch lunch)
        {
            if (!this.authenticated) return 0;

            var response = await this.client.DeleteAsync(string.Format(firebaseLunchItemUrl, lunch.Id, this.idToken));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return 1;
            }
            return 0;
        }

        public async Task<int> UpdateItemAsync(Lunch lunch)
        {
            if (!this.authenticated) return 0;

            var content = new StringContent(JsonConvert.SerializeObject(lunch));

            var response = await this.client.PatchAsync(new Uri(string.Format(firebaseLunchItemUrl, lunch.Id, this.idToken)), content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return 1;
            }
            return 0;
        }
    }
}