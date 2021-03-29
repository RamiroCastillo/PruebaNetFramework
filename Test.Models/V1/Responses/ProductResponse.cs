using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Models.V1.Responses
{
    public class ProductResponse
    {
        public List<SimpleProduct> Products { get; set; }
        public ProductResponse()
        {
            Products = new List<SimpleProduct>();
        }
    }

    public class SimpleProduct
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public float Cost { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
