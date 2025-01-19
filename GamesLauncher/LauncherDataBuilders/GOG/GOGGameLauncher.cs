using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Windows.Media.Imaging;
namespace GamesLauncher.LauncherDataBuilders.GOG
{
    internal class GOGGameLauncher : GameLauncher
    {
        private const string PARENT_DIRECTORY = "GOG";

        public override ImageSource GetIconImage()
        {
            return new BitmapImage(new Uri("pack://application:,,,/GamesLauncher;component/Assets/Icons/Icon_GOG.png"));
        }

        public override string GetDisplayName()
        {
            return "GOG";
        }

        public override List<GameInfo> GetInstalledGames()
        {

            List<GameInfo> installedGames = new List<GameInfo>(); 
            string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string rootPath = Path.GetPathRoot(systemPath);

            // Get all drives to search
            string[] pahts = [
                    @"c:\",
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                ];

            foreach (string path in pahts)
            {
                try
                {
                    // Recursively search for game folders
                    string gamesParentPath = Path.Combine(path, PARENT_DIRECTORY);

                    if(!Directory.Exists(gamesParentPath))
                    {
                        Console.WriteLine($"Directory {gamesParentPath} not found.");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(gamesParentPath))
                    {
                        // Find the executable file in the directory
                        string exePath = GetLargestExecutable(FindExecutableInDirectory(gamesParentPath));

                        if (!string.IsNullOrEmpty(exePath))
                        {
                            List<string> files = exePath.Split('\\').ToList();
                            string gameName = files.Last().Split('.')[0];

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
                    Console.WriteLine($"Error search for GOG Games");
                }
            }

            return installedGames;
        }

        public override void RunGame(GameInfo gameInfo)
        {
            throw new NotImplementedException();
        }
    }
}
