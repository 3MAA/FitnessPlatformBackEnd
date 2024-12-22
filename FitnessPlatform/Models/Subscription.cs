using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Models;

public partial class Subscription
{
    [Key]
    public int SubscriptionId { get; set; }

    public int? UserId { get; set; }

    public string? SubscriptionType { get; set; }

    public decimal? Price { get; set; }

    public string? Term { get; set; }

    public virtual User? UserIdNavigation { get; set; }
}
