using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? UserPassword { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public decimal? UserWeight { get; set; }

    public decimal? UserHeight { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<Objective> Objectives { get; set; } = new List<Objective>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
