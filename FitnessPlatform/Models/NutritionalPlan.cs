using System;
using System.Collections.Generic;

namespace FitnessPlatform.Models;

public partial class NutritionalPlan
{
    public int PlanId { get; set; }

    public string? PlanType { get; set; }

    public string? MealType { get; set; }

    public string? FoodStuff { get; set; }

    public decimal? Grammage { get; set; }

    public decimal? FoodStuffCalories { get; set; }

    public decimal? MealCalories { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
