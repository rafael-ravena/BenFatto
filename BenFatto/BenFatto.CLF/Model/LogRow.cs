using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable

namespace BenFatto.CLF.Model
{
    public partial class LogRow
    {
        public long Id { get; set; }
        public int ImportId { get; set; }
        public string IpAddress { get; set; }
        public string RfcId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public short TimeZone { get; set; }
        public string DateTimeUtfWithTimeZone
        {
            get
            {
                return Date.FormateTimeZone(TimeZone);
            }
        }
        public short Method { get; set; }
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
        public string Resource { get; set; }
        public string Protocol { get; set; }
        public short ResponseCode { get; set; }
        public long ResponseSize { get; set; }
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
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public string OriginalLine { get; set; }
        public int? RowNumber { get; set; }

        public virtual Import Import { get; set; }

        #region [ Instance Methods ]

        public static LogRow Parse(string line)
        {
            LogRow logRow = new LogRow();
            logRow.OriginalLine = line;

            //get IpAddress from line: from start to first occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.IpAddress = Helper.GetChunk(ref line, ' ');

            //get RFCId from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.RfcId = Helper.GetChunk(ref line, ' ');

            //get UserId from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.UserId = Helper.GetChunk(ref line, ' ');

            //remove character from line to keep parsing
            Helper.GetChunk(ref line, '[');

            //get Date from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Date = DateTime.ParseExact(Helper.GetChunk(ref line, ' '), "dd/MMM/yyyy:HH:mm:ss", CultureInfo.GetCultureInfo("en-US"));

            //get UTC from line: from new start to next occurrence of trailing braces
            //then remove value from line to keep parsing
            logRow.TimeZone = short.Parse(Helper.GetChunk(ref line, ']'));

            //remove character from line to keep parsing
            Helper.GetChunk(ref line, '"');

            //get HTTP Method from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Method = (short)Enum.Parse(typeof(LogRowMethod), Helper.GetChunk(ref line, ' '), true);

            //get Requested Resource from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Resource = Helper.GetChunk(ref line, ' ');

            //get Request Protocol from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.Protocol = Helper.GetChunk(ref line, '"');

            //remove character from line to keep parsing
            Helper.GetChunk(ref line, ' ');

            //get Response Status Code from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.ResponseCode = short.Parse(Helper.GetChunk(ref line, ' '));

            //let's check if next value is the last one!
            if (line.IndexOf(' ') <= 0)
            {
                //that's all, folks! take the rest of it as the response size and we're done
                logRow.ResponseSize = int.Parse(line.Replace("-", "-1"));
                //Peace, out... (drop the mic)
                return logRow;
            }


            //oh! it was not... my bad!
            //so let's get Response Byte Size from line if any: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            //if there's nos response size (as it may happen), it will be a dash sign... then, replace with negative just to store!
            logRow.ResponseSize = int.Parse(Helper.GetChunk(ref line, ' ').Replace("-", "-1"));

            //and let's keep removing characters from line to keep parsing
            Helper.GetChunk(ref line, '"');

            //get URL from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.Referer = Helper.GetChunk(ref line, '"');

            //remove character from line to keep parsing
            Helper.GetChunk(ref line, '"');

            //get User Agent from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.UserAgent = Helper.GetChunk(ref line, '"');

            //that's all, folks! Let's give the logRow instance properly filled
            //Don't forget to fulfill my row number, man!
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
