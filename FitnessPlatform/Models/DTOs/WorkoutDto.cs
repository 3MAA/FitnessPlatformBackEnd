namespace FitnessPlatform.Models.DTOs
{
    public class WorkoutDto
    {
        public int WorkoutId { get; set; }

        public string? WorkoutDescription { get; set; }

        public string? WorkoutType { get; set; }

        public string? DifficultyLevel { get; set; }

        public string? WorkoutDuration { get; set; }

        public decimal? CaloriesBurned { get; set; }

        public string? ContentPath { get; set; }
        public bool IsDeleted { get; set; }
    }
}
