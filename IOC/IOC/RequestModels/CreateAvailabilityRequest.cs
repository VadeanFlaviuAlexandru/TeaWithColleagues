using System.ComponentModel.DataAnnotations;

namespace IOC.RequestModels
{
    public class CreateAvailabilityRequest
    {
        [Required]
        public DateTime StartDate { get; set; }
    }
}
