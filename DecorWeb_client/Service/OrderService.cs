using Decor_Models;
using DecorWeb_client.Service.IService;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<OrderDTO> Create(StripePaymentDTO Paymentdto)
        {
            var content = JsonConvert.SerializeObject(Paymentdto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Order/Create", bodyContent);

            string responseResult = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Response order : " + responseResult);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<OrderDTO>(responseResult);
                Console.WriteLine("result : " + result);

                return result;
            }
            return new OrderDTO();
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

        public async Task<OrderHeaderDTO> MarkPaymentSuccessfully(OrderHeaderDTO orderHeader)
        {
            var content = JsonConvert.SerializeObject(orderHeader);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Order/PaymentSuccessfully", bodyContent);

            string responseResult = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Response PaymentSuccessfully : " + responseResult);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<OrderHeaderDTO>(responseResult);
                Console.WriteLine("result PaymentSuccessfully : " + result);

                return result;
            }
            var errModel = JsonConvert.DeserializeObject<ErrorModelDTO>(responseResult);
            throw new Exception(errModel.ErrorMessage);
        }
    }
}
