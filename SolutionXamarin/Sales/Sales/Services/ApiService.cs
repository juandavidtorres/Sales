

namespace Sales.Services
{

    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Common.Models;
    using Sales.Helpers;
    public class ApiService
    {

        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet
                };
            }

            return new Response
            {
                IsSuccess = true,
            };
        }
        public async Task<Response> GetList<T>(string UrlBase, string Prefix, string Controller)
        {
            try
            {

                //1. Crear un cliente HTTP Client
                var client = new HttpClient();
                client.BaseAddress = new System.Uri(UrlBase);
                var url = $"{Prefix}{Controller}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response()
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }
                else
                {
                    var list = JsonConvert.DeserializeObject<List<T>>(answer);
                    return new Response()
                    {
                        IsSuccess = true,
                        Result = list
                    };
                }
            }
            catch (System.Exception ex)
            {

                return new Response() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
