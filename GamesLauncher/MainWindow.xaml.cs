using GamesLauncher.LauncherDataBuilders;
using System.Windows;
using System.Windows.Controls;

namespace GamesLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<GameLauncher> _launchers = new List<GameLauncher>()
        {
            new LauncherDataBuilders.EpicGames.EpicGameLauncher(),
            new LauncherDataBuilders.Steam.SteamGameLauncher(),
            new LauncherDataBuilders.BattleNet.BattleNetGameLauncher(),
            new LauncherDataBuilders.GOG.GOGGameLauncher()
        };

        public MainWindow()
        {
            InitializeComponent();

            BuildGrid();

            AddLaunchers();
        }

        private void BuildGrid()
        {
            // create rows
            for (int i = 0; i < _launchers.Count; i++)
            {
                RowDefinition row = new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                };

                x_mainGrid.RowDefinitions.Add(row);
            }
        }

        private void AddLaunchers()
        {
            for (int i = 0; i < _launchers.Count; i++)
            {
                GameLauncher launcher = _launchers[i];

                List<GameInfo> launcherGames = launcher.GetInstalledGames();

                //Console.WriteLine($"\n\nLoading launcher: {launcher.GetDisplayName()}. Games count: {launcherGames.Count}");

                //for (int j = 0; j < launcherGames.Count; j++)
                //{
                //    Console.WriteLine($"{launcherGames[j].Name}");
                //}

                LauncherSettings settings = new LauncherSettings()
                {
                    Launcher = launcher,
                    LauncherName = launcher.GetDisplayName(),
                    RowIndex = i,
                    IconImage = launcher.GetIconImage(),
                    Games = launcherGames
                };

                LauncherDataLoader loader = new LauncherDataLoader();
                loader.AddLoader(x_mainGrid, settings, this);
            }
        }
    }
}