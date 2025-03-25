using System;
using System.Collections.Generic;

#nullable disable

namespace Lost_and_Found.Models
{
    public partial class Claim
    {
        public Claim()
        {
            UserClaims = new HashSet<UserClaim>();
        }

        public int ClaimId { get; set; }
        public int? UserId { get; set; }
        public int? ItemId { get; set; }

        public virtual Founditem Item { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
    }
}
