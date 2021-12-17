using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour, INetEventListener
{
    private NetManager server;
    public Vector3 initialPosition = new Vector3();
    private NetDataWriter writer;
    private NetPacketProcessor packetProcessor;
    private Dictionary<uint, ServerPlayer> players = new Dictionary<uint, ServerPlayer>();

    private void Start()
    {
        server = new NetManager(this)
        {
            AutoRecycle = true,
        };

        Debug.Log("Starting server...");
        server.Start(12345);
    }

    private void Update()
    {
        server.PollEvents();
    }

    #region INetEventListener methods
    public void OnConnectionRequest(ConnectionRequest request)
    {
        Debug.Log($"Incoming connecting request from {request.RemoteEndPoint.ToString()}");
        request.Accept();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        throw new NotImplementedException();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        throw new NotImplementedException();
    }
    #endregion
}
