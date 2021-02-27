using System;
using System.ComponentModel;

namespace BenFatto.App.DTO
{
    public class LogRow
    {
        public long Id { get; set; }
        public long ImportId { get; set; }
        [DisplayName("IP Address")]
        public string IpAddress { get; set; }
        [DisplayName("RFC ID")]
        public string RfcId { get; set; }
        [DisplayName("User ID")]
        public string UserId { get; set; }
        [DisplayName("HTTP Method")]
        public DateTime Date { get; set; }
        [DisplayName("UTC Time zone")]
        public short TimeZone { get; set; }
        [DisplayName("HTTP Method")]
        public short Method { get; set; }
        [DisplayName("Requested Resource")]
        public string Resource { get; set; }
        [DisplayName("Request Protocol")]
        public string Protocol { get; set; }
        [DisplayName("HTTP Response Code")]
        public short ResponseCode { get; set; }
        public long ResponseSize { get; set; }
        [DisplayName("Referer")]
        public string Referer { get; set; }
        [DisplayName("User Agent")]
        public string UserAgent { get; set; }
        [DisplayName("Original Line")]
        public string OriginalLine { get; set; }
        [DisplayName("Row number in original file")]
        public int RowNumber { get; set; }
        public virtual Import Import { get; set; }
    }
}