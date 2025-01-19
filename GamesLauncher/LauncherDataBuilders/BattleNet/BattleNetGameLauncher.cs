using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders.BattleNet
{
    internal class BattleNetGameLauncher : GameLauncher
    {
        private static readonly List<string> BattleNetGames = new List<string>
        {   
            "World of Warcraft",
            "Overwatch",
            "Diablo III",
            "Diablo IV",
            "Hearthstone",
            "Starcraft II",
            "Heroes of the Storm",
            "Call of Duty: Warzone",
            "Call of Duty: Modern Warfare",
            "Call of Duty: Black Ops Cold War",
            "Call of Duty: Vanguard",
            "Warcraft III"
        };

        public override ImageSource GetIconImage()
        {
            return new BitmapImage(new Uri("pack://application:,,,/GamesLauncher;component/Assets/Icons/Icon_BattleNet.png"));
        }

        public override string GetDisplayName()
        {
            return "Battle.net";
        }

        public override List<GameInfo> GetInstalledGames()
        {
            List<GameInfo> installedGames = new List<GameInfo>();

            // Get all drives to search
            string[] pahts = [
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                ];

            foreach (string drive in pahts)
            {

                foreach (string gameName in BattleNetGames)
                {
                    try
                    {
                        // Recursively search for game folders
                        string gamePath = FindGameDirectory(drive, gameName);

                        if (!string.IsNullOrEmpty(gamePath))
                        {
                            // Find the executable file in the directory
                            string exePath = FindExecutableInDirectory(gamePath);

                            if (!string.IsNullOrEmpty(exePath))
                            {
                                // Create GameInfo for the discovered game
                                GameInfo gameInfo = new GameInfo
                                {
                                    Name = gameName,
                                    ExecutablePath = exePath,
                                    IconImage = GetHighResolutionIcon(exePath)
                                };

                                installedGames.Add(gameInfo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error searching for {gameName}: {ex.Message}");
                    }
                }
            }

            return installedGames;
        }

        private string FindGameDirectory(string rootPath, string gameName)
        {
            string gamePath = Path.Combine(rootPath, gameName);

            if (!Directory.Exists(gamePath))
            {
                Console.WriteLine($"Game {gameName} wasn't found in {rootPath}");
                return string.Empty;
            }

            return gamePath;
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
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch the game: {ex.Message}");
            }
        }
    }
}
