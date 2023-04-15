using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControllerServer
{
    public class CustomDsuServer
    {
        private Socket _udpServer;
        private const uint MAX_PROTOCOL_VERSION = 1001;
        public CustomDsuServer()
        {
            _udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Start(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            _udpServer.Bind(localEndPoint);
            Console.WriteLine($"UDP Server started on port {port}");
            StartServerThread();
        }

        public void StartServerThread()
        {
            new Thread(() =>
            {
                while (true)
                {
                    // Receive data from a client
                    byte[] buffer = new byte[1024];
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    int numBytesReceived = _udpServer.ReceiveFrom(buffer, ref remoteEndPoint);
                
                    if (numBytesReceived == 0) continue;

                    byte[] headerBytes = new byte[20];
                    Array.Copy(buffer, 0, headerBytes, 0, 20);
                    //handle message
                    PacketHeader header = new PacketHeader(headerBytes);

                    switch (header.MessageType)
                    {
                        //version
                        
                    }
                }
            }).Start();
        }
    }
}