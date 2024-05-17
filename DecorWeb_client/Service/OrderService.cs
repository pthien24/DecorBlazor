using Decor_Models;
using DecorWeb_client.Service.IService;
using Newtonsoft.Json;

namespace DecorWeb_client.Service
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string BaseServerURL;
        public OrderService(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerURL = _configuration.GetSection("BaseServerUrl").Value;
        }
        public async Task<OrderDTO> Get(int id)
        {
            var respone = await _httpClient.GetAsync($"/api/order/{id}");
            var context = await respone.Content.ReadAsStringAsync();

            if (respone.IsSuccessStatusCode)
            {
                var order = JsonConvert.DeserializeObject<OrderDTO>(context);
                return order;
            }
            else
            {
                var errModel = JsonConvert.DeserializeObject<ErrorModelDTO>(context);
                throw new Exception(errModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAll()
        {
            var respone = await _httpClient.GetAsync("/api/order");
            if (respone.IsSuccessStatusCode)
            {
                var context = await respone.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(context);
                return order;
            }
            return new List<OrderDTO>();
        }
    }
}
