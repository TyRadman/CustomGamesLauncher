using GamesLauncher.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesLauncher.LauncherDataBuilders
{
    internal class LauncherDataLoader
    {
        private Grid? _mainGrid;
        private Window _parentWindow;
        private LauncherSettings _settings;

        private readonly int _launcherButtonsColumnIndex = 0;
        private readonly int _headerIconMaxSize = 100;

        public LauncherDataLoader()
        {
            _settings = new LauncherSettings();
        }

        public void AddLoader(Grid parentGrid, LauncherSettings settings, Window parentWindow)
        {
            if (parentGrid == null)
            {
                throw new ArgumentNullException(nameof(parentGrid));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if(parentWindow == null)
            {
                throw new ArgumentNullException(nameof(parentWindow));
            }

            _parentWindow = parentWindow;

            _mainGrid = parentGrid;
            _settings = settings;

            AddLauncherHeader();

            AddLauncherGames();
        }

        private void AddLauncherHeader()
        {
            string buttonText = !String.IsNullOrEmpty(_settings.LauncherName) ? _settings.LauncherName : "Undefined";

            Grid titleGrid = new Grid();

            titleGrid.SetValue(Grid.ColumnProperty, _launcherButtonsColumnIndex);
            titleGrid.SetValue(Grid.RowProperty, _settings.RowIndex);

            ColumnDefinition iconColumn = new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            };

            ColumnDefinition nameColumn = new ColumnDefinition()
            {
                Width = new GridLength(4, GridUnitType.Star)
            };

            titleGrid.ColumnDefinitions.Add(iconColumn, nameColumn);

            Image icon = new Image()
            {
                Source = _settings.IconImage,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                MaxHeight = _headerIconMaxSize,
                MaxWidth = _headerIconMaxSize,
                Margin = new Thickness(5)
            };
            icon.SetValue(Grid.ColumnProperty, 0);

            TextBlock nameTextBlock = new TextBlock()
            {
                Text = _settings.LauncherName,
                VerticalAlignment = VerticalAlignment.Center,
                //FontFamily = FontsUtils.GetFont(FontsUtils.FontType.Black),
                FontFamily = new FontFamily("./#Commissioner"),
                FontSize = 20,
                FontStyle = FontStyles.Normal,
                FontWeight = FontWeights.Black,
            };
            nameTextBlock.SetValue(Grid.ColumnProperty, 1);

            titleGrid.Children.Add(icon, nameTextBlock);

            _mainGrid?.Children.Add(titleGrid);
        }
        
        private void AddLauncherGames()
        {
            var games = _settings.Games;

            if(games == null || games.Count == 0)
            {
                return;
            }

            ScrollViewer gamesScrollViewer = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(5),
                Background = Brushes.Transparent
            };

            StackPanel stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent
            };
            gamesScrollViewer.Content = stack;

            for (int i = 0; i < games.Count; i++)
            {
                GameInfo game = games[i];

                Button gameButton = CreateGameButton(game);
                stack.Children.Add(gameButton);
                Console.WriteLine($"{_settings.LauncherName}: Added game button: {gameButton.Content}");
            }

            Grid.SetColumn(gamesScrollViewer, 1);
            _mainGrid?.Children.Add(gamesScrollViewer);
        }

        private Button CreateGameButton(GameInfo game)
        {
            ImageBrush buttonBackgroundBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(game.IconUrl)),
                Stretch = Stretch.UniformToFill
            };

            object styleObject = _parentWindow.TryFindResource("GameButtonStyle");

            if(styleObject is not Style gameButtonStyle)
            {
                throw new InvalidOperationException("GameButtonStyle not found in resources.");
            }

            Button gameButton = new Button()
            {
                Content = game.Name,
                Style = gameButtonStyle
            };

            gameButton.Background = buttonBackgroundBrush;


            gameButton.Click += (object sender, RoutedEventArgs e) => GameButton_Click(sender, e, game);

            return gameButton;
        }

        private void GameButton_Click(object sender, RoutedEventArgs e, GameInfo game)
        {
            _settings.Launcher.RunGame(game);
            Console.WriteLine($"Game button clicked: {game.Name}");
        }
    }

    public class LauncherSettings
    {
        public string LauncherName { get; set; } = "Undefined";
        public int RowIndex { get; set; } = -1;
        public ImageSource IconImage { get; set; }
        public List<GameInfo> Games { get; set; }
        internal GameLauncher Launcher { get; set; }
    }
}
