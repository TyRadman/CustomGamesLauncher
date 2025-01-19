using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders.Steam
{
    internal class SteamGameLauncher : GameLauncher
    {
        public override ImageSource GetIconImage()
        {
            return new BitmapImage(new Uri("pack://application:,,,/GamesLauncher;component/Assets/Icons/Icon_Steam.png"));
        }

        public override string GetDisplayName()
        {
            return "Steam";
        }

        public List<string> GetSteamLibraryPaths()
        {
            string steamPath = @"C:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf";
            var libraryPaths = new List<string>();

            if (!File.Exists(steamPath))
            {
                Console.WriteLine("Steam library file not found.");
                return libraryPaths;
            }

            foreach (var line in File.ReadLines(steamPath))
            {
                if (line.Contains("\"path\""))
                {
                    string path = line.Split('"')[3];
                    libraryPaths.Add(Path.Combine(path, "steamapps"));
                }
            }

            return libraryPaths;
        }

        public override List<GameInfo> GetInstalledGames()
        {
            var libraryPaths = GetSteamLibraryPaths();
            var installedGames = new List<GameInfo>();

            foreach (var libraryPath in libraryPaths)
            {
                var manifestFiles = Directory.GetFiles(libraryPath, "appmanifest_*.acf");

                foreach (var manifestFile in manifestFiles)
                {
                    var gameInfo = ParseAppManifest(manifestFile);

                    if (gameInfo != null)
                    {
                        installedGames.Add(gameInfo);
                    }
                }
            }

            return installedGames;
        }

        private GameInfo ParseAppManifest(string manifestFile)
        {
            string[] lines = File.ReadAllLines(manifestFile);
            GameInfo gameInfo = new GameInfo();
            string installDir = string.Empty;

            foreach (var line in lines)
            {
                if (line.Contains("\"appid\""))
                {
                    gameInfo.AppId = int.Parse(line.Split('"')[3]);
                }

                if (line.Contains("\"name\""))
                {
                    gameInfo.Name = line.Split('"')[3];
                }

                if (line.Contains("\"installdir\""))
                {
                    installDir = line.Split('"')[3];
                }
            }

            string libraryPath = Path.GetDirectoryName(manifestFile);

            if (!string.IsNullOrEmpty(installDir) && libraryPath != null)
            {
                string gameDir = Path.Combine(libraryPath, "common", installDir);

                if (Directory.Exists(gameDir))
                {
                    // Search for the .exe in the game directory and subdirectories
                    string exePath = FindExecutableInDirectory(gameDir);

                    if (!string.IsNullOrEmpty(exePath))
                    {
                        gameInfo.ExecutablePath = exePath;
                        gameInfo.IconImage = GetHighResolutionIcon(exePath);
                    }
                    else
                    {
                        Console.WriteLine($"Executable not found for game: {gameInfo.Name}");
                    }
                }
            }

            gameInfo.IconUrl = $"https://cdn.cloudflare.steamstatic.com/steam/apps/{gameInfo.AppId}/header.jpg";
            return gameInfo;
        }

        private string FindExecutableInDirectory(string directoryPath)
        {
            try
            {
                // Get all .exe files in the directory and subdirectories
                var exeFiles = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);

                // Return the first .exe file found, if any
                return exeFiles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while searching for executables: {ex.Message}");
                return string.Empty;
            }
        }


        public override void RunGame(GameInfo gameInfo)
        {
            if (gameInfo == null || string.IsNullOrEmpty(gameInfo.ExecutablePath))
            {
                Console.WriteLine("Invalid game information or executable path.");
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = gameInfo.ExecutablePath,
                    UseShellExecute = true, // Ensures the process starts correctly
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch the game: {ex.Message}");
            }
        }

    }
}
