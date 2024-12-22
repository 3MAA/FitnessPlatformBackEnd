using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Models;

public partial class Discount
{
    [Key]
    public int DiscountId { get; set; }

    public int? UserId { get; set; }

    public decimal? DiscountPercent { get; set; }

    public DateOnly? GrantDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public virtual User? User { get; set; }
}
