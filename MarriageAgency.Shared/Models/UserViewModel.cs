using System;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(string userName, int userAge, string userCity) 
        {
            Username = userName;
            City = userCity;
            Age = userAge;
            ProfilePicture = new byte[] { };
        }

        public byte[] ProfilePicture { get; set; }

        public string Username { get; set; }
        
        public string City { get; set; }

        public int Age { get; set; }
    }
}