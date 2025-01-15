using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders.EpicGames
{
    internal class EpicGameLauncher : GameLauncher
    {
        public override ImageSource GetIconImage()
        {
            return new BitmapImage(new Uri("pack://application:,,,/GamesLauncher;component/Assets/Icons/Icon_EpicGames.png"));
        }

        public override string GetDisplayName()
        {
            return "Epic Games";
        }

        public List<string> GetEpicLibraryPaths()
        {
            string epicManifestPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Epic",
                "EpicGamesLauncher",
                "Data",
                "Manifests"
            );

            if (!Directory.Exists(epicManifestPath))
            {
                Console.WriteLine("Epic Games manifest directory not found.");
                return new List<string>();
            }

            return new List<string> { epicManifestPath };
        }

        public override List<GameInfo> GetInstalledGames()
        {
            var manifestPaths = GetEpicLibraryPaths();
            var installedGames = new List<GameInfo>();

            foreach (var manifestPath in manifestPaths)
            {
                var manifestFiles = Directory.GetFiles(manifestPath, "*.item", SearchOption.TopDirectoryOnly);

                foreach (var manifestFile in manifestFiles)
                {
                    var gameInfo = ParseManifest(manifestFile);
                    if (gameInfo != null)
                    {
                        installedGames.Add(gameInfo);
                    }
                }
            }

            return installedGames;
        }

        private GameInfo ParseManifest(string manifestFile)
        {
            string[] lines = File.ReadAllLines(manifestFile);
            GameInfo gameInfo = new GameInfo();

            string installDir = string.Empty;

            foreach (var line in lines)
            {
                if (line.Contains("\"DisplayName\""))
                {
                    gameInfo.Name = line.Split(':')[1].Trim(' ', '"');
                }

                if (line.Contains("\"InstallLocation\""))
                {
                    installDir = line.Split(':')[1].Trim(' ', '"');
                }
            }

            if (!string.IsNullOrEmpty(installDir) && Directory.Exists(installDir))
            {
                // Search for the .exe in the install directory and subdirectories
                string exePath = FindExecutableInDirectory(installDir);
                if (!string.IsNullOrEmpty(exePath))
                {
                    gameInfo.ExecutablePath = exePath;
                }
                else
                {
                    Console.WriteLine($"Executable not found for game: {gameInfo.Name}");
                }
            }

            return gameInfo;
        }

        private string FindExecutableInDirectory(string directoryPath)
        {
            try
            {
                var exeFiles = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);
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
                    UseShellExecute = true,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch the game: {ex.Message}");
            }
        }
    }
}
