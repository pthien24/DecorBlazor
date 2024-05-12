using Decor_Models;
using DecorWeb_client.Service.IService;
using Newtonsoft.Json;
namespace DecorWeb_client.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ProductDTO> Get(int id)
        {
            var respone = await _httpClient.GetAsync($"/api/product/{id}");
            var context = await respone.Content.ReadAsStringAsync();

            if (respone.IsSuccessStatusCode)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(context);
                return product;
            }else
            {
                var errModel = JsonConvert.DeserializeObject<ErrorModelDTO>(context);
                throw new Exception(errModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var respone = await _httpClient.GetAsync("/api/product");
            if (respone.IsSuccessStatusCode)
            {
                var context = await respone.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(context);
                return products;
            }
            return new List<ProductDTO>();
        }
    }
}
