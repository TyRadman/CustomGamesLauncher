using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders
{
    internal abstract class GameLauncher
    {
        public abstract ImageSource GetIconImage();
        public abstract string GetDisplayName();
        public abstract List<GameInfo> GetInstalledGames();
        public abstract void RunGame(GameInfo gameInfo);
    }

    public class GameInfo
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public BitmapImage IconImage { get; set; }
        public string ExecutablePath { get; set; }
    }
}
