using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebShop.Domain.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            string password = value as string;
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Регулярное выражение для проверки пароля
            // (?=.*[A-Z]) - должен содержать хотя бы одну заглавную букву
            // (?=.*[a-z]) - должен содержать хотя бы одну строчную букву
            // (?=.*\d) - должен содержать хотя бы одну цифру
            // .{8,16} - должен иметь длину от 8 до 16 символов
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{6,50}$");
        }
    }
}
