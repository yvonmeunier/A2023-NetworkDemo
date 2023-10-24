using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace SimpleNetworkDemo.Network
{
    public class NetworkManagerHUD : MonoBehaviour
    {
        [SerializeField] private bool _simulateLag = false;
        [SerializeField] private int _packetDelay = 120;
        [SerializeField] private int _packetJitter = 5;
        [SerializeField] private int _dropRate = 3;
        
        private static bool HasStartedAsClientOrServer => NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
        
        private void Awake() 
        {
            // NOTE: "Simulate Lag" checkbox requires to restart the game to take effect
            if (_simulateLag)
            {
                GetComponentInParent<UnityTransport>().SetDebugSimulatorParameters(
                    packetDelay: _packetDelay,
                    packetJitter: _packetJitter,
                    dropRate: _dropRate);
            }
        }
        
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            
            if (HasStartedAsClientOrServer)
            {
                DrawStatusInformation();
            }
            else
            {
                DrawStartButtons();
            }
            
            GUILayout.EndArea();
        }

        private static void DrawStatusInformation()
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        private static void DrawStartButtons()
        {
            DrawButton("Host", NetworkManager.Singleton.StartHost);
            DrawButton("Server", NetworkManager.Singleton.StartServer);
            DrawButton("Client", NetworkManager.Singleton.StartClient);
        }

        private static void DrawButton(string text, Func<bool> onClick)
        {
            if (GUILayout.Button(text))
            {
                onClick();
            }
        }
    }
}