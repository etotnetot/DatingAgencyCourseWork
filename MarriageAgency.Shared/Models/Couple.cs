﻿using System;

namespace MarriageAgency.Shared.Models
{
    [Serializable]
    public class Couple
    {
        public int CoupleID { get; set; }

        public int FirstClient { get; set; }

        public int SecondClient { get; set; }

        public string CoupleStatus { get; set; }

        public Couple() { }
    }
}