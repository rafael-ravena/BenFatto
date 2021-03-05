using BenFatto.CLF.Model;
using System;
using System.IO;
using BenFatto;
using System.Net.Http;
using Newtonsoft.Json;
using BenFatto.App.Model;

namespace HandyMan
{
    class Program
    {
        static string _fileName;
        private static string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                    _fileName = $"{Directory.GetCurrentDirectory()}\\{DateTime.Now.ToString("yyyyMMdd-hhmmss")}.log";
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        static void Main(string[] args)
        {
            Instructions();
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
                    case "createfile":
                        string file = GenerateFile();
                        WriteOut($"File successfuly created: {file}");
                        break;
                    case "file":
                        ImportFewFiles();
                        break;
                    case "parse":
                        WriteOut("Enter the row to test and press enter!");
                        TestLogRowParse(Console.ReadLine());
                        break;
                    case "setfile":
                        WriteOut("Enter the file name to be generated!");
                        FileName = Console.ReadLine();
                        break;
                    case "user":
                        CreateDefaulUser();
                        break;
                    case "help":
                        HelpMe();
                        break;
                    default:
                        WriteOut("Command not found!");
                        HelpMe();
                        WriteOut("");
                        break;
                }
                Instructions();
            }
        }

        private static void HelpMe()
        {
            WriteOut("type one of the following commands:");
            WriteOut("\tIntParser: ParseInt characters");
            WriteOut("\tSettings: read from appsettings");
            WriteOut("\tUser: Create a valid User to operate the application");
            WriteOut("\tCreateFile: Create a log file with valid and invalid bogus data");
            WriteOut("\tFile: Imports 5 files with valid and invalid bogus data");
            WriteOut("\tParse: convert LogFile row to object, then print it back.");
            WriteOut("\t\t(to verify if the original row is equal to the generated one)");
            WriteOut("\tSetFile: sets the logical path to generate file (in CreateFile)");
            WriteOut("\tHelp: List available commands");
        }

        private static void CreateDefaulUser()
        {
            WriteOut("OK! Let's create a user!");
            WriteOut("Enter \"#CANCEL!\" anytime to cancel this operation!");
            WriteOut("Enter the new user's name (Name + Last Name)!");
            string userName = Console.ReadLine();
            if ("#CANCEL!" == userName)
                return;
            WriteOut($"Enter {userName}'s e-mail address!");
            string email = Console.ReadLine();
            if ("#CANCEL!" == email)
                return;
            WriteOut("Finally enter the password!");
            string password = Console.ReadLine();
            if ("#CANCEL!" == password)
                return;
            BenFattoUser usr = new BenFattoUser { Email = email, Name = userName, Password = password };
            using (BenFattoAppContext context = new BenFattoAppContext())
            {
                context.Users.Add(usr);
                context.SaveChanges();
            }
            WriteOut($"User successfuly created with Id: {usr.Id}");
        }

        private static void Instructions()
        {
            WriteOut("Type the command and press enter! (type help for list of available commands)");
        }

        private static void ImportFewFiles()
        {
            for (int i = 0; i < 5; i++)
                TestFileImporter();
        }

        private static void TestFileImporter()
        {
            string file = GenerateFile();
            BenFatto.CLF.Service.FileProcessor processor = new BenFatto.CLF.Service.FileProcessor(file);
            using (ClfContext context = new ClfContext())
                processor.ProcessFile(context);
            WriteOut($"File imported successfuly: {file} at {DateTime.Now.ToString(BenFatto.CLF.ClfAppSettings.Current.DateFormat, BenFatto.CLF.ClfAppSettings.Current.CultureInfo)}");
        }

        private static string GenerateFile()
        {
            string[] RfcIds = { "-", "user-identifier", "intranet", "some-other", "lovedoodles", "code99", string.Empty };
            string[] Users = { "-", "Mike", "Bob", "Tom", "Kenny", "Rick", "Morthy", string.Empty };

            string[] Methods = { "GET", "POST", "PUT", "PATCH", "DELETE", string.Empty };
            string[] Resources = { "http://www.teste.com/file", "http://www.paginaweb.com.br/controller/view", "http://www.website.com/", "http://www.exemple.url", "http://www.instance.net", "http://www.google.com/?s=wtf", "http://www.lmgtfy.com/?q=dummy+text", "http://www.google.com/?q=dummy+url+generator", "http://bit.ly/AFShASSa34d16fnosan35", string.Empty };
            string[] Protocols = { "HTTP/1.0", "HTTP/2.0", string.Empty };
            int[] ResponseCodes = { -1, 200, 204, 404, 500, 400, 422 };

            string[] Agents = {
                "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0",
                "Microsoft Office/15.0 (Windows NT 6.1; Microsoft Outlook 15.0.4631; Pro)",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299",
                "Mozilla/5.0 (Linux; Android 10; VOG-L29) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.181 Mobile Safari/537.36 OPR/61.2.3076.56749",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36 Edg/88.0.705.74",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 EdgiOS/46.1.2 Mobile/15E148 Safari/605.1.15",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36 Edge/44.19041.4788",
                string.Empty };

            Random random = new Random(int.Parse(DateTime.Now.ToString("hhmmssfff")));
            using (StreamWriter stream = File.CreateText(FileName))
            {
                int count = 0;
                for (int i = 0; i < random.Next((int)1e4, (int)1e5); i++)
                {
                    string line = $"{random.Next(1, 256)}.{random.Next(0, 256)}.{random.Next(0, 256)}.{random.Next(0, 256)} ";
                    line += $"{RfcIds[random.Next(0, RfcIds.Length)]} ";
                    line += $"{Users[random.Next(0, Users.Length)]} ";
                    line += $"[{DateTime.Now.AddMilliseconds(random.Next((int)-10368e5, (int)10368e5)).ToString(BenFatto.CLF.ClfAppSettings.Current.DateFormat, BenFatto.CLF.ClfAppSettings.Current.CultureInfo)} {(random.Next(1, 12) * 100).ToTimeZoneString()}] ";
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
            return FileName;
        }

        private static void TestSettingsReader()
        {
            WriteOut(BenFatto.CLF.DbConfiguration.Current.ConnectionString);
            WriteOut(BenFatto.CLF.ClfAppSettings.Current.BatchSize.ToString());
            WriteOut(BenFatto.CLF.ClfAppSettings.Current.AutoFlush.ToString());
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
