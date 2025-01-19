using GamesLauncher.Utilities;
using System.Diagnostics;
using System.Drawing;
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

        public override List<GameInfo> GetInstalledGames()
        {
            List<string> manifestPaths = GetEpicLibraryPaths();
            List<GameInfo> installedGames = new List<GameInfo>();

            foreach (string manifestPath in manifestPaths)
            {
                string[] manifestFiles = Directory.GetFiles(manifestPath, "*.item", SearchOption.TopDirectoryOnly);

                foreach (string manifestFile in manifestFiles)
                {
                    GameInfo gameInfo = ParseManifest(manifestFile);

                    if (gameInfo != null)
                    {
                        //Console.WriteLine($"Added game {gameInfo.Name}");
                        installedGames.Add(gameInfo);
                    }
                }
            }

            return installedGames;
        }

        private List<string> GetEpicLibraryPaths()
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

        private GameInfo ParseManifest(string manifestFile)
        {
            string[] lines = File.ReadAllLines(manifestFile);
            GameInfo gameInfo = new GameInfo();

            string installDir = string.Empty;
            string catalogNamespace = string.Empty;
            string catalogItemId = string.Empty;

            foreach (string line in lines)
            {
                //Console.WriteLine($"EPIC Line: {line}");

                if (line.Contains("\"bIsApplication\""))
                {
                    string isApplication = line.Split(':')[1].Trim(' ', '"').Trim(',');

                    if(isApplication == "false")
                    {
                        return null;
                    }
                }

                if (line.Contains("\"DisplayName\""))
                {
                    gameInfo.Name = line.Split(':')[1].Trim(' ', '"').Trim().Trim(',').Trim('"');
                }

                if (line.Contains("\"InstallLocation\""))
                {
                    string[] splits = line.Split(':');
                    installDir = $"{splits[1]}:{splits[2]}".Trim(' ').Trim(',').Trim().Trim('"');
                }
            }
            
            if (!string.IsNullOrEmpty(installDir) && Directory.Exists(installDir))
            {
                // Search for the .exe in the install directory and subdirectories
                string exePath = FindExecutableInDirectory(installDir);

                if (!string.IsNullOrEmpty(exePath))
                {
                    gameInfo.ExecutablePath = exePath;
                    Helper.Debug($"GEtting icon for {exePath}");
                    gameInfo.IconImage = GetHighResolutionIcon(exePath);
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
                string[] exeFiles = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);

                if(exeFiles.Length == 0)
                {
                    Console.WriteLine($"No executables found in {directoryPath}");
                    return string.Empty;
                }

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
