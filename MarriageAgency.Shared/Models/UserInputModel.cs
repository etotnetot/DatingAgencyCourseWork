using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Shared.Models
{
    public class UserInputModel
    {
        [Required]
        [ValidateComplexType]
        public User User { get; set; }

        [Required]
        public Requirement RequirementOfUser { get; set; }       
    }
}