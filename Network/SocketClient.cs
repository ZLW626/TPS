using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;
using System.IO;

namespace Assets.Script.Network
{
    class SocketClient: MonoBehaviour
    {
        private string host = "127.0.0.1";
        private int port = 8888;
        internal Boolean socketReady = false;
        public static NetworkStream netStream;
        TcpClient tcpSocket;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            SetupSocket();
        }

        public void SetupSocket()
        {
            try
            {
                tcpSocket = new TcpClient(host, port);
                netStream = tcpSocket.GetStream();
                socketReady = true;
            }
            catch(Exception e)
            {
                Debug.Log("Socket error: " + e);
            }
        }

        public void CloseSocket()
        {
            if (!socketReady)
                return;
            tcpSocket.Close();
            socketReady = false;
        }

        public static byte[] RemoveDataHead()
        {
            byte[] dataLenBytes = new byte[4];
            netStream.Read(dataLenBytes, 0, 4);
            int dataLen = ConvertBytesToInt32(dataLenBytes);
            byte[] dataBytesNoHead = new byte[dataLen];
            netStream.Read(dataBytesNoHead, 0, dataLen - 4);
            return dataBytesNoHead;
        }

        private static int ConvertBytesToInt32(byte[] bytes)
        {
            return bytes[0] & 0xff | ((bytes[1] & 0xff) << 8)
                | ((bytes[2] & 0xff) << 16) | (bytes[3] & 0xff) << 24;
        }

        private void OnApplicationQuit()
        {
            CloseSocket();
        }
    }


}
