﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }

        [Required (ErrorMessage = "Proszę podać nazwę produktu")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Proszę podać opis.")]
        [DataType(DataType.MultilineText),Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Proszę podać dodatnią cenę.")]
        [Display (Name = "Cena")]
        public decimal Price { get; set; }

        [Required (ErrorMessage =  "Proszę określić kategorię.")]
        [Display (Name = "Kategoria")]
        public string Category { get; set; }

    }
}
