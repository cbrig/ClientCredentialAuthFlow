using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BFF
{
    public class ComponentService : IComponentService
    {
        private readonly HttpClient _client;

        public ComponentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<string>> GetValues()
        {
            var response = await _client.GetAsync("api/values");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<string>>(json);
                return values;
            }
            else
            {
                return null;
            }
        }
    }
}