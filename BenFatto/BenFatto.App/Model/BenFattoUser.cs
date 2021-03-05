using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BenFatto.App.Model
{
    [Table("BenFattoUsers")]
    public partial class BenFattoUser
    {
        [DisplayName("User Id")]
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("E-mail address")]
        public string Email { get; set; }
        [DisplayName("Password")]
        [StringLength(12, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
