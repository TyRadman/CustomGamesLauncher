using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders.BattleNet
{
    internal class BattleNetGameLauncher : GameLauncher
    {
        public override ImageSource GetIconImage()
        {
            return new BitmapImage(new Uri("pack://application:,,,/GamesLauncher;component/Assets/Icons/Icon_BattleNet.png"));
        }

        public override string GetDisplayName()
        {
            return "BattleNet";
        }

        public override List<GameInfo> GetInstalledGames()
        {
            return null;
        }

        public override void RunGame(GameInfo gameInfo)
        {
            throw new NotImplementedException();
        }
    }
}
