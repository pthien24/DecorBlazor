using AutoMapper;
using Decor_Business.Repository.IRepository;
using Decor_Common;
using Decor_DataAccess;
using Decor_DataAccess.Data;
using Decor_DataAccess.ViewModel;
using Decor_Models;
using Microsoft.EntityFrameworkCore;
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
            return ObjDTO;
        }

        public async Task<int> Delete(int id)
        {
            var objHeader = _Db.OrderHeader.FirstOrDefaultAsync(x => x.Id == id);
            if (objHeader != null)
            {
                IEnumerable<OrderDetails> objDetails = _Db.OrderDetails.Where(x => x.OrderHeaderId == objHeader.Id);
                _Db.RemoveRange(objDetails);
                _Db.Remove(objHeader);
                return _Db.SaveChanges();
            }
            return 0;
        }

        public async Task<OrderDTO> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = _Db.OrderHeader.FirstOrDefault(x => x.Id == id),
                OrderDetails = _Db.OrderDetails.Where(x => x.OrderHeaderId == id)
            };
            if (order != null)
            {
                return _Mp.Map<Order, OrderDTO>(order);
            }
            return new OrderDTO();

        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> OrderFormDB = new List<Order>();
            IEnumerable<OrderDetails> orderDetailsList = _Db.OrderDetails;
            IEnumerable<OrderHeader> orderHeadersList= _Db.OrderHeader;
            if (userId != null)
            {
                orderHeadersList = orderHeadersList.Where(x => x.UserId == userId);
            }
            if (status != null)
            {
                orderHeadersList = orderHeadersList.Where(x => x.Status == status);
            }
            foreach (OrderHeader header in orderHeadersList)
            {
                Order order = new()
                {
                    OrderHeader = header,
                    OrderDetails =orderDetailsList.Where(x => x.OrderHeaderId==  header.Id)
                };
                OrderFormDB.Add(order);

            }
            return _Mp.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(OrderFormDB);

        }

        public async Task<OrderHeaderDTO> MarkPaymentSuccesfull(int Id)
        {
            var data = await _Db.OrderHeader.FindAsync(Id);
            if (data == null)
            {
                return new OrderHeaderDTO();
            }
            if (data.Status == SD.Status_Pending)
            {
                data.Status = SD.Status_Confirmed;
                await _Db.SaveChangesAsync();
                return _Mp.Map<OrderHeader, OrderHeaderDTO>(data);
            }
            return new OrderHeaderDTO();
        }

        public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO orderHeader)
        {
            if (orderHeader != null)
            {
                var OrderHeader = _Mp.Map<OrderHeaderDTO, OrderHeader>(orderHeader);
                _Db.OrderHeader.Update(OrderHeader);
                await _Db.SaveChangesAsync();
                return _Mp.Map<OrderHeader ,OrderHeaderDTO>(OrderHeader);
            }
            return new OrderHeaderDTO();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _Db.OrderHeader.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (status == SD.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            await _Db.SaveChangesAsync();
            return true;
        }
    }
}
