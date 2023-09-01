using System;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class Requirement
    {
        public int RequirementID { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым!")]
        public string PartnerGender { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым!")]
        [Range(18, 99, ErrorMessage = "Минимальный возраст от 18!")]
        public int AgeFrom { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым!")]
        [Range(18, 99, ErrorMessage = "Максимальный возраст до 18!")]
        public int AgeTo { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым!")]
        public string BodyType { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым!")]
        public string Kids { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым!")]
        public string Education { get; set; }

        public Requirement() { }

        public Requirement(int id)
        {
            RequirementID = id;
        }

        public Requirement(int id, string gender, int from, int to, string body, string kids, string education)
        {
            RequirementID = id;
            PartnerGender = gender;
            AgeFrom = from;
            AgeTo = to;
            BodyType = body;
            Kids = kids;
            Education = education;
        }
    }
}
