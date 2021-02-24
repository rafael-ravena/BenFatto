using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

#nullable disable

namespace BenFatto.CLF.Model
{
    public partial class LogRow : IModelBase
    {
        public long Id { get; set; }
        public long ImportId { get; set; }
        [Description("IP Address")]
        public string IpAddress { get; set; }
        [Description("RFC ID")]
        public string RfcId { get; set; }
        [Description("User ID")]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public short TimeZone { get; set; }
        [NotMapped]
        [Description("Date (UTF)")]
        public string DateTimeUtfWithTimeZone
        {
            get
            {
                return Date.FormateTimeZone(TimeZone, AppSettings.Current.CultureInfo, AppSettings.Current.DateFormat);
            }
        }
        public short Method { get; set; }
        [NotMapped]
        [Description("HTTP Method")]
        public LogRowMethod MethodName
        {
            get
            {
                return (LogRowMethod)Method;
            }
            set
            {
                Method = (short)value;
            }
        }
        [Description("Requested Resource")]
        public string Resource { get; set; }
        [Description("Request Protocol")]
        public string Protocol { get; set; }
        [Description("HTTP Response Code")]
        public short ResponseCode { get; set; }
        public long ResponseSize { get; set; }
        [NotMapped]
        [Description("Response Size (bytes)")]
        public string ResponseSizeString
        {
            get
            {
                if (0 > ResponseSize)
                    return "-";
                else
                    return ResponseSize.ToString();
            }
        }
        [Description("Referer")]
        public string Referer { get; set; }
        [Description("User Agent")]
        public string UserAgent { get; set; }
        [Description("Original Line")]
        public string OriginalLine { get; set; }
        [Description("Row number in original file")]
        public int RowNumber { get; set; }

        public virtual Import Import { get; set; }

        #region [ Instance Methods ]

        public static LogRow Parse(string lineText, long importId, int lineNumber)
        {
            LogRow logRow = new LogRow();
            logRow.ImportId = importId;
            logRow.OriginalLine = lineText;

            //get IpAddress from line: from start to first occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.IpAddress = Helper.GetChunk(ref lineText, ' ');

            //get RFCId from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.RfcId = Helper.GetChunk(ref lineText, ' ');

            //get UserId from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.UserId = Helper.GetChunk(ref lineText, ' ');

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, '[');

            //get Date from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Date = DateTime.ParseExact(Helper.GetChunk(ref lineText, ' '), AppSettings.Current.DateFormat, AppSettings.Current.CultureInfo);

            //get UTC from line: from new start to next occurrence of trailing braces
            //then remove value from line to keep parsing
            logRow.TimeZone = short.Parse(Helper.GetChunk(ref lineText, ']'));

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, '"');

            //get HTTP Method from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            //yep! it has to be converted to int, and only then to short... throws an MSCoreLib Exception
            logRow.Method = (short)((int)Enum.Parse(typeof(LogRowMethod), Helper.GetChunk(ref lineText, ' '), true));

            //get Requested Resource from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Resource = Helper.GetChunk(ref lineText, ' ');

            //get Request Protocol from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.Protocol = Helper.GetChunk(ref lineText, '"');

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, ' ');

            //get Response Status Code from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.ResponseCode = short.Parse(Helper.GetChunk(ref lineText, ' '));

            //let's check if next value is the last one!
            if (lineText.IndexOf(' ') <= 0)
            {
                //that's all, folks! take the rest of it as the response size and we're done
                logRow.ResponseSize = int.Parse(lineText.Replace("-", "-1"));
                //Peace, out... (drop the mic)
                return logRow;
            }


            //oh! it was not... my bad!
            //so let's get Response Byte Size from line if any: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            //if there's nos response size (as it may happen), it will be a dash sign... then, replace with negative just to store!
            logRow.ResponseSize = int.Parse(Helper.GetChunk(ref lineText, ' ').Replace("-", "-1"));

            //and let's keep removing characters from line to keep parsing
            Helper.GetChunk(ref lineText, '"');

            //get URL from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.Referer = Helper.GetChunk(ref lineText, '"');

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, '"');

            //get User Agent from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.UserAgent = Helper.GetChunk(ref lineText, '"');

            logRow.RowNumber = lineNumber;

            //that's all, folks! Let's give the logRow instance properly filled
            return logRow;
        }

        public override string ToString()
        {
            return ($"{IpAddress} {RfcId} {UserId} [{DateTimeUtfWithTimeZone}] \"{MethodName.GetDescription().ToUpper()} {Resource} {Protocol}\" " +
                $"{ResponseCode} {ResponseSizeString} \"{Referer}\" \"{UserAgent}\"".Replace("\"\"", "")).Trim();
        }

        #endregion

    }
}
