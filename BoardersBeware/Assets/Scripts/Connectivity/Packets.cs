using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class JoinPacket
{
    public string username { get; set; }
}

public class JoinAcceptPacket
{
    public PlayerState state { get; set; }
}

public struct PlayerState : INetSerializable
{
    public uint pid;
    public Vector3 position;

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(pid);
        writer.Put(position);
    }

    public void Deserialize(NetDataReader reader)
    {
        pid = reader.GetUInt();
        position = reader.GetVector3();
    }
}

public struct ClientPlayer : INetSerializable
{
    public PlayerState state;
    public string username;

    public void Serialize(NetDataWriter writer)
    {
        state.Serialize(writer);
        writer.Put(username);
    }

    public void Deserialize(NetDataReader reader)
    {
        state.Deserialize(reader);
        username = reader.GetString();
    }
}

public class ServerPlayer
{
    public NetPeer peer;
    public PlayerState state;
    public string username;
}

public class PlayerSendUpdatePacket
{
    public Vector3 position { get; set; }
}

public class PlayerReceiveUpdatePacket
{
    public PlayerState[] states { get; set; }
}

public class PlayerJoinedGamePacket
{
    public ClientPlayer player { get; set; }
}

public class PlayerLeftGamePacket
{
    public uint pid { get; set; }
}

public static class SerializingExtensions
{
    public static void Put(this NetDataWriter writer, Vector3 vector)
    {
        writer.Put(vector.x);
        writer.Put(vector.y);
        writer.Put(vector.z);
    }

    public static Vector3 GetVector3(this NetDataReader reader)
    {
        return new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
    }
}
