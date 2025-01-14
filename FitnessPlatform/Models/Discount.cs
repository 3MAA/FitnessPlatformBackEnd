using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPlatform.Models;

public partial class Discount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DiscountId { get; set; }

    public int? UserId { get; set; }

    public decimal? DiscountPercent { get; set; }

    public DateOnly? GrantDate { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public bool IsUsed { get; set; }

    public virtual User? UserIdNavigation { get; set; }
}
