using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace BenFatto.App.DTO
{
    public class File
    {
        [DisplayName("Log file")]
        public IFormFile LogFile { get; set; }
    }
}