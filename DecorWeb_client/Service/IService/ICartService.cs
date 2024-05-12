using DecorWeb_client.ViewModels;

namespace DecorWeb_client.Service.IService
{
    public interface ICartService
    {
        public event Action OnChange;
        Task DecrementCart(ShoppingCart shoppingCart);
        Task IncrementCart(ShoppingCart shoppingCart);
    }
}
