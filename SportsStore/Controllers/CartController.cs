using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportsStore.Models.Domain;

namespace SportsStore.Controllers {
    public class CartController : Controller {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        public IActionResult Index() {
            var cart = ReadCartFromSession();
            if (cart.IsEmpty)
                return View("EmptyCart");
            ViewData["Total"] = cart.TotalValue;
            return View(cart.CartLines);
        }

        private Cart ReadCartFromSession() {
            Cart cartWithProductIdsOnly = HttpContext.Session.GetString("cart") == null ?
                new Cart() : JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("cart"));
            Cart cart = new Cart();
            foreach (var l in cartWithProductIdsOnly.CartLines)
            {
                cart.AddLine(_productRepository.GetById(l.Product.ProductId), l.Quantity);
            }
            return cart;
        }

        private void WriteCartToSession(Cart cart) {
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
        }
    }
}