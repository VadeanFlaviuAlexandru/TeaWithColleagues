using System.ComponentModel.DataAnnotations;

namespace IOC.RequestModels
{
    public class CreateAvailabilityRequest
    {
        public int? IdParticipant { get; set; }
        public string? Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}
