using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Shared.Models
{
    public class UserInputModel
    {
        [Required]
        User CurrentUser { get; set; }

        [Required]
        Requirement RequirementOfUser { get; set; }       
    }
}