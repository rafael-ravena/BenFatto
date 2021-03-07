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
        public string IpAddress { get; set; }
        public string RfcId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public short TimeZone { get; set; }
        [NotMapped]
        public string DateTimeUtfWithTimeZone
        {
            get
            {
                return Date.FormateTimeZone(TimeZone, ClfAppSettings.Current.CultureInfo, ClfAppSettings.Current.DateFormat);
            }
        }
        public short Method { get; set; }
        [NotMapped]
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
        [NotMapped]
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
        public int RowNumber { get; set; }
        public virtual Import Import { get; set; }

        #region [ Instance Methods ]

        public void SetMethodByName(string value)
        {
            Method = (short)((int)Enum.Parse(typeof(LogRowMethod), value, true));
        }

        public static LogRow Parse(string lineText, long importId, int lineNumber)
        {
            LogRow logRow = new LogRow();
            logRow.ImportId = importId;
            logRow.OriginalLine = lineText;


            //get IpAddress from line: from start to first occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.IpAddress = Helper.GetChunk(ref lineText, ' ');

            //anytime, if the parsing is over (there's no more text to parse), give back
            //      the LogRow Object and leave validation to DB validation process
            if (lineText.Length == 0)
                return logRow;

            //get RFCId from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.RfcId = Helper.GetChunk(ref lineText, ' ');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get UserId from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.UserId = Helper.GetChunk(ref lineText, ' ');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, '[');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get Date from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Date = DateTime.ParseExact(Helper.GetChunk(ref lineText, ' '), ClfAppSettings.Current.DateFormat, ClfAppSettings.Current.CultureInfo);

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get UTC from line: from new start to next occurrence of trailing braces
            //then remove value from line to keep parsing
            logRow.TimeZone = short.Parse(Helper.GetChunk(ref lineText, ']'));

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, '"');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get HTTP Method from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            //yep! it has to be converted to int, and only then to short... throws an MSCoreLib Exception
            logRow.Method = (short)((int)Enum.Parse(typeof(LogRowMethod), Helper.GetChunk(ref lineText, ' '), true));

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get Requested Resource from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.Resource = Helper.GetChunk(ref lineText, ' ');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get Request Protocol from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.Protocol = Helper.GetChunk(ref lineText, '"');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, ' ');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get Response Status Code from line: from new start to next occurrence of whitespace
            //then remove value from line to keep parsing
            logRow.ResponseCode = short.Parse(Helper.GetChunk(ref lineText, ' '));

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

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

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //and let's keep removing characters from line to keep parsing
            Helper.GetChunk(ref lineText, '"');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //get URL from line: from new start to next occurrence of double quote
            //then remove value from line to keep parsing
            logRow.Referer = Helper.GetChunk(ref lineText, '"');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

            //remove character from line to keep parsing
            Helper.GetChunk(ref lineText, '"');

            //Again, live it to DB validation
            if (lineText.Length == 0)
                return logRow;

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
