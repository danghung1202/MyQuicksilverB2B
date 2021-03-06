using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Cart.Controllers;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using Mediachase.Commerce.Catalog;
using Moq;
using Xunit;

namespace EPiServer.Reference.Commerce.Site.Tests.Features.Cart.Controllers
{
    public class CartControllerTests
    {
        [Fact]
        public void MiniCartDetails_ShouldCreateViewModel()
        {
            _subject.MiniCartDetails();
            _cartViewModelFactoryMock.Verify(s => s.CreateMiniCartViewModel(It.IsAny<ICart>()));
        }

        [Fact]
        public void LargeCart_ShouldCreateViewModel()
        {
            _subject.LargeCart();
            _cartViewModelFactoryMock.Verify(s => s.CreateLargeCartViewModel(It.IsAny<ICart>()));
        }

        [Fact]
        public void ChangeCartItem_WhenChangeQuantity_ShouldCallChangeCartItemOnCartService()
        {
            string code = "Code 1";
            int shipmentId = 1;
            int quantity = 0;
            string size = null;
            string newSize = null;
            _subject.ChangeCartItem(shipmentId, code, quantity, size, newSize);
            _cartServiceMock.Verify(s => s.ChangeCartItem(It.IsAny<ICart>(), shipmentId, code, quantity, size, newSize));
        }

        [Fact]
        public void AddToCart_ShouldCallAddToCartOnCartService()
        {
            string code = "Code 1";
            string warningMessage = null;

            _subject.AddToCart(code);
            _cartServiceMock.Verify(s => s.AddToCart(It.IsAny<ICart>(), code, out warningMessage));
        }

        [Fact]
        public void AddToCart_WhenSuccessfullyAdded_ShouldSaveCart()
        {
            string code = "Code 1";
            _subject.AddToCart(code);
            _orderRepositoryMock.Verify(s => s.Save(It.IsAny<IOrderGroup>()), Times.Once);
        }

        [Fact]
        public void AddToCart_WhenFailedToAdd_ShouldNotSaveCart()
        {
            string code = "Non-existing-code";
            _subject.AddToCart(code);
            _orderRepositoryMock.Verify(s => s.Save(It.IsAny<IOrderGroup>()), Times.Never); 
        }

        private readonly CartController _subject;
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<CartViewModelFactory> _cartViewModelFactoryMock;
        private readonly Mock<ICartServiceB2B> _cartServiceB2BMock;
        private readonly Mock<IQuickOrderService> _quickOrderService;
        private readonly Mock<ReferenceConverter> _referenceConverter;
        private readonly Mock<ICustomerService> _customerService;
        private readonly Mock<IContentLoader> _contentLoader;

        public CartControllerTests()
        {
            string warningMessage = null;
            _cartServiceMock = new Mock<ICartService>();
            _cartServiceB2BMock = new Mock<ICartServiceB2B>();
            _cartViewModelFactoryMock = new Mock<CartViewModelFactory>(null, null, null, null, null);
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _cartServiceMock.Setup(x => x.AddToCart(It.IsAny<ICart>(), "Code 1", out warningMessage)).Returns(true).Verifiable();
            _quickOrderService = new Mock<IQuickOrderService>();
            _referenceConverter = new Mock<ReferenceConverter>();
            _customerService = new Mock<ICustomerService>();
            _contentLoader = new Mock<IContentLoader>();
            _subject = new CartController(_cartServiceMock.Object, _orderRepositoryMock.Object, _cartViewModelFactoryMock.Object, _cartServiceB2BMock.Object, _quickOrderService.Object, _referenceConverter.Object, _customerService.Object, _contentLoader.Object);
        }
    }
}