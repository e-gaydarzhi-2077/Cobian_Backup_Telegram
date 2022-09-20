using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CobianBackupTG
{
    class Program
    {
        static void Main(string[] args)
        {
            //Vars
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string PathLog = "C:\\Program Files (x86)\\Cobian Backup 11\\Logs\\";
            string PathLogTemp = "C:\\Program Files (x86)\\Cobian Backup 11\\Logs\\temp\\";
            string BotID = File.ReadLines(System.Environment.CurrentDirectory + "\\CobianBackupTG.cfg").ElementAtOrDefault(0);
            string BotChatID = File.ReadLines(System.Environment.CurrentDirectory + "\\CobianBackupTG.cfg").ElementAtOrDefault(1);

            //Create Temp if not exist
            System.IO.Directory.CreateDirectory(PathLogTemp);

            if (File.Exists(PathLog + "log " + DateNow + ".txt"))
            {
                Console.WriteLine("File exists");

                //Copy Opened File
                System.IO.File.Copy(PathLog + "log " + DateNow + ".txt", PathLogTemp + "log " + DateNow + ".txt", true);

                //Get Public IP
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                var externalIp = IPAddress.Parse(externalIpString);

                //Read File
                var lines = File.ReadLines(PathLogTemp + "log " + DateNow + ".txt");
                string result = string.Join("\n", lines.Where(s => s.IndexOf("ERR", StringComparison.InvariantCultureIgnoreCase) >= 0));
                Console.WriteLine(result);

                try
                {
                    //Check results if empty
                    if (result == "")
                    {
                        Console.WriteLine("Log file not have errors");
                    }
                    else
                    {
                        //Send result in Telegram
                        string GetBot = "Ошибка Cobian Backup ⚠️ " + "\n" + "Пользователь: " + Environment.UserName + "\n" + "Компьютер: " + Environment.MachineName + "\n" + "IP-Адресс: " + externalIp.ToString() + "\n" + "`" + result + "`";
                        System.Net.WebRequest reqGET = System.Net.WebRequest.Create(@"https://api.telegram.org/bot" + BotID + "/sendMessage?chat_id=" + BotChatID + "&parse_mode=Markdown&text=" + GetBot);
                        System.Net.WebResponse resp = reqGET.GetResponse();
                        System.IO.Stream stream = resp.GetResponseStream();
                        System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                        Console.WriteLine("Ошибки найдены и отправлены");
                    }
                }
                catch
                {
                    Console.WriteLine("File cannot be opened");
                }
                finally
                {
                    //Delete temp file and exit
                    System.IO.File.Delete(PathLogTemp + "log " + DateNow + ".txt");
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

