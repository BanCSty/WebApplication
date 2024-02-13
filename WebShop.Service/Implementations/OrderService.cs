using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DAL.Interfaces;
using WebShop.Domain.Entity;
using WebShop.Domain.Enum;
using WebShop.Domain.Responce;
using WebShop.Domain.ViewModels.Order;
using WebShop.Service.Interfaces;

namespace WebShop.Service.Implementations
{
    public class OrderService : IOrderService
    {

        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Basket> _basketRepository;
        private readonly IBaseRepository<Product> _productRepository;

        public OrderService(IBaseRepository<User> userRepository, IBaseRepository<Order> orderRepository, IBaseRepository<Basket> basketRepository, IBaseRepository<Product> productRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        public async Task<IBaseResponse<Order>> Create(CreateOrderViewModel model)
        {
            try
            {
                var user = await _userRepository.Select()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.Login == model.Login);



                if (user == null)
                {
                    return new BaseResponse<Order>()
                    {
                        DescriptionError = "Пользователь не найден",
                        StatusCode = StatusCode.InternalServer
                    };
                }

                var product = await _productRepository.Select().FirstOrDefaultAsync(x => x.Id == model.ProductId);
                if(product == null)
                    return new BaseResponse<Order>()
                    {
                        DescriptionError = "Пользователь не найден",
                        StatusCode = StatusCode.InternalServer
                    };

                var order = new Order()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    DateCreated = DateTime.Now,
                    BasketId = user.Basket.Id,
                    ProductId = model.Id,
                    Price = product.Price * model.Quantity,
                    Quantity = model.Quantity
                };

                await _orderRepository.Create(order);

                return new BaseResponse<Order>()
                {
                    DescriptionError = "Заказ создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    DescriptionError = ex.Message,
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(Guid id)
        {
            try
            {
                var order = await _orderRepository.Select()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.OrderNotFound,
                        DescriptionError = "Заказ не найден"
                    };
                }

                await _orderRepository.Delete(order.Id);
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    DescriptionError = "Заказ удален"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    DescriptionError = ex.Message,
                    StatusCode = StatusCode.InternalServer
                };
            }
        }
    }
}
