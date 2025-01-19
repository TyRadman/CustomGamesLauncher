using GamesLauncher.Utilities;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
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

        protected BitmapImage GetHighResolutionIcon(string filePath)
        {
            filePath = filePath.Replace("/", "\\").Replace("\\\\", "\\");

            Helper.Debug($"GEtting icon for {filePath}");
            try
            {
                Helper.Debug($"GEtting icon for {filePath}");
                var hIcon = GetIconFromShell(filePath);

                if (hIcon != IntPtr.Zero)
                {
                    using (var icon = Icon.FromHandle(hIcon))
                    {
                        using (MemoryStream iconStream = new MemoryStream())
                        {
                            icon.ToBitmap().Save(iconStream, ImageFormat.Png);
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
            }
            catch (Exception ex)
            {
                Helper.Debug($"Error extracting high-resolution icon from {filePath}: {ex.Message}");
            }

            return null;
        }

        private IntPtr GetIconFromShell(string filePath)
        {
            SHFILEINFO shinfo = new SHFILEINFO();

            // SHGFI_SYSICONINDEX = Retrieve the system index for the icon
            const uint SHGFI_SYSICONINDEX = 0x4000;
            const uint SHGFI_LARGEICON = 0x0000;

            IntPtr hImageList = SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_SYSICONINDEX | SHGFI_LARGEICON);

            if (hImageList == IntPtr.Zero)
            {
                Helper.Debug($"Failed to get icon from shell for {filePath}");
                return IntPtr.Zero;
            }

            IntPtr hIcon = ImageList_GetIcon(hImageList, shinfo.iIcon, 0x0001); // ILD_TRANSPARENT
            return hIcon;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon; // Icon handle
            public int iIcon;    // Index in the system image list
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("Comctl32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, uint flags);

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
