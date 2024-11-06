using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace PWCrack
{
    public static class Program
    {
        private static void ShowInfo()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://t.me/Wergity_Mods",
                UseShellExecute = true
            });

            MessageBox.Show(
                "By subscribing to the channel, you will support me and will be able to play with new cracks",
                "Cracked by Wergity",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        public static void Main()
        {
            ShowInfo();

            Assembly assembly = Assembly.LoadFile(Path.Combine(
                Environment.CurrentDirectory,
                "powercheattt.dll"
            ));

            Protection.Init();
            Requests.Init();

            assembly.EntryPoint?.Invoke(null, null);
        }
    }
}