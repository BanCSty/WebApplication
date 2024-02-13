using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Количество")]
        [Range(1, 100, ErrorMessage = "Количество должно быть от 1 до 100")]
        public int Quantity { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Укажите адрес")]
        [MinLength(5, ErrorMessage = "Адрес должен быть больше 5 символов")]
        [MaxLength(200, ErrorMessage = "Адрес должен быть меньше 200 символов")]
        public string Address { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(20, ErrorMessage = "Имя должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [MaxLength(50, ErrorMessage = "Фамилия должно иметь длину меньше 50 символов")]
        [MinLength(2, ErrorMessage = "Фамилия должно иметь длину больше 2 символов")]
        public string LastName { get; set; }

        public Guid ProductId { get; set; }

        public string Login { get; set; }

        public decimal Price { get; set; }
    }
}
