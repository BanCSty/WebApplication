using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Responce;
using WebShop.Domain.ViewModels.Order;

namespace WebShop.Service.Interfaces
{
    public interface IBasketService
    {
        //Получение всех заказов пользователя
        Task<IBaseResponse<IEnumerable<OrderViewModel>>> GetItems(string userName);

        //Получение сведений об одном заказе
        Task<IBaseResponse<OrderViewModel>> GetItem(string userName, Guid id);
    }
}
