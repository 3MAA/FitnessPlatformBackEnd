namespace FitnessPlatform.Models.DTOs
{
    public class ObjectiveDto
    {
        public int ObjectiveId { get; set; }

        public int? UserId { get; set; }

        public string? ObjectiveType { get; set; }

        public decimal? TargetValue { get; set; }

        public decimal? Progress { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? Deadline { get; set; }
        public bool IsDeleted { get; set; }
    }
}
