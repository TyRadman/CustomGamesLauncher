using GamesLauncher.LauncherDataBuilders;
using GamesLauncher.LauncherDataBuilders.Steam;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GamesLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int _columnsCount = 2;

        private readonly List<GameLauncher> _launchers = new List<GameLauncher>()
        {
            new LauncherDataBuilders.Steam.SteamGameLauncher(),
            new LauncherDataBuilders.EpicGames.EpicGameLauncher(),
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
            //x_mainGrid.Children.Clear();

            // create columns
            //ColumnDefinition column = new ColumnDefinition()
            //{
            //    Width = new GridLength(1, GridUnitType.Star)
            //};

            //ColumnDefinition gamesColumn = new ColumnDefinition()
            //{
            //    Width = new GridLength(3, GridUnitType.Star)
            //};

            //x_mainGrid.ColumnDefinitions.Add(column);
            //x_mainGrid.ColumnDefinitions.Add(gamesColumn);

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

                Console.WriteLine($"\n\nLoading launcher: {launcher.GetDisplayName()}. Games count: {launcher.GetInstalledGames().Count}");

                for (int j = 0; j < launcher.GetInstalledGames().Count; j++)
                {
                    Console.WriteLine($"{launcher.GetInstalledGames()[j].Name}");
                }

                LauncherSettings settings = new LauncherSettings()
                {
                    Launcher = launcher,
                    LauncherName = launcher.GetDisplayName(),
                    RowIndex = i,
                    IconImage = launcher.GetIconImage(),
                    Games = launcher.GetInstalledGames()
                };

                LauncherDataLoader loader = new LauncherDataLoader();
                loader.AddLoader(x_mainGrid, settings, this);

                //if (launcher is SteamGameLauncher steam)
                //{
                //    List<string> libraryPaths = steam.GetSteamLibraryPaths();
                //    Console.WriteLine(libraryPaths.Count);
                //    StringBuilder sb = new StringBuilder();

                //    foreach (var path in libraryPaths)
                //    {
                //        Console.WriteLine(path);
                //    }

                //    var installedGames = steam.GetInstalledSteamGames();

                //    foreach (var game in installedGames)
                //    {
                //        Console.WriteLine($"{game.Name} - AppID: {game.AppId}");
                //        Console.WriteLine($"Icon: {game.IconUrl}");
                //    }
                //}
            }
        }
    }
}