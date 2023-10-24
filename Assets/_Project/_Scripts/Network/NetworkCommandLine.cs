using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Network
{
    public class NetworkCommandLine : MonoBehaviour
    {
        private void Start()
        {
            if (Application.isEditor) return;

            var args = GetCommandlineArgs();
            if (!args.TryGetValue("-mlapi", out var mlapiValue)) return;
            
            switch (mlapiValue)
            {
                case "server":
                    NetworkManager.Singleton.StartServer();
                    break;
                case "host":
                    NetworkManager.Singleton.StartHost();
                    break;
                case "client":
                    NetworkManager.Singleton.StartClient();
                    break;
            }
        }

        private Dictionary<string, string> GetCommandlineArgs()
        {
            var argsDictionary = new Dictionary<string, string>();
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; ++i)
            {
                var arg = args[i].ToLower();
                if (!arg.StartsWith("-")) continue;
                
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;
                argsDictionary.Add(arg, value);
            }
            return argsDictionary;
        }
    }
}
