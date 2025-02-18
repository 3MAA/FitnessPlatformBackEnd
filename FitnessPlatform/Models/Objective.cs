﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Models;

public partial class Objective
{
    [Key]
    public int ObjectiveId { get; set; }

    public int? UserId { get; set; }

    public string? ObjectiveType { get; set; }

    public decimal? TargetValue { get; set; }

    public decimal? Progress { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? Deadline { get; set; }
    public bool IsDeleted { get; set; }

    public bool IsDraft { get; set; }

    public virtual User? UserIdNavigation { get; set; }
}
