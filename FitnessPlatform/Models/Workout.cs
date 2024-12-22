﻿using System;
using System.Collections.Generic;

namespace FitnessPlatform.Models;

public partial class Workout
{
    public int WorkoutId { get; set; }

    public string? WorkoutDescription { get; set; }

    public string? WorkoutType { get; set; }

    public string? DifficultyLevel { get; set; }

    public string? WorkoutDuration { get; set; }

    public decimal? CaloriesBurned { get; set; }

    public string? ContentPath { get; set; }
}
