//
// My Network GUI, based on
// https://bitbucket.org/Unity-Technologies/networking/src/2bdd9961092c4a73443c09694cff5dde9e56f9f1/Runtime/NetworkManagerHUD.cs
//
using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkGui : MonoBehaviour
{
	public static string serversAddress = "127.0.0.1";
	private NetworkManager manager;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }


    void OnGUI()
    {
        int xpos = 10;
        int ypos = 40;
        const int spacing = 24;

        if (!manager.IsClientConnected() && !NetworkServer.active)
        {
			bool noConnection = (manager.client == null || manager.client.connection == null ||
				                 manager.client.connection.connectionId == -1);

            if (noConnection)
            {
				GUI.Label (new Rect (xpos, ypos, 100, 20), "Servers address");
				serversAddress = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), serversAddress);
				ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
                {
                    manager.StartHost();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Client(C)"))
                {
                    manager.StartClient();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
                {
                    manager.StartServer();
                }
                ypos += spacing;

				manager.networkAddress = serversAddress;
            }
            else
            {
                GUI.Label(new Rect(xpos, ypos, 200, 20), "Connecting to " + manager.networkAddress + ":" + manager.networkPort + "..");
                ypos += spacing;


                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }
        else
        {
            if (NetworkServer.active)
            {
                string serverMsg = "Server: port=" + manager.networkPort;
                if (manager.useWebSockets)
                {
                    serverMsg += " (Using WebSockets)";
                }
                GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                ypos += spacing;
            }
            if (manager.IsClientConnected())
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                ypos += spacing;
            }
        }

        if (NetworkServer.active || manager.IsClientConnected())
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
            {
                manager.StopHost();
            }
        }
    }
}
