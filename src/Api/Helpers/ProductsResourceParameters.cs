using System.ComponentModel.DataAnnotations;

namespace Api.Helpers
{
    public enum ProductSortOptions {
        Low,
        High,
        Ascending,
        Descending,
        Recommended,
    }

    public class ProductsResourceParameters
    {
        [Required]
        public ProductSortOptions? SortOption { get; set; }
    }
}
