using Moto_Web.Models;
using Newtonsoft.Json;

namespace Moto_Web.Services
{
    public class ServiceTest
    {
        HttpClient client;
        Uri baseAddress = new Uri("https://localhost:7001/");

        public ServiceTest( )
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public async Task<List<VehicleImages>> GetImagesAdId(int id)
        {
            //await SetAuthToken();
            var response = await client.GetStringAsync(client.BaseAddress + "/api/AdMauiImages/" + id);
            return JsonConvert.DeserializeObject<List<VehicleImages>>(response);
        }
    }
}
