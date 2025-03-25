using System;
using System.Collections.Generic;

#nullable disable

namespace Lost_and_Found.Models
{
    public partial class User
    {
        public User()
        {
            Claims = new HashSet<Claim>();
            Founditems = new HashSet<Founditem>();
            Lostitems = new HashSet<Lostitem>();
            UserClaims = new HashSet<UserClaim>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<Founditem> Founditems { get; set; }
        public virtual ICollection<Lostitem> Lostitems { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
    }
}
