using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.App.DTO
{
    public class User : Model.BenFattoUser
    {
        [DisplayName("New password and confirmation")]
        public string NewPassword { get; set; }
    }
}
