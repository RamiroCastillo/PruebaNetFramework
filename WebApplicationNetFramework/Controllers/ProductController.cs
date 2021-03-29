using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.DataAccess.V1;
using Test.Models.V1.Responses;

namespace WebApplicationNetFramework.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }
        // GET: Product
        public ActionResult Index()
        {

            var products = _product.GetAllProducts();
            var productResponse = (ProductResponse)products.Data;
            var all = productResponse.Products;
            List<SimpleProduct> allProducts = productResponse.Products;
            return View(allProducts);
            //return View();
        }
    }
}