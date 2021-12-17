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
    [SerializeField] private ControllerTest Player;
    private Dictionary<uint, RemotePlayer> players = new Dictionary<uint, RemotePlayer>();
    [SerializeField] private GameObject remotePlayerPrefab;

    //networkTimer =
    
    public void Connect(string username)
    {
        player.username = username;
        writer = new NetDataWriter();
        packetProcessor = new NetPacketProcessor();
        packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector3());
        packetProcessor.RegisterNestedType<PlayerState>();
        packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAccept);
        packetProcessor.RegisterNestedType<ClientPlayer>();
        packetProcessor.SubscribeReusable<PlayerReceiveUpdatePacket>(OnReceiveUpdate);
        packetProcessor.SubscribeReusable<PlayerJoinedGamePacket>(OnPlayerJoin);
        packetProcessor.SubscribeReusable<PlayerLeftGamePacket>(OnPlayerLeave);

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
        Player.transform.position = player.state.position;
    }

    public void OnReceiveUpdate(PlayerReceiveUpdatePacket packet)
    {
        foreach (PlayerState state in packet.states)
        {
            if (state.pid == player.state.pid)
            {
                continue;
            }

            players[state.pid].receivedPosition = state.position;
        }
    }

    public void OnPlayerJoin(PlayerJoinedGamePacket packet)
    {
        Debug.Log($"Player '{packet.player.username}' (pid: {packet.player.state.pid}) joined the game");
        players.Add(packet.player.state.pid, this.gameObject.AddComponent<RemotePlayer>());
        players[packet.player.state.pid].name = packet.player.state.pid.ToString();
        players[packet.player.state.pid].receivedPosition = packet.player.state.position;
        players[packet.player.state.pid].playerObject = GameObject.Instantiate(remotePlayerPrefab, packet.player.state.position, remotePlayerPrefab.transform.rotation);
    }

    public void OnPlayerLeave(PlayerLeftGamePacket packet)
    {
        Debug.Log($"Player (pid: {packet.pid}) left the game");
        GameObject.Destroy(players[packet.pid].playerObject);
        RemotePlayer.Destroy(players[packet.pid]);
        players.Remove(packet.pid);
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

    private void Start()
    {
        Connect("TestPC1");
    }

    private void Update()
    {
        if (client != null)
        {
            client.PollEvents();
            if (Player.gameObject != null)
            {
                SendPacket(new PlayerSendUpdatePacket { position = Player.gameObject.transform.position }, DeliveryMethod.Unreliable);
            }
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
