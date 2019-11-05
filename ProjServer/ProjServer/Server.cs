using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;

namespace ProjServer
{
    public class Server : INetEventListener
    {

        private NetManager serverNetManager;
        private NetDataWriter writer;
        private int port = 2500;
        private const int MAX_PEERS = 16;
        private NetPeer[] connectedPeers = new NetPeer[MAX_PEERS];

        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            Console.ReadKey();
        }

        public void Run()
        {
            serverNetManager = new NetManager(this);
            writer = new NetDataWriter();

            if (serverNetManager.Start(port))
            {
                Console.WriteLine("Server open on port:" + port);
            }


            while (serverNetManager.IsRunning) {

                serverNetManager.PollEvents();
                System.Threading.Thread.Sleep(50);

            }

        }

        public void SpreadTransform(NetDataReader reader, NetPeer peer)
        {
            float xPos = reader.GetFloat();
            float yPos = reader.GetFloat();
            float zPos = reader.GetFloat();

            float xRot = reader.GetFloat();
            float yRot = reader.GetFloat();
            float zRot = reader.GetFloat();
            float wRot = reader.GetFloat();
            
            writer.Reset();
            writer.Put((int)DataType.PLAYER_TRANSFORM);
            writer.Put(peer.Id);
            writer.Put(xPos);
            writer.Put(yPos);
            writer.Put(zPos);
            writer.Put(xRot);
            writer.Put(yRot);
            writer.Put(zRot);
            writer.Put(wRot);


            for (int i = 0; i < connectedPeers.Length; i++)
            {
                if (connectedPeers[i] == null)
                    break;
                if (i == peer.Id)
                    continue;

                connectedPeers[i].Send(writer, DeliveryMethod.Sequenced);
            }
        }

        #region Network Events
        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.Accept();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            DataType dataType = (DataType)reader.GetInt();

            switch (dataType) {

                case DataType.PLAYER_TRANSFORM:
                    SpreadTransform(reader, peer);
                    break;
            
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine(peer.EndPoint.Address + " has connected.");

            writer.Reset();
            writer.Put((int)DataType.INITIAL_SETUP);
            writer.Put(peer.Id);
            writer.Put(MAX_PEERS);
            peer.Send(writer, DeliveryMethod.Sequenced);
            Console.WriteLine(peer.Id);
            connectedPeers[peer.Id] = peer;
            
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            
        }
        #endregion
    }
}

public enum DataType
{
    INITIAL_SETUP, PLAYER_TRANSFORM
}
