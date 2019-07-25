using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using Assets.Script.Common;

public class TestNetwork : MonoBehaviour
{
    public String host = "127.0.0.1";
    public Int32 port = 8888;
    
    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;

    byte[] bytes;
    byte[] bytes2;
    int len = 14;
    short sidcid = 0x1001;
    int flag1 = 65;
    int flag2 = 66;

    int len2 = 0;
    short sidcid2 = 0;
    int flag11 = 0;
    int flag22 = 0;
    

    MemoryStream memoryStream;
    MemoryStream memoryStream2;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;

    private void Awake()
    {
        bytes2 = new byte[14];
        setupSocket();
        memoryStream = new MemoryStream();
        memoryStream2 = new MemoryStream(bytes2);
        binaryWriter = new BinaryWriter(memoryStream);
        binaryReader = new BinaryReader(memoryStream2);
    }

    // Update is called once per frame
    void Update()
    {
        MsgCSLogin msg = new MsgCSLogin(flag1, flag2);
        bytes = msg.Marshal();
        //Debug.Log(sizeof(short));
        //binaryWriter.Write(len);
        //binaryWriter.Write(sidcid);
        //binaryWriter.Write(flag1);
        //binaryWriter.Write(flag2);

        //Debug.Log("bytes: " + bytes.Length);
        //bytes = memoryStream.ToArray();
        //binaryWriter.Close();
        net_stream.Write(bytes,0,bytes.Length);

        //net_stream.Read(bytes2, 0, 14);

        //len2 = binaryReader.ReadInt32();
        //sidcid2 = binaryReader.ReadInt16();
        //flag11 = binaryReader.ReadInt32();
        //flag22 = binaryReader.ReadInt32();
        //Debug.Log(len2);
        //Debug.Log(sidcid2);
        //Debug.Log(flag11);
        //Debug.Log(flag22);
        //Header header = new Header();
        //header.WriteInt32(9);
        //header.WriteString8("hello");
        //bytes = header.GetBuffer();
        //Debug.Log(bytes.Length);
        //header.Close();
        //writeSocketBytes(bytes);
        //string received_data = readSocket();
        //string key_stroke = Input.inputString;

        // Collects keystrokes into a buffer
        //if (key_stroke != "")
        //{
        //    input_buffer += key_stroke;

        //    //if (key_stroke == "\n")
        //    {
        //        // Send the buffer, clean it
        //        Debug.Log("Sending: " + input_buffer);

        //        writeSocket(input_buffer);
        //        input_buffer = "";
        //    }

        //}


        //if (received_data != "")
        //{
        //    // Do something with the received data,
        //    // print it in the log for now
        //    Debug.Log(received_data);
        //}
    }

    public void setupSocket()
    {
        try
        {
            tcp_socket = new TcpClient(host, port);
            
            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);

            socket_ready = true;
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    public void writeSocket(string line)
    {
        if (!socket_ready)
            return;

        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    public void writeSocketBytes(byte[] bytes)
    {
        if (!socket_ready)
            return;
        socket_writer.Write(bytes);
        socket_writer.Flush();
    }

    public String readSocket()
    {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine();

        return "";
    }

    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }
}
