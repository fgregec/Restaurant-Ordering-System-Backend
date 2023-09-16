﻿using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Ingredient
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
