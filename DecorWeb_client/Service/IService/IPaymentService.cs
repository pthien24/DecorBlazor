using Decor_Models;

namespace DecorWeb_client.Service.IService
{
    public interface IPaymentService
    {
        public Task<SuccessModelDTO> CheckOut(StripePaymentDTO stripePaymentDTO);

    }
}
