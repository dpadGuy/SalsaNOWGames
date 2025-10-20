using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace SalsaNOWGames
{
    internal class Program
    {
        private static string username = "";
        private static string password = "";
        static void Main(string[] args)
        {
            InstallStartup().GetAwaiter().GetResult();
            GameSelection().GetAwaiter().GetResult();
        }

        static async Task InstallStartup()
        {
            string salsaNowDirectory = @"I:\Apps\SalsaNOW";
            string zipPath = Path.Combine(salsaNowDirectory, "DepotDownloader-windows-x64.zip");
            string extractPath = Path.Combine(salsaNowDirectory, "DepotDownloader");
            string depotDownloaderUrl = "https://github.com/dpadGuy/SalsaNOWThings/releases/download/Things/DepotDownloader-windows-x64.zip";

            try
            {
                Directory.CreateDirectory(salsaNowDirectory);

                if (!Directory.Exists(extractPath))
                {
                    // https://github.com/SteamRE/DepotDownloader
                    using (WebClient webClient = new WebClient())
                    {
                        Console.WriteLine("Downloading DepotDownloader...");
                        Console.WriteLine();
                        await webClient.DownloadFileTaskAsync(new Uri(depotDownloaderUrl), zipPath);
                    }

                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task GameSelection()
        {
            string gamesDirectory = @"I:\Apps\SalsaNOW\DepotDownloader\Games";

            Console.Title = "SalsaNOW Games - Steam";
            Console.Clear();

            Console.WriteLine("https://github.com/SteamRE/DepotDownloader");
            Console.WriteLine("https://github.com/dpadGuy/SalsaNOWGames");
            Console.WriteLine();

            if (username == "" || password == "")
            {
                Console.Write("Enter your name: ");
                username = Console.ReadLine();

                Console.WriteLine("WARNING: PASSWORD WILL BE VISIBLE !!!");
                Thread.Sleep(2000);
                Console.Write("Enter your password: ");
                password = Console.ReadLine();
            }

            Console.Write("Enter App ID to install game: ");
            string appID = Console.ReadLine();

            Directory.CreateDirectory($"{gamesDirectory}\\{appID}");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = @"I:\Apps\SalsaNOW\DepotDownloader\DepotDownloader.exe",
                Arguments = $"-app {appID} -username {username} -password {password} -os windows -no-mobile -dir {gamesDirectory}\\{appID}",
            };

            Console.Title = $"SalsaNOW Games - Steam \"{gamesDirectory}\\{appID}\"";

            Console.Clear();
            Process process = Process.Start(psi);
            process.WaitForExit();

            Process.Start("I:\\Apps\\SalsaNOW\\Explorer++.exe", $"{gamesDirectory}\\{appID}");

            GameSelection();
            return;
        }
    }
}
