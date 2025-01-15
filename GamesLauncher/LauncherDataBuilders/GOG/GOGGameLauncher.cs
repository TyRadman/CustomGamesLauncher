using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Windows.Media.Imaging;
namespace GamesLauncher.LauncherDataBuilders.GOG
{
    internal class GOGGameLauncher : GameLauncher
    {
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
            return null;
        }

        public override void RunGame(GameInfo gameInfo)
        {
            throw new NotImplementedException();
        }
    }
}
