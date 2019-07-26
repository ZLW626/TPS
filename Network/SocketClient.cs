using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;

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

        private void OnApplicationQuit()
        {
            CloseSocket();
        }
    }


}
