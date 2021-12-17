using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour, INetEventListener
{
    private NetManager client;
    private NetPeer server;
    private NetDataWriter writer;
    private NetPacketProcessor packetProcessor;
    private ClientPlayer player;

    public void Connect(string username)
    {
        player.username = username;
        writer = new NetDataWriter();
        packetProcessor = new NetPacketProcessor();
        packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector3());
        packetProcessor.RegisterNestedType<PlayerState>();
        packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAccept);

        client = new NetManager(this) 
        { AutoRecycle = true, };

        client.Start();
        Debug.Log("Connecting to server...");
        client.Connect("localhost", 12345, "");
    }

    public void OnJoinAccept(JoinAcceptPacket packet)
    {
        Debug.Log($"Join accepted by server (pid: {packet.state.pid})");
        player.state = packet.state;
    }

    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
    {
        if (server != null)
        {
            writer.Reset();
            packetProcessor.Write(writer, packet);
            server.Send(writer, deliveryMethod);
        }
    }

    private void Update()
    {
        if (client != null)
        {
            client.PollEvents();
        }
    }

    #region INetEventListener methods
    public void OnConnectionRequest(ConnectionRequest request)
    {
        throw new NotImplementedException();
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
        packetProcessor.ReadAllPackets(reader);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("Successful connected to server");
        server = peer;
        SendPacket(new JoinPacket { username = player.username }, DeliveryMethod.ReliableOrdered);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        throw new NotImplementedException();
    }

    #endregion
}
