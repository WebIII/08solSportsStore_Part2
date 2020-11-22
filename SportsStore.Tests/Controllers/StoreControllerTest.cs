using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models.Domain;
using SportsStore.Tests.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Controllers {
    public class StoreControllerTest {
        private readonly StoreController _controller;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly DummyApplicationDbContext _dummyContext;

        public StoreControllerTest() {
            _dummyContext = new DummyApplicationDbContext();
            _productRepository = new Mock<IProductRepository>();
            _controller = new StoreController(_productRepository.Object);
        }

        [Fact]
        public void Index_PassesOnlineProductsToViewViaModel() {
            _productRepository.
                Setup(p => p.GetByAvailability(new List<Availability> { Availability.ShopAndOnline, Availability.OnlineOnly })).
                Returns(_dummyContext.ProductsOnline);
            var result = Assert.IsType<ViewResult>(_storeControlller.Index());
            var model = Assert.IsAssignableFrom <IEnumerable<Product>>( result.Model);
            Assert.Equal(10, model.Count());
        }
    }
}
