using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Attributes;

namespace WebShop.Domain.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [MaxLength(30, ErrorMessage = "Логин должно иметь длину меньше 30 символов")]
        [MinLength(6, ErrorMessage = "Логин должно иметь длину больше 6 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(30, ErrorMessage = "Имя должно иметь длину меньше 30 символов")]
        [MinLength(6, ErrorMessage = "Имя должно иметь длину больше 6 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [MaxLength(30, ErrorMessage = "Фамилия должна иметь длину меньше 30 символов")]
        [MinLength(3, ErrorMessage = "Фамилия должна иметь длину больше 3 символов")]
        public string LastName { get; set; }

        [StrongPassword(ErrorMessage = "пароль должен содержать хотя бы одну заглавную букву, одну строчную букву, одну цифру и имеет длину от 8 до 16 символов")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}

