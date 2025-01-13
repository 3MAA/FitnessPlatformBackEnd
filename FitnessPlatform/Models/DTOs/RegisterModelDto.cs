﻿using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Models.DTOs
{
    public class RegisterModelDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? UserPassword { get; set; }
    }
}
