using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models.Domain;
using SportsStore.Tests.Data;
using System.Collections.Generic;
using Xunit;

namespace SportsStore.Tests.Controllers {
    public class CartControllerTest {
        private readonly CartController _controller;
        private readonly Cart _cart;

        public CartControllerTest() {
            var context = new DummyApplicationDbContext();
            var productRepository = new Mock<IProductRepository>();
            _controller = new CartController(productRepository.Object);
            _cart = new Cart();
            _cart.AddLine(context.Football, 2);
        }

        #region Index
        [Fact]
        public void Index_EmptyCart_ShowsEmptyCartView() {
            var emptycart = new Cart();
            var result = Assert.IsType<ViewResult>(_controller.Index(emptycart));
            Assert.Equal("EmptyCart", result.ViewName);
        }

        [Fact]
        public void Index_NonEmptyCart_PassesCartLinesToViewViaModelAndStoresTotalInViewData() {
            var result = Assert.IsType<ViewResult>(_controller.Index(_cart));
            var cartresult = Assert.IsAssignableFrom<IEnumerable<CartLine>>(result.Model);
            Assert.Single(cartresult);
            Assert.Equal(50M, result.ViewData["Total"]);
        }
        #endregion
    }
}