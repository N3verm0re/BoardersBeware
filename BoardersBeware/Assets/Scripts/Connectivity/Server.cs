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
        writer = new NetDataWriter();
        packetProcessor = new NetPacketProcessor();
        packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector3());
        packetProcessor.RegisterNestedType<PlayerState>();
        packetProcessor.SubscribeReusable<JoinPacket, NetPeer>(OnJoinReceived);
        packetProcessor.RegisterNestedType<ClientPlayer>();
        packetProcessor.SubscribeReusable<PlayerSendUpdatePacket, NetPeer>(OnPlayerUpdate);

        server = new NetManager(this)
        {
            AutoRecycle = true,
        };

        Debug.Log("Starting server...");
        server.Start(12345);
    }

    public void SendPacket<T>(T packet, NetPeer peer, DeliveryMethod deliveryMethod) where T : class, new()
    {
        if (peer != null)
        {
            writer.Reset();
            packetProcessor.Write(writer, packet);
            peer.Send(writer, deliveryMethod);
        }
    }

    public void OnJoinReceived(JoinPacket packet, NetPeer peer)
    {
        Debug.Log($"Received join from {packet.username} (pid: {(uint)peer.Id})");

        ServerPlayer newPlayer = (players[(uint)peer.Id] = new ServerPlayer
        {
            peer = peer,
            state = new PlayerState
            {
                pid = (uint)peer.Id,
                position = initialPosition,
            },
            username = packet.username,
        });

        SendPacket(new JoinAcceptPacket { state = newPlayer.state }, peer, DeliveryMethod.ReliableOrdered);

        foreach (ServerPlayer player in players.Values)
        {
            if (player.state.pid != newPlayer.state.pid)
            {
                SendPacket(new PlayerJoinedGamePacket
                {
                    player = new ClientPlayer
                    {
                        username = newPlayer.username,
                        state = newPlayer.state,
                    },
                }, player.peer, DeliveryMethod.ReliableOrdered);

                SendPacket(new PlayerJoinedGamePacket
                {
                    player = new ClientPlayer
                    {
                        username = player.username,
                        state = player.state,
                    },
                }, newPlayer.peer, DeliveryMethod.ReliableOrdered);
            }
        }
    }

    public void OnPlayerUpdate(PlayerSendUpdatePacket packet, NetPeer peer)
    {
        players[(uint)peer.Id].state.position = packet.position;
    }

    private void Update()
    {
        server.PollEvents();
        PlayerState[] states = new PlayerState[players.Count];

        int i = 0;
        foreach (ServerPlayer player in players.Values)
        {
            states[i] = player.state;
            i++;
        }

        foreach (ServerPlayer player in players.Values)
        {
            SendPacket(new PlayerReceiveUpdatePacket { states = states }, player.peer, DeliveryMethod.Unreliable);
        }
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
        packetProcessor.ReadAllPackets(reader, peer);
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
        Debug.Log($"Player (pid: {(uint)peer.Id}) left the game");
        if (peer.Tag != null)
        {
            ServerPlayer playerLeft;
            if (players.TryGetValue(((uint)peer.Id), out playerLeft))
            {
                foreach (ServerPlayer player in players.Values)
                {
                    if (player.state.pid != playerLeft.state.pid)
                    {
                        SendPacket(new PlayerLeftGamePacket { pid = playerLeft.state.pid }, player.peer, DeliveryMethod.ReliableOrdered);
                    }
                }
                players.Remove((uint)peer.Id);
            }
        }
    }
    #endregion
}
