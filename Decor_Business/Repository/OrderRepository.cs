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

        public async Task<OrderHeaderDTO> CancelOrder(int orderId)
        {

            var orderHeader = await _Db.OrderHeader.FindAsync(orderId);
            if (orderHeader == null) { 
                return new OrderHeaderDTO();
            }
            if (orderHeader.Status ==SD.Status_Pending)
            {
                orderHeader.Status = SD.Status_Cancelled;
                await _Db.SaveChangesAsync();
            }
            if (orderHeader.Status ==SD.Status_Confirmed)
            {
                var options = new Stripe.RefundCreateOptions
                {
                    Reason = Stripe.RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId,
                };
                var sevice = new Stripe.RefundService();
                Stripe.Refund refund = sevice.Create(options);

                orderHeader.Status = SD.Status_Refunded;
                await _Db.SaveChangesAsync();
            }
            return _Mp.Map<OrderHeader, OrderHeaderDTO>(orderHeader);

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

        public async Task<OrderHeaderDTO> MarkPaymentSuccesfull(int Id ,string payMentID )
        {
            var data = await _Db.OrderHeader.FindAsync(Id);
            if (data == null)
            {
                return new OrderHeaderDTO();
            }
            if (data.Status == SD.Status_Pending)
            {
                data.PaymentIntentId = payMentID;
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
                var orderHeaderFormDB = _Db.OrderHeader.FirstOrDefault(u => u.Id == orderHeader.Id);
                orderHeaderFormDB.Name = orderHeader.Name;
                orderHeaderFormDB.PhoneNumber = orderHeader.PhoneNumber;
                orderHeaderFormDB.Carrier = orderHeader.Carrier;
                orderHeaderFormDB.Tracking = orderHeader.Tracking;
                orderHeaderFormDB.StreetAddress = orderHeader.StreetAddress;
                orderHeaderFormDB.City = orderHeader.City;
                orderHeaderFormDB.State = orderHeader.State;
                orderHeaderFormDB.PostalCode = orderHeader.PostalCode;
                orderHeaderFormDB.Status = orderHeader.Status;
                await _Db.SaveChangesAsync();
                return _Mp.Map<OrderHeader ,OrderHeaderDTO>(orderHeaderFormDB);
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
