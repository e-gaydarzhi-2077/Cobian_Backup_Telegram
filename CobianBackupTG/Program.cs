using System;
using System.IO;
using System.Linq;
using System.Net;

namespace CobianBackupTG
{
    class Program
    {
        static void Main(string[] args)
        {
            //Vars
            string DirExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string PathLog = "C:\\Program Files (x86)\\Cobian Backup 11\\Logs\\";
            string PathLogTemp = "C:\\Program Files (x86)\\Cobian Backup 11\\Logs\\temp\\";
            string BotID = File.ReadLines(Path.GetDirectoryName(DirExe) + "\\CobianBackupTG.cfg").ElementAtOrDefault(0);
            string BotChatID = File.ReadLines(Path.GetDirectoryName(DirExe) + "\\CobianBackupTG.cfg").ElementAtOrDefault(1);

            //Create Temp if not exist
            Directory.CreateDirectory(PathLogTemp);

            if (File.Exists(PathLog + "log " + DateNow + ".txt"))
            {
                //Copy Opened File To Temp Folder
                File.Copy(PathLog + "log " + DateNow + ".txt", PathLogTemp + "log " + DateNow + ".txt", true);

                //Get Public IP
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                var externalIp = IPAddress.Parse(externalIpString);

                //Read File
                var lines = File.ReadLines(PathLogTemp + "log " + DateNow + ".txt");
                string result = string.Join("\n", lines.Where(s => s.IndexOf("ERR", StringComparison.InvariantCultureIgnoreCase) >= 0));

                try
                {
                    //Check results if empty
                    if (result == "")
                    {
                        Console.WriteLine("Log file not have errors");
                    }
                    else
                    {
                        //Check results if have upper characters contains
                        if (!result.Contains("ERR"))
                        {
                            Console.WriteLine("ERR: " + result.Contains("ERR") + "!");
                        }
                        else
                        {
                            //Send result in Telegram
                            string GetBot = "Error Cobian Backup ⚠️ " + "\n" + "User: " + Environment.UserName + "\n" + "Computer: " + Environment.MachineName + "\n" + "IP-Adress: " + externalIp.ToString() + "\n" + "`" + result + "`";
                            WebRequest reqGET = WebRequest.Create(@"https://api.telegram.org/bot" + BotID + "/sendMessage?chat_id=" + BotChatID + "&parse_mode=Markdown&text=" + GetBot);
                            WebResponse resp = reqGET.GetResponse();
                            Stream stream = resp.GetResponseStream();
                            StreamReader sr = new StreamReader(stream);
                            Console.WriteLine("Errors Finded");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("File cannot be opened");
                }
                finally
                {
                    //Delete temp file and exit
                    File.Delete(PathLogTemp + "log " + DateNow + ".txt");
                    Directory.GetFiles(PathLogTemp).ToList().ForEach(File.Delete);
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("File not exists");
                Environment.Exit(0);
            }
        }
    }
}

