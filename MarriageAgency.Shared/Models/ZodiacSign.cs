using System;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class ZodiacSign
    {
        public string ZodiacSignName { get; set; }

        public string ZodiacDescription { get; set; }

        public string BestCompability { get; set; }

        public ZodiacSign() { }
    }
}