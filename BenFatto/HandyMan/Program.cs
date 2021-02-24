using BenFatto.CLF.Model;
using System;
using System.IO;
using BenFatto;

namespace HandyMan
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while ("exit" != (line = Console.ReadLine()))
            {
                switch (line.ToLower())
                {
                    case "intparser":
                        CheckIntParser();
                        break;
                    case "settings":
                        TestSettingsReader();
                        break;
                    case "file":
                        TestFileImporter();
                        break;
                    case "help":
                        HelpMe();
                        break;
                    default:
                        WriteOut("Enter the row to test and press enter!");
                        TestLogRowParse(Console.ReadLine());
                        break;
                }
            }
        }

        private static void TestFileImporter()
        {
            string file = GenerateFile();
            BenFatto.CLF.Service.FileProcessor processor = new BenFatto.CLF.Service.FileProcessor(file, 0);
            processor.ProcessFile();
        }

        private static string GenerateFile()
        {
            string[] IpAddresses = { "144.203.204.43", "22.52.125.200", "99.22.11.33", "11.20.0.100", "14.209.90.10", "88.11.23.55", "100.10.12.10", "94.52.44.33", "112.22.33.200", "44.88.100.115", "23.12.45.12", "55.99.12.44", "220.200.103.204", "221.123.22.151", "127.0.0.1", "132.10.11.5", "8.8.8.8", "214.44.55.123", "85.24.94.4", string.Empty };
            string[] RfcIds = { "-", "user-identifier", "intranet", "some-other", "lovedoodles", "code99", string.Empty };
            string[] Users = { "-", "Mike", "Bob", "Tom", "Kenny", "Rick", "Morthy", string.Empty };

            string[] Methods = { "GET", "POST", "PUT", "PATCH", "DELETE", string.Empty };
            string[] Resources = { "http://www.teste.com/file", "http://www.paginaweb.com.br/controller/view", "http://www.website.com/", "http://www.exemple.url", "http://www.instance.net", "http://www.google.com/?s=wtf", "http://www.lmgtfy.com/?q=dummy+text", "http://www.google.com/?q=dummy+url+generator", "http://bit.ly/AFShASSa34d16fnosan35", string.Empty };
            string[] Protocols = { "HTTP/1.0", "HTTP/2.0", string.Empty };
            int[] ResponseCodes = { -1, 200, 204, 404, 500, 400, 422 };

            string[] Agents = { "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0", "Microsoft Office/15.0 (Windows NT 6.1; Microsoft Outlook 15.0.4631; Pro)", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299", string.Empty };

            string fileName = $"{Directory.GetCurrentDirectory()}\\{DateTime.Now.ToString("yyyyMMdd-hhmmss")}.log";

            Random random = new Random(int.Parse(DateTime.Now.ToString("hhmmssfff")));
            using (StreamWriter stream = File.CreateText(fileName))
            {
                int count = 0;
                for (int i = 0; i < random.Next((int)1e4, (int)1e5); i++)
                {
                    string line = $"{IpAddresses[random.Next(0, IpAddresses.Length)]} ";
                    line += $"{RfcIds[random.Next(0, RfcIds.Length)]} ";
                    line += $"{Users[random.Next(0, Users.Length)]} ";
                    line += $"[{DateTime.Now.AddMilliseconds(random.Next((int)-10368e5, (int)10368e5)).ToString(BenFatto.CLF.AppSettings.Current.DateFormat, BenFatto.CLF.AppSettings.Current.CultureInfo)} {(random.Next(1, 12) * 100).ToTimeZoneString()}] ";
                    line += $"\"{Methods[random.Next(0, Methods.Length)]} ";
                    line += $"{Resources[random.Next(0, Resources.Length)]} ";
                    line += $"{Protocols[random.Next(0, Protocols.Length)]}\" ";
                    line += $"{ResponseCodes[random.Next(0, ResponseCodes.Length)]} ";
                    line += $"{Math.Floor(random.NextDouble() * random.NextDouble() * Math.Pow(1024, 3))} ";
                    line += $"\"{Resources[random.Next(0, Resources.Length)]}\" ";
                    line += $"\"{Agents[random.Next(0, Agents.Length)]}\"";
                    stream.Write(line.Replace("\"\"", "").Trim() + '\r' + '\n');
                    count++;
                    if (100 < count)
                    {
                        stream.Flush();
                        count = 0;
                    }
                }
            }
            return fileName;
        }

        private static void TestSettingsReader()
        {
            WriteOut(BenFatto.CLF.DbConfiguration.Current.ConnectionString);
            WriteOut(BenFatto.CLF.AppSettings.Current.BatchSize.ToString());
            WriteOut(BenFatto.CLF.AppSettings.Current.AutoFlush.ToString());
        }

        private static void HelpMe()
        {
            WriteOut("type one of the following commands:");
            WriteOut("\tIntParser: ParseInt characters");
            WriteOut("\tLogRow: convert LogFile row to object, then print it back");
        }
        static void CheckIntParser()
        {
            WriteOut(int.Parse("0500").ToString());
            WriteOut(int.Parse("+0500").ToString());
            WriteOut(int.Parse("-0500").ToString());
            WriteOut(Math.Abs(int.Parse("-0500")).ToString());
            WriteOut(Math.Abs(int.Parse("+0500")).ToString());
            WriteOut(Math.Abs(Double.Parse("+0500.50")).ToString());
        }
        static void TestLogRowParse(string row)
        {
            LogRow logRow = LogRow.Parse(row, 0, 0);
            WriteOut(logRow.ToString());
            WriteOut($"equals? {logRow.OriginalLine == logRow.ToString()}");
        }
        static void WriteOut(string message)
        {
            Console.WriteLine(message);
        }
    }
}
