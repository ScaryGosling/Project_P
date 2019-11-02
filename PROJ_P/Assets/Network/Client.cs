using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour, INetEventListener
{
    [SerializeField] private GameObject peerPrefab;
    [SerializeField] private GameObject player;

    private string ipAddress = "localhost";
    private string port;
    private string connectionStatus = "Disconnected";
    private bool isHost;
    private int peerIndex;
    private float updateSpeed = 0.1f;

    private NetManager netManager;
    private NetDataWriter writer;
    private NetPeer server;
    private GameObject[] connectedPlayers;

    public void OnGUI()
    {
        GUI.Box(new Rect(5, 5, 250, 95), "");
        GUI.Label(new Rect(10, 10, 100, 20), "IP Address:");
        ipAddress = GUI.TextField(new Rect(100, 10, 150, 20), ipAddress);
        GUI.Label(new Rect(10, 35, 100, 20), "Port:");
        port = GUI.TextField(new Rect(100, 35, 150, 20), port);
        if (GUI.Button(new Rect(10, 60, 240, 20), "Connect"))
            ConnectToServer();
        GUI.Label(new Rect(10, 80, 240, 20), connectionStatus);
    }

    public void Start()
    {
        netManager = new NetManager(this);
        writer = new NetDataWriter();
    }

    public void FixedUpdate()
    {
        netManager.PollEvents();
    }

    public void ConnectToServer()
    {
        StartCoroutine(ServerUpdate());
        connectionStatus = "Connecting...";

        if (netManager.Start())
        {
            netManager.Connect(ipAddress, int.Parse(port), "");
        }
    }

    public IEnumerator ServerUpdate()
    {
        while (true)
        {
            SendPosition();
            yield return new WaitForSeconds(updateSpeed);
        }
    }

    public void SendPosition()
    {
        if (server == null)
            return;

        writer.Reset();
        writer.Put((int)DataType.PLAYER_TRANSFORM);
        writer.Put(player.transform.position.x);
        writer.Put(player.transform.position.y);
        writer.Put(player.transform.position.z);
        writer.Put(player.transform.rotation.x);
        writer.Put(player.transform.rotation.y);
        writer.Put(player.transform.rotation.z);
        writer.Put(player.transform.rotation.w);

        server.Send(writer, DeliveryMethod.Sequenced);
    }

    public void InitalSetup(NetDataReader reader)
    {
        peerIndex = reader.GetInt();
        int maxPeers = reader.GetInt();
        connectedPlayers = new GameObject[maxPeers];

        if(peerIndex == 0)
        {
            connectionStatus = "Hosting: " + server.EndPoint.Address + ":" + server.EndPoint.Port;
            isHost = true;
        }
        else
        {
            connectionStatus = "Connected: " + server.EndPoint.Address + ":" + server.EndPoint.Port;
            isHost = false;
            for(int i = 0; i < peerIndex; i++)
            {
                connectedPlayers[i] = Instantiate(peerPrefab);
            }
        }

    }

    public void SetConnecteePosition(NetDataReader reader)
    {
        int player = reader.GetInt();
        Debug.Log("Transform recieved from connectee: " + player);

        if (connectedPlayers[player] == null)
        {
            connectedPlayers[player] = Instantiate(peerPrefab);

        }

        float xPos = reader.GetFloat();
        float yPos = reader.GetFloat();
        float zPos = reader.GetFloat();

        float xRot = reader.GetFloat();
        float yRot = reader.GetFloat();
        float zRot = reader.GetFloat();
        float wRot = reader.GetFloat();
        Debug.Log(xPos + ", " + yPos + ", " + zPos);

        connectedPlayers[player].GetComponent<NetworkPlayer>().SetNewTarget(new Vector3(xPos, yPos, zPos), new Quaternion(xRot, yRot, zRot, wRot), updateSpeed);
    }

    public void OnPeerConnected(NetPeer peer)
    {
        server = peer;
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        DataType dataType = (DataType)reader.GetInt();

        switch (dataType)
        {
            case DataType.INITIAL_SETUP:
                InitalSetup(reader);
                break;

            case DataType.PLAYER_TRANSFORM:
                SetConnecteePosition(reader);
                break;
        }
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        
    }
}

    public enum DataType
    {
        INITIAL_SETUP, PLAYER_TRANSFORM
}