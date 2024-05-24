using Decor_Models;
using DecorWeb_client.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace DecorWeb_client.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        public PaymentService(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        public async Task<SuccessModelDTO> CheckOut(StripePaymentDTO stripePaymentDTO)
        {
            try
            {
                var content = JsonConvert.SerializeObject(stripePaymentDTO);
                var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/stripepayment/create", bodyContent);
                Console.WriteLine("Response1 : " + response);
                string responseResult = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Response  stripe: " + responseResult);
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<SuccessModelDTO>(responseResult);
                    Console.WriteLine("result : " + result);
                    return result;
                }
                else
                {
                    var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(responseResult);
                    Console.WriteLine("errorModel : " + errorModel);
                    throw new Exception(errorModel.ErrorMessage);
                }
            }
            catch (Exception ex )
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
