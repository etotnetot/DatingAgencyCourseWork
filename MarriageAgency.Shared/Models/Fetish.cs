using System;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class Fetish
    {
        public int FetishID { get; set; }

        public string FetishTitle { get; set; }

        public Fetish(int id, string name)
        {
            FetishID = id;
            FetishTitle = name;
        }

        public Fetish() { }

        public Fetish(int id)
        {
            FetishID = id;
        }

    }
}
