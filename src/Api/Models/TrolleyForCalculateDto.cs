using System.Collections.Generic;

namespace Api.Models
{
    public class TrolleyForCalculateDto
    {
        public List<TrolleyProduct> Products { get; set; }
        public List<Special> Specials { get; set; }
        public IEnumerable<ProductQuantity> Quantities { get; set; }
    }

    public class TrolleyProduct
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ProductQuantity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class Special
    {
        public List<ProductQuantity> Quantities { get; set; }
        public double Total { get; set; }
    }
}
