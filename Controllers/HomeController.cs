using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStoreScraper.Models;
using OnlineStoreScraper.StoreParsers;

namespace OnlineStoreScraper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductContext _productContext;
        private readonly ProductRepository _productRepository;
        private readonly IParser _parser;
        public HomeController(ILogger<HomeController> logger,
            ProductContext productContext,
            ProductRepository productRepository,
            IParser parser)
        {
            _logger = logger;
            _productContext = productContext;
            _productRepository = productRepository;
            _parser = parser;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult Products()
        {
            var ProductsList = _productRepository.ReadAll();
            return View(ProductsList);
        }

        public IActionResult Delete(int id)
        {
            _productRepository.Delete(id);
            return Redirect("~/Home/Products");
        }

        public IActionResult Product(int id)
        {
            ProductModel product = _productRepository.Read(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductModel productModel)
        {
            _productRepository.Create(productModel);
            return Redirect("~/Home/Products");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            ProductModel productModel = _productRepository.Read(id);
            return View(productModel);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel productModel)
        {
            _productRepository.Update(productModel);
            return Redirect("~/Home/Products");
        }

        [HttpPost]
        public async Task<IActionResult> ReadProductsFromStore(string url)
        {
            var productsList = await  _parser.ParseWebPage(url);
            _productRepository.Create(productsList);
            return Redirect("~/Home/Products");
        }

        [HttpGet]
        public  IActionResult ReadProductsFromStore()
        {
            return View();
        }


       

        
        
    }
}
