using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Entity.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Service.Helper
{
    public static class HttpHelperExtensions
    {
        public static async Task<TOutput> PostAsync<TInput, TOutput>(this HttpClient httpClient, string url, TInput requestBody)
        {
            StringContent requestContent = default;
            if (requestBody is not null)
            {
                var jsonBody = JsonConvert.SerializeObject(requestBody);
                requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            var response = await httpClient.PostAsync(url, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var failedMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(failedMessage);
            }

            var json = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(json))
                return JsonConvert.DeserializeObject<TOutput>(json);

            return default;
        }

        public static async Task<TokenVm> GetAccessTokenAsync(this HttpClient httpClient, AppSettings appSettings)
        {
            LoginDto dto = new()
            {
                UserName = appSettings.SpecialUser.UserName,
                Password = appSettings.SpecialUser.Password
            };
            var token = await httpClient.PostAsync<LoginDto, TokenVm>(appSettings.InvoiceGeneratorHostApi.AccessTokenUrl, dto);
            return token;
        }
    }
}
