using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders
{
    class GeneralGameLauncher : GameLauncher
    {
        public override string GetDisplayName()
        {
            return "All Games";
        }

        public override ImageSource GetIconImage()
        {
            return new BitmapImage(new Uri("pack://application:,,,/GamesLauncher;component/Assets/Icon_Games.png"));
        }

        public override List<GameInfo> GetInstalledGames()
        {
            var games = new List<GameInfo>();
            var searchDirectories = new List<string>
                {
                    @"C:\Program Files",
                    @"C:\Program Files (x86)"
                };


            foreach (var directory in searchDirectories)
            {
                try
                {
                    var subDirectories = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories);

                    foreach (var subDirectory in subDirectories)
                    {
                        try
                        {
                            var executables = Directory.GetFiles(subDirectory, "*.exe", SearchOption.TopDirectoryOnly);

                            foreach (var exePath in executables)
                            {
                                var gameName = Path.GetFileNameWithoutExtension(exePath);

                                // Extract icon
                                var iconImage = ExtractIcon(exePath);
                                Console.WriteLine($"Found game: {gameName}");

                                games.Add(new GameInfo
                                {
                                    Name = gameName,
                                    ExecutablePath = exePath,
                                    IconImage = iconImage
                                });
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine($"Access denied to directory: {subDirectory}");
                            continue; // Skip this folder
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"Access denied to base directory: {directory}");
                    continue; // Skip this base directory
                }
            }

            return games;
        }

        public override void RunGame(GameInfo gameInfo)
        {
            try
            {
                Process.Start(gameInfo.ExecutablePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error launching game {gameInfo.Name}: {ex.Message}");
            }
        }

        private BitmapImage ExtractIcon(string exePath)
        {
            try
            {
                var icon = Icon.ExtractAssociatedIcon(exePath);

                if (icon != null)
                {
                    using (var iconStream = new MemoryStream())
                    {
                        icon.Save(iconStream);
                        iconStream.Seek(0, SeekOrigin.Begin);

                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = iconStream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        return bitmapImage;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error extracting icon from {exePath}: {ex.Message}");
            }

            return null;
        }
    }
}
