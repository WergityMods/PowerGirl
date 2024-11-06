using HarmonyLib;

using System.Net;
using System.Net.Sockets;

using System.Reflection;

namespace PWCrack
{
    public static class Requests
    {
        private static string Host { get; set; } = "127.0.0.1";

        private static Harmony Instance { get; set; } = new Harmony("Requests");

        public static void Init()
        {
            Hooks.Hook(typeof(WebClient).GetMethod(
                "DownloadString",
                [
                    typeof(string)
                ]
            )!);

            Hooks.Hook(typeof(HttpClient).GetMethod(
                "GetAsync",
                [
                    typeof(string)
                ]
            )!);

            Hooks.Hook(typeof(HttpClient).GetMethod(
                "PostAsync",
                [
                    typeof(string),
                    typeof(HttpContent)
                ]
            )!);

            Hooks.Hook(typeof(TcpClient).GetMethod(
                "ConnectAsync",
                [
                    typeof(string),
                    typeof(int)
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

            public static bool DownloadString(ref string address)
            {
                // icanhazip.com
                address = $"http://{Host}/ip";

                return true;
            }

            public static bool GetAsync(ref string requestUri)
            {
                // https://api.cheat.pw/api.json
                requestUri = $"http://{Host}/api";

                return true;
            }

            public static bool PostAsync(ref string requestUri)
            {
                // https://api.cheat.pw/authenticate
                requestUri = $"http://{Host}/auth";

                return true;
            }

            public static bool ConnectAsync(ref string host)
            {
                // 194.87.189.235
                host = Host;

                return true;
            }
        }
    }
}