using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class User
    {
        public int ClientID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string ClientFullName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string ClientGender { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string ClientPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string EducationID { get; set; }

        public string ZodiacSign { get; set; }

        public string BodyType { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string ClientCity { get; set; }

        public string ClientKids { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string ClientHobbies { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string ClientInformation { get; set; }

        public Requirement RequirementID { get; set; }

        public byte[] ProfilePhoto { get; set; }

        public Fetish FetishID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public DateTime BirthDate { get; set; }

        public virtual List<Invitation> MyInvitations { get; set; }

        public virtual List<Invitation> SentInvitations { get; set; }

        public virtual List<int> UsersArchive { get; set; }

        public bool IsAdmin { get; set; }

        public User ClientCouple { get; set; }

        public int CompabilityPoint { get; set; }

        public string BestCompability { get; set; }

        public User(string mailName)
        {
            Email = mailName;
        }

        public User(string name, int age, string city)
        {
            ClientFullName = name;
            Age = age;
            ClientCity = city;
            ProfilePhoto = new byte[] { };
        }

        public User() { }

        public int Age { get; set; }
    }
}