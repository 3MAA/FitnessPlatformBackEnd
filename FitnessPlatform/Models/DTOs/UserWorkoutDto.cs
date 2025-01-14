namespace FitnessPlatform.Models.DTOs
{
    public class UserWorkoutDto
    {
        public int UserId { get; set; }
        public int WorkoutId { get; set; }
        public DateOnly CompletionDate { get; set; }
    }
}
