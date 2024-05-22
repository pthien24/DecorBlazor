using Blazored.LocalStorage;
using Decor_Common;
using DecorWeb_client.Service.IService;
using DecorWeb_client.ViewModels;

namespace DecorWeb_client.Service
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorageService;
        public event Action OnChange;
        public CartService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            for (int i = 0; i < cart.Count; i++)
            {

                if (cart[i].ProductId == cartToDecrement.ProductId && cart[i].ProductPriceId == cartToDecrement.ProductPriceId)
                {
                    if (cart[i].Count == 1 || cartToDecrement.Count == 0)
                    {
                        cart.Remove(cart[i]);
                    }else
                    {
                        cart[i].Count -= cartToDecrement.Count;
                    }

                }
            }
            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange?.Invoke();
        }

        public async Task IncrementCart(ShoppingCart cartToadd)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemIncart = false;
            cart ??= new List<ShoppingCart>();

            foreach (var obj in cart)
            {
                if (obj.ProductId == cartToadd.ProductId && obj.ProductPriceId == cartToadd.ProductPriceId)
                {
                    itemIncart = true;
                    obj.Count += cartToadd.Count; 
                }
            }
            if (!itemIncart )
            {
                cart.Add(new ShoppingCart
                {
                    ProductId = cartToadd.ProductId,
                    ProductPriceId = cartToadd.ProductPriceId,
                    Count = cartToadd.Count
                });
            }
            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange?.Invoke();
        }
    }
}
