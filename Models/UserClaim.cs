using System;
using System.Collections.Generic;

#nullable disable

namespace Lost_and_Found.Models
{
    public partial class UserClaim
    {
        public int UserClaimId { get; set; }
        public int? UserId { get; set; }
        public int? ClaimId { get; set; }

        public virtual Claim User { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
