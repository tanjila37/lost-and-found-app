using System;
using System.Collections.Generic;

#nullable disable

namespace Lost_and_Found.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
