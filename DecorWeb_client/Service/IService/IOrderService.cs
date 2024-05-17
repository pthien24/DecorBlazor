using Decor_Models;

namespace DecorWeb_client.Service.IService
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDTO>> GetAll();
        public Task<OrderDTO> Get(int id);
    }
}
