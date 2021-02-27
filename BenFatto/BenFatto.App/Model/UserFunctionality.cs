using System;
using System.Collections.Generic;

#nullable disable

namespace BenFatto.App.Model
{
    public partial class UserFunctionality
    {
        public int UserId { get; set; }
        public int FunctionalityId { get; set; }

        public virtual Functionality Functionality { get; set; }
        public virtual User User { get; set; }
    }
}
