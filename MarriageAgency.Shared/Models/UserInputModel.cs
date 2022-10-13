using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

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