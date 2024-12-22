namespace FitnessPlatform.Models.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? UserPassword { get; set; }

        public string? Gender { get; set; }

        public int? Age { get; set; }

        public decimal? UserWeight { get; set; }

        public decimal? UserHeight { get; set; }

        public virtual ICollection<Objective> Objectives { get; set; } = new List<Objective>();

    }
}
