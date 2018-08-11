using System;
using System.Linq;
using System.Collections.Generic;
using Api.Models;
using Api.Helpers;
using Api.Clients;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Options;
using Api.Options;

namespace Api.Services
{
    public interface IProductsService {
        Task<IEnumerable<ProductDto>> GetSortedProductsAsync(ProductSortOptions sortOption);

        double GetTotal(TrolleyForCalculateDto dto);
    }

    public class ProductsService : IProductsService
    {
        private readonly IWooliesXApiClient client;

        public ProductsService()
        {
        }

        public ProductsService(
            IOptions<ApiOptions> options,
            IWooliesXApiClient client)
        {
            this.client = client;
        }

        public double GetTotal(TrolleyForCalculateDto dto)
        {
            var productPrices = dto.Products.ToDictionary(x => x.Name, x => x.Price);
            var shoppingCart = dto.Quantities.ToDictionary(x => x.Name, x => x.Quantity);
            var total = GetBasePrice(productPrices, shoppingCart);

            foreach(var special in GetValidSpecials(dto.Specials, shoppingCart)) {
                total = Math.Min(total, GetSpecialPrice(special, productPrices, shoppingCart));
            }

            return total;
        }

        private IEnumerable<Special> GetValidSpecials(
            IEnumerable<Special> specials,
            Dictionary<string, int> shoppingCart)
        {
            return specials.Where(x => x.Quantities.All(quantity => 
                 shoppingCart.ContainsKey(quantity.Name) &&
                 shoppingCart[quantity.Name] >= quantity.Quantity));
        }

        private double GetSpecialPrice(
            Special special,
            Dictionary<string, double> productPrices,
            Dictionary<string, int> origShoppingCart)
        {
            var shoppingCart = new Dictionary<string, int>(origShoppingCart.Count);
            var specialDict = special.Quantities.ToDictionary(x => x.Name, x => x.Quantity);

            foreach (var (name, quantity) in origShoppingCart)
            {
                shoppingCart.Add(name, quantity);
            }

            var count = 0;
            while(special.Quantities.TrueForAll(x => 
                shoppingCart.ContainsKey(x.Name) && shoppingCart[x.Name] >= x.Quantity))
            {
                foreach(var (name, quantity) in specialDict) {
                    shoppingCart[name] = shoppingCart[name] - quantity;
                }

                count += 1;
            }

            var total = count * special.Total;

            foreach(var (name, quantity) in shoppingCart) {
                total += productPrices[name] * quantity;
            }

            return total;
        }

        private double GetBasePrice(
            Dictionary<string, double> productPrices,
            Dictionary<string, int> shoppingCart)
        {
            return shoppingCart.Sum(x => productPrices[x.Key] * x.Value);
        }

        public async Task<IEnumerable<ProductDto>> GetSortedProductsAsync(ProductSortOptions sortOption)
        {
            var products = await client.GetProductsAsync();

            if (sortOption != ProductSortOptions.Recommended)
            {
                var orderBy = SortOptionsToOrderBy(sortOption);

                return products.AsQueryable().OrderBy(orderBy);
            }

            var popularProducts = await GetShoppingHistories();
            var productsList = products.ToList(); 

            productsList.Sort((x, y) =>
            {
                double? xValue = null;
                double? yValue = null;

                if (popularProducts.ContainsKey(x.Name)) { xValue = popularProducts[x.Name]; }
                if (popularProducts.ContainsKey(y.Name)) { yValue = popularProducts[y.Name]; }

                if (xValue.HasValue && yValue.HasValue) {
                    return yValue.Value.CompareTo(xValue.Value);
                }

                if (xValue.HasValue) {
                    return -1;
                }

                if (yValue.HasValue) {
                    return 1;
                }

                return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            });

            return productsList;
        }

        private async Task<SortedDictionary<string, double>> GetShoppingHistories()
        {
            var lookup = new SortedDictionary<string, double>();
            var shoppingHistories = await client.GetShoppingHistoriesAsync();

            shoppingHistories
                .SelectMany(x => x.Products).ToList().GroupBy(x => x.Name)
                .Select(x => new KeyValuePair<string, double>(x.Key, x.ToList().Sum(product => product.Quantity))).ToList()
                .ForEach(x => lookup.Add(x.Key, x.Value));

            return lookup;
        }

        private string SortOptionsToOrderBy(ProductSortOptions sortOptions) {
            switch(sortOptions) { 
                case ProductSortOptions.Ascending:
                    return $"{nameof(ProductDto.Name)} ascending";
                case ProductSortOptions.Descending:
                    return $"{nameof(ProductDto.Name)} descending";
                case ProductSortOptions.High:
                    return $"{nameof(ProductDto.Price)} descending";
                case ProductSortOptions.Low:
                    return $"{nameof(ProductDto.Price)} ascending";
                case ProductSortOptions.Recommended:
                    throw new ArgumentException();
            }

            return null;
        }
    }
}
