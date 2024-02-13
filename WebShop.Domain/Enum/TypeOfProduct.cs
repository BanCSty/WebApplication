using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.Enum
{
    public enum TypeOfProduct
    {
        //СТРОЙМАТЕРИАЛЫ
        [Display(Name = "Кирпич")]
        Kirpich = 0,
        [Display(Name = "Общестроительные материалы")]
        StroyMaterial = 1,
        [Display(Name = "Металлопрокат")]
        Metalloprokat = 2,
        [Display(Name = "Сухие смеси")]
        SuhieSmesi = 3,
        [Display(Name = "Цемент, вяжущие и сыпучие материалы")]
        Cement = 4,

        //ИЗОЛЯЦИЯ
        [Display(Name = "Теплоизоляция")]
        Teoloizolyatsiya = 5,
        [Display(Name = "Звукоизоляционные материалы")]
        ZvukoizolytsionnieMateriali = 6,
        [Display(Name = "Пены, герметики")]
        PenaGermetiki = 7,
        [Display(Name = "Полиэтиленовые пленки")]
        Plenki = 8,
        [Display(Name = "Ветрозащита")]
        Vetrozashita = 9,

    }
}
