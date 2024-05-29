using Decor_DataAccess;
using Decor_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Business.Repository.IRepository
{
    public interface IOrderRepository
    {
        public Task<OrderDTO> Get(int id);
        public Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null);
        public Task<OrderDTO> Create(OrderDTO OjbDTO);

        public Task<int> Delete(int id);
        public Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO orderHeader);
        public Task<OrderHeaderDTO> CancelOrder(int orderId);
        public Task<OrderHeaderDTO> MarkPaymentSuccesfull(int Id, string payMentID);
        public Task<bool> UpdateOrderStatus(int orderId,string status);

    }
}
