using System.ComponentModel.DataAnnotations;

namespace IOC.RequestModels
{
    public class CreateTeaTimeRequest
    {
        [Required]
        public int? IdParticipant { get; set;}

        [Required]
        public string? Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}
