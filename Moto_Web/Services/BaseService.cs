using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static Moto_Utility.SD;

namespace Moto_Web.Services
{
    public class BaseService : IBaseService
    {
        //generic method gdzie wywolujemy API endpoint

        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }


        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MotoAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*"); //any media type/subtype                   
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                }
                //          

                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach (var prop in apiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(apiRequest.Data);
                        if (value is List<IFormFile>)
                        {
                            var file = (List<IFormFile>)value;
                            if (file != null)
                            {
                                foreach (var img in file)
                                {
                                    content.Add(new StreamContent(img.OpenReadStream()), prop.Name, img.FileName);
                                }
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (apiRequest.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                                                            Encoding.UTF8, "application/json");
                    }
                }

                        switch (apiRequest.ApiType)
                        {
                            case SD.ApiType.POST:
                                message.Method = HttpMethod.Post;
                                break;

                            case SD.ApiType.PUT:
                                message.Method = HttpMethod.Put;
                                break;

                            case SD.ApiType.DELETE:
                                message.Method = HttpMethod.Delete;
                                break;

                            default:
                                message.Method = HttpMethod.Get;
                                break;
                        }

                        HttpResponseMessage apiResponse = null;

                        if (!string.IsNullOrEmpty(apiRequest.Token))
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                        }

                        apiResponse = await client.SendAsync(message);
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                        return APIResponse;
                    }
            
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;

            }
        }
    }
}
