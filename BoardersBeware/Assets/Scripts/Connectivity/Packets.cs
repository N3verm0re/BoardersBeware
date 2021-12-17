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

public class ClientPlayer
{
    public PlayerState state;
    public string username;
}

public class ServerPlayer
{
    public NetPeer peer;
    public PlayerState state;
    public string username;
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
