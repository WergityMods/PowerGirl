using HarmonyLib;

using Microsoft.Win32;

using System.Reflection;
using System.Diagnostics;

using System.Runtime.InteropServices;

namespace PWCrack
{
    public static class Protection
    {
        private static Harmony Instance { get; set; } = new Harmony("Protection");

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string name);

        private static void RemoveDeviceBan()
        {
            string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Audit";
            string value = "ProcessCreationIncludeCmdLine_Enabled";

            try
            {
                Registry.CurrentUser.OpenSubKey(key, true)?.DeleteValue(value, false);
            }
            catch
            {
                DialogResult dialogResult = MessageBox.Show(
                    $"Error while bypassing device ban; remove the parameter manually:\n{key} -> {value}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                if (dialogResult == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        private static void AntiBSOD()
        {
            string path = Path.Combine(
                Environment.CurrentDirectory,
                "AntiBSOD.dll"
            );

            if (LoadLibrary(path) == IntPtr.Zero)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Failed to load AntiBsod.dll",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                if (dialogResult == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        public static void Init()
        {
            AntiBSOD();
            RemoveDeviceBan();

            Hooks.Hook(typeof(Process).GetMethod(
                "EnterDebugMode"
            )!);

            Hooks.Hook(typeof(Directory).GetMethod(
                "GetFiles",
                [
                    typeof(string),
                    typeof(string)
                ]
            )!);
        }

        public static class Hooks
        {
            public static void Hook(MethodInfo method)
            {
                MethodInfo? stub = typeof(Hooks).GetMethod(method.Name)
                    ?? throw new NullReferenceException();

                Instance.Patch(method, stub);
            }

            public static bool EnterDebugMode()
            {
                return false;
            }
            
            public static bool GetFiles(ref string[] __result)
            {
                __result = ["powercheat.exe"];
                return false;
            }
        }
    }
}