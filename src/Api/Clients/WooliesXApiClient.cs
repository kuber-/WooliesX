using System;
using System.Collections.Generic;
using Api.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Api.Options;
using Newtonsoft.Json;
          
namespace Api.Clients
{
    public interface IWooliesXApiClient
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<IEnumerable<ShoppingHistoryDto>> GetShoppingHistoriesAsync();
    }

    public class WooliesXApiClient : IWooliesXApiClient
    {
        private readonly HttpClient client;
        private readonly string token;

        public WooliesXApiClient(
            IOptions<WooliesXApiClientOptions> options,
            IOptions<ApiOptions> apiOptions)
        {
            token = apiOptions.Value.Token;
            client = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Host)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = Enumerable.Empty<ProductDto>();
            var path = $"/api/resource/products?token={token}";
            var response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }

            return products;
        }

        public async Task<IEnumerable<ShoppingHistoryDto>> GetShoppingHistoriesAsync()
        {
            var shoppingHistories = Enumerable.Empty<ShoppingHistoryDto>();
            var path = $"/api/resource/shopperHistory?token={token}";
            var response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                shoppingHistories = await response.Content.ReadAsAsync<IEnumerable<ShoppingHistoryDto>>();
            }

            return shoppingHistories;
        }
    }
}
