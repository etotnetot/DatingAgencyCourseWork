using System;

namespace MarriageAgency.BLL.Models
{
    public class Invitation
    {
        public int InvitationID { get; set; }

        public int Sender { get; set; }

        public int Recipient { get; set; }

        public DateTime InvitationDate { get; set; }

        public string InvitationPlace { get; set; }

        public Invitation(int id, int from, int to, DateTime date, string place)
        {
            InvitationID = id;
            Sender = from;
            Recipient = to;
            InvitationDate = date;
            InvitationPlace = place;
        }

        public Invitation() { }
    }
}
