using System;
using System.Collections.Generic;

#nullable disable

namespace BenFatto.App.Model
{
    public partial class Functionality
    {
        public Functionality()
        {
            UserFunctionalities = new HashSet<UserFunctionality>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }

        public virtual ICollection<UserFunctionality> UserFunctionalities { get; set; }
    }
}
