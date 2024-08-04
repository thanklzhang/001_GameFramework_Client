﻿using System.Collections;
using System.IO;
using System.Text;
using System;

public class ByteBuffer
{
    MemoryStream stream = null;
    BinaryWriter writer = null;
    BinaryReader reader = null;

    public ByteBuffer()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
    }

    public ByteBuffer(byte[] data)
    {
        if (data != null)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
        }
        else
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }
    }



    public void WriteByte(byte v)
    {
        writer.Write(v);
    }

    public void WriteInt(int v)
    {
        writer.Write((int)v);
    }

    public void WriteUInt(uint v)
    {
        writer.Write(v);
    }
    internal void WriteUShort(ushort v)
    {
        writer.Write(v);
    }

    public void WriteShort(ushort v)
    {
        writer.Write((ushort)v);
    }

    public void WriteLong(long v)
    {
        writer.Write((long)v);
    }

    public void WriteULong(ulong v)
    {
        writer.Write(v);
    }

    //public void WriteFloat(float v)
    //{
    //    byte[] temp = BitConverter.GetBytes(v);
    //    Array.Reverse(temp);
    //    writer.Write(BitConverter.ToSingle(temp, 0));
    //}

    //public void WriteDouble(double v)
    //{
    //    byte[] temp = BitConverter.GetBytes(v);
    //    Array.Reverse(temp);
    //    writer.Write(BitConverter.ToDouble(temp, 0));
    //}

    //public void WriteString(string v)
    //{
    //    byte[] bytes = Encoding.UTF8.GetBytes(v);
    //    writer.Write((ushort)bytes.Length);
    //    writer.Write(bytes);
    //}

    public void WriteBytes(byte[] v)
    {
        //writer.Write((int)v.Length);
        writer.Write(v);
    }


    public void WriteBytesAndLen(byte[] v)
    {
        writer.Write((int)v.Length);
        writer.Write(v);
    }


    public bool HasNext()
    {
        return reader.BaseStream.Position < reader.BaseStream.Length;
    }


    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public uint ReadUInt()
    {
        return reader.ReadUInt32();
    }

    public int ReadInt()
    {
        return (int)reader.ReadInt32();
    }

    internal ulong ReadULong()
    {
        return reader.ReadUInt64();
    }

    public ushort ReadUShort()
    {
        return (ushort)reader.ReadUInt16();
    }

    public short ReadShort()
    {
        return (short)reader.ReadInt16();
    }

    public long ReadLong()
    {
        return (long)reader.ReadInt64();
    }

    //public float ReadFloat()
    //{
    //    byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
    //    Array.Reverse(temp);
    //    return BitConverter.ToSingle(temp, 0);
    //}

    //public double ReadDouble()
    //{
    //    byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
    //    Array.Reverse(temp);
    //    return BitConverter.ToDouble(temp, 0);
    //}

    //public string ReadString()
    //{
    //    ushort len = ReadShort();
    //    byte[] buffer = new byte[len];
    //    buffer = reader.ReadBytes(len);
    //    return Encoding.UTF8.GetString(buffer);
    //}

    public byte[] ReadBytes(int len)
    {
        return reader.ReadBytes(len);
    }

    public byte[] ReadBytesAndLen()
    {
        int len = ReadInt();
        return reader.ReadBytes(len);
    }


    public byte[] ToBytes()
    {
        writer.Flush();
        return stream.ToArray();
    }

    public void Flush()
    {
        writer.Flush();
    }

    public void Close()
    {
        if (writer != null) writer.Close();
        if (reader != null) reader.Close();

        stream.Close();
        writer = null;
        reader = null;
        stream = null;
    }
}
