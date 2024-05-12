using AutoMapper;
using Decor_Business.Repository.IRepository;
using Decor_DataAccess;
using Decor_DataAccess.Data;
using Decor_DataAccess.ViewModel;
using Decor_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _Db;
        private readonly IMapper _Mp;
        public OrderRepository(ApplicationDBContext db,IMapper mp)
        {
            _Db = db;
            _Mp = mp;
        }
        public async Task<OrderDTO> Create(OrderDTO ObjDTO)
        {
            try
            {

                var obj = _Mp.Map<OrderDTO, Order>(ObjDTO);
                _Db.OrderHeader.Add(obj.OrderHeader);
                await _Db.SaveChangesAsync();
                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }
                _Db.OrderDetails.AddRange(obj.OrderDetails);
                await _Db.SaveChangesAsync();
                return new OrderDTO()
                {
                    OrderDetails = _Mp.Map<IEnumerable<OrderDetails>, IEnumerable<OrderDetailsDTO>>(obj.OrderDetails).ToList(),
                    OrderHeader = _Mp.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader)
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ObjDTO   ;
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string status = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeaderDTO> MarkPaymentSuccesfull(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO orderHeader)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
