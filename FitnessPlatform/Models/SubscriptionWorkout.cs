using System;
using System.Collections.Generic;

namespace FitnessPlatform.Models;

public partial class SubscriptionWorkout
{
    public int? SubscriptionId { get; set; }

    public int? WorkoutId { get; set; }

    public decimal? WorkoutPrice { get; set; }

    public virtual Subscription? Subscription { get; set; }

    public virtual Workout? Workout { get; set; }
}
