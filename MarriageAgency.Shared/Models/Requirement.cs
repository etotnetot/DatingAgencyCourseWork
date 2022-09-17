using System;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class Requirement
    {
        public int RequirementID { get; set; }

        public string PartnerGender { get; set; }

        public int AgeFrom { get; set; }

        public int AgeTo { get; set; }

        public string BodyType { get; set; }

        public string Kids { get; set; }

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
