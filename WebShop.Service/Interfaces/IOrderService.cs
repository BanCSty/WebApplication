using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Entity;
using WebShop.Domain.Responce;
using WebShop.Domain.ViewModels.Order;

namespace WebShop.Service.Interfaces
{
    public interface IOrderService
    {
        //Создание заказа
        Task<IBaseResponse<Order>> Create(CreateOrderViewModel model);

        //Удаление заказа
        Task<IBaseResponse<bool>> Delete(Guid id);
    }
}
