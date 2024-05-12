using Decor_Models;

namespace DecorWeb_client.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAll();
        public Task<ProductDTO> Get(int id);
    }
}
