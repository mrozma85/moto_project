using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;
using System.Collections.Generic;
using static System.Net.WebRequestMethods;

namespace Moto_Web.Services
{
    public class AdService : BaseService, IAdService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public AdService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> CreateAsyncNew<T>(AdCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = adUrl + "/api/AdApi",
                Token = token,
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public Task<T> CreateAsync<T>(AdCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = adUrl + "/api/AdApi",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/AdApi/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdApi",
                Token = token
            });
        }

        public Task<T> GetAsyncByUser<T>(string user, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdAPI?filterDlaUser=" + user,
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdApi/" + id,
                Token = token
            });
        }

        public Task<T> GetByName<T>(string search, 
                                    float priceStart, float priceEnd, 
                                    int rokProdukcjiStart, int rokProdukcjiEnd,
                                    int pojemnoscStart,
                                    int pojemnoscEnd,
                                    string searchLocation, 
                                    string token)
        {
            
            //fiter bez nazwa + cend od
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart,
                    Token = token
                });
            }
            //fiter bez nazwa + cend do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd,
                    Token = token
                });
            }
            //fiter tylko nazwa
            if (search != null && priceStart == 0 && priceEnd == 0 
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                        Url = adUrl + "/api/AdApi?filterNazwa=" + search,
                        Token = token
                });
            }
            //fiter bez nazwa + cend od + do
            if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd,
                    Token = token
                });
            }

            //fiter nazwa + cend od + do
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa="+ search + "&" + "priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd,
                    Token = token
                });
            }


            //fiter nazwa + cend od
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd,
                    Token = token
                });
            }
            //fiter nazwa + cend do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd,
                    Token = token
                });
            }
            //fiter nazwa + cena od + pojemnosc od
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart,
                    Token = token
                });
            }
            //fiter nazwa + cena do + pojemnosc od
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart >= 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart,
                    Token = token
                });
            }
            //fiter nazwa + cena od + pojemnosc do
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            //fiter nazwa + cena do + pojemnosc od
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            //fiter bez nazwa + cena od + pojemnosc od
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart >= 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart,
                    Token = token
                });
            }
            //fiter bez nazwa + cena od + pojemnosc do
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            //fiter bez nazwa + cena do + pojemnosc od
            if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart >= 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            //fiter bez nazwa + cena do + pojemnosc od
            if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart >= 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart,
                    Token = token
                });
            }
            //fiter bez nazwa + rok produkcji od
            if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            //fiter bez nazwa + rok produkcji do
            if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //fiter bez nazwa + rok od + rok do
            if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //fiter nazwa + rok produkcji od
            if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    ////
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            //fiter  nazwa + rok produkcji do
            if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //fiter nazwa + rok od + rok do
            if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart ==0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena od + poj od + rok od
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // nazwa cena od + poj od + rok do
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena od + poj od + rok od/do
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena od + poj od + rok od
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // bez nazwa cena od + poj od + rok do
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }

            // bez nazwa cena od + poj od + rok od/do
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }

            // bez nazwa cena do + poj od + rok od
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // bez nazwa cena do + poj od + rok do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena do + rok od/do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena do + poj od + rok od/do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena do + poj od + rok od
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // nazwa cena do + poj od + rok do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena do + poj od + rok od/do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena do + poj do + rok od
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // nazwa cena do + poj do + rok do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //bez nazwa cena od/do + rok od/do
            if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?pricestart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //nazwa cena od/do + rok od/do
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "pricestart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //nazwa cena od/do + rok od
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "pricestart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            //nazwa cena od/do + rok do
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "pricestart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            //bez nazwa cena od/do + rok od
            if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?pricestart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            //beez nazwa cena od/do + rok do
            if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?pricestart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena do + poj do + rok od/do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena do + poj do + rok od
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // bez nazwa cena do + poj do + rok do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena do + poj do + rok od/do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena od + rok od
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // bez nazwa cena od + rok do
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena od + rok od/do
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena od + rok od
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // nazwa cena od + rok do
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena od + rok od/do
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena do + rok od
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // bez nazwa cena do + rok do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena do + rok od/do
            if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena do + rok od
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart,
                    Token = token
                });
            }
            // nazwa cena do + rok do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // nazwa cena do + rok od/do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa cena od + poj do + rok od/do
            if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "pojemnoscEnd=" + pojemnoscEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd,
                    Token = token
                });
            }
            // bez nazwa poj od
            if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?pojemnoscStart=" + pojemnoscStart,
                    Token = token
                });
            }
            // bez nazwa poj do
            if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // bez nazwa poj od do
            if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa poj od
            if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "pojemnoscStart=" + pojemnoscStart,
                    Token = token
                });
            }
            // nazwa poj do
            if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa poj od do
            if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa cnea od poj od do
            if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa cnea do poj od do
            if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa cena od do poj od do
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa cena od do poj od do rok od
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa cena od do poj od do rok do
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            // nazwa cena od do poj od do rok od do
            if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0
                               && searchLocation == null)
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi?filterNazwa=" + search + "&" + "priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd + "&" + "rokProdukcjiStart=" + rokProdukcjiStart + "&" + "rokProdukcjiEnd=" + rokProdukcjiEnd + "&" + "pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                    Token = token
                });
            }
            else
            {
                return SendAsync<T>(new APIRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = adUrl + "/api/AdApi",
                    Token = token
                });
            }

        }

        public Task<T> GetByRangePrice<T>(float priceStart, float priceEnd, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdApi?priceStart=" + priceStart + "&" + "priceEnd=" + priceEnd,
                Token = token
            });
        }

        public Task<T> GetByPojemnosc<T>(int pojemnoscStart, int pojemnoscEnd, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdApi?pojemnoscStart=" + pojemnoscStart + "&" + "pojemnoscEnd=" + pojemnoscEnd,
                Token = token
            });
        }

        public Task<T> GetByIdAdType<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdApi/?filterAdType=" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(AdUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = adUrl + "/api/AdApi/" + dto.Id,
                Token = token
            });
        }
    }
}
