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
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Product> _productRepository;

        public BasketService(IBaseRepository<User> userRepository, IBaseRepository<Product> productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<IBaseResponse<IEnumerable<OrderViewModel>>> GetItems(string userName)
        {
            try
            {
                var user = await _userRepository.Select()
                    .Include(x => x.Basket)
                    .ThenInclude(y => y.Orders)
                    .FirstOrDefaultAsync(z => z.Login == userName);


                if (user == null)
                {
                    return new BaseResponse<IEnumerable<OrderViewModel>>()
                    {
                        DescriptionError = "Пользователь не найден",
                        StatusCode = StatusCode.InternalServer
                    };
                }

                var orders = user.Basket?.Orders;
                var response = from p in orders
                               join c in _productRepository.Select() on p.ProductId equals c.Id
                               select new OrderViewModel()
                               {
                                   Id = p.Id,
                                   ProductName = c.Name,
                                   Type = c.Type,
                                   Image = c.Image,
                                   Price = c.Price * p.Quantity,
                                   Quantity = p.Quantity
                               };

                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    DescriptionError = ex.Message,
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<OrderViewModel>> GetItem(string userName, Guid id)
        {
            try
            {
                var user = await _userRepository.Select()
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.Orders)
                    .FirstOrDefaultAsync(x => x.Login == userName);

                if (user == null)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        DescriptionError = "Пользователь не найден",
                        StatusCode = StatusCode.InternalServer
                    };
                }

                var orders = user.Basket?.Orders.Where(x => x.Id == id).ToList();
                if (orders == null || orders.Count == 0)
                {
                    return new BaseResponse<OrderViewModel>()
                    {
                        DescriptionError = "Заказов нет",
                        StatusCode = StatusCode.OrderNotFound
                    };
                }

                var response = (from p in orders
                                join c in _productRepository.Select() on p.ProductId equals c.Id
                                select new OrderViewModel()
                                {
                                    Id = p.Id,
                                    ProductName = c.Name,
                                    Type = c.Type,
                                    Address = p.Address,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,
                                    DateCreate = p.DateCreated.ToLongDateString(),
                                    Image = c.Image,
                                    Price = c.Price
                                }).FirstOrDefault();

                return new BaseResponse<OrderViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderViewModel>()
                {
                    DescriptionError = ex.Message,
                    StatusCode = StatusCode.InternalServer
                };
            }
        }
    }
}
