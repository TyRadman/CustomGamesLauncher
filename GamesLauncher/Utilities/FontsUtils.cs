using System.IO;
using System.Windows;
using System.Windows.Media;

namespace GamesLauncher.Utilities
{
    internal static class FontsUtils
    {
        public enum FontType
        {
            Thin = 0,
            Light = 1,
            Regular = 2,
            Medium = 3,
            Bold = 4,
            SemiBold = 5,
            Black = 6,
        }

        private const string FONT_PATH_PREFIX = "pack://application:,,,/GamesLauncher;component/Assets/Fonts/Commissioner-";

        private static readonly Dictionary<FontType, FontFamily> _fontTypeToFamily = new Dictionary<FontType, FontFamily>()
        {
            { FontType.Thin,  new FontFamily($"{FONT_PATH_PREFIX}Black.ttf") },
            {FontType.Light, new FontFamily($"{FONT_PATH_PREFIX}Light.ttf") },
            {FontType.Regular, new FontFamily($"{FONT_PATH_PREFIX}Regular.ttf") },
            {FontType.Medium, new FontFamily($"{FONT_PATH_PREFIX}Medium.ttf") },
            {FontType.Bold, new FontFamily($"{FONT_PATH_PREFIX}Bold.ttf") },
            {FontType.SemiBold, new FontFamily($"{FONT_PATH_PREFIX}SemiBold.ttf") },
            //{FontType.Black, new FontFamily($"{FONT_PATH_PREFIX}Black.ttf") },
            { FontType.Black, new FontFamily(new Uri("pack://application:,,,/"), "./Assets/Fonts/#Commissioner-Black") }

        };

        public static FontFamily GetFont(FontType fontType)
        {
            FontFamily fontt = _fontTypeToFamily[fontType];

            //ListFonts("C:\\Users\\timde\\Documents\\GitHub\\GamesLauncher\\GamesLauncher\\GamesLauncher\\Assets\\Fonts\\");

            if (!_fontTypeToFamily.TryGetValue(fontType, out FontFamily font))
            {
                Console.WriteLine($"Font for {fontType} not found");
                return fontt;
            }
            // Test if the font is valid
            try
            {
                Typeface typeface = new Typeface(font, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                GlyphTypeface glyphTypeface;
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                {
                    Console.WriteLine($"Font {font.Source} is invalid or cannot be used.");
                    return fontt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading font: {ex.Message}");
                return fontt;
            }

            Console.WriteLine($"Font {font.Source} loaded successfully.");
            return fontt;
        }

        public static void ListFonts(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Directory not found: {path}");
                return;
            }

            // Get all font files with .ttf or .otf extensions
            string[] fontFiles = Directory.GetFiles(path, "*.ttf", SearchOption.AllDirectories);
            fontFiles = fontFiles.Concat(Directory.GetFiles(path, "*.otf", SearchOption.AllDirectories)).ToArray();

            if (fontFiles.Length == 0)
            {
                Console.WriteLine("No font files found in the directory.");
                return;
            }

            Console.WriteLine("Found Font Files:");
            foreach (var fontFile in fontFiles)
            {
                Console.WriteLine(Path.GetFileName(fontFile));
            }
        }
    }
}
