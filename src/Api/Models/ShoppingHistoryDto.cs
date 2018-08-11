using System.Collections.Generic;

namespace Api.Models
{
    public class ShoppingHistoryDto
    {
        public int CustomerId { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
    }
}
