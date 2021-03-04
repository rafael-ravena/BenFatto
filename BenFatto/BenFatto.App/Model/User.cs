using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace BenFatto.App.Model
{
    public partial class User
    {
        public User()
        {
            UserFunctionalities = new HashSet<UserFunctionality>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserFunctionality> UserFunctionalities { get; set; }
    }
}
