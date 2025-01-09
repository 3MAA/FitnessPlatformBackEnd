using System.ComponentModel.DataAnnotations;

namespace FitnessPlatform.Models
{
    public class UserWorkout
    {
        [Key]
        public int UserWorkoutId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }  
        public int WorkoutId { get; set; }
        public virtual Workout Workout { get; set; }  
        public DateOnly CompletionDate { get; set; }
    }
}
