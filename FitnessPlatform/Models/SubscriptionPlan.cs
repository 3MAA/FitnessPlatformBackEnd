using System;
using System.Collections.Generic;

namespace FitnessPlatform.Models;

public partial class SubscriptionPlan
{
    public int? SubscriptionId { get; set; }

    public int? PlanId { get; set; }

    public decimal? PlanPrice { get; set; }

    public virtual NutritionalPlan? Plan { get; set; }

    public virtual Subscription? Subscription { get; set; }
}
