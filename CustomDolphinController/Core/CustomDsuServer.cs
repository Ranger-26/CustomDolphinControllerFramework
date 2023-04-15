using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CustomDolphinController.Enums;
using CustomDolphinController.Helpers;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Core
{
    public class CustomDsuServer
    {
        private Socket _udpServer;
        private const uint MAX_PROTOCOL_VERSION = 1001;
        private const string SERVER_CODE = "DSUS";
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
                try
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
                        //Get packet header
                        PacketHeader header = PacketHelpers.FromByteArray(headerBytes);
                        Console.WriteLine($"Got Header: {header}");
                        //Get remaning data from the packet
                        byte[] remainingBytes = new byte[header.PacketLength - 4];
                        Array.Copy(buffer, 20, remainingBytes, 0, header.PacketLength - 4);
                        HandleMessage(header, remainingBytes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An exception occured, stopping server: {e}");
                    _udpServer.Close();
                }
            }).Start();
        }

        public void HandleMessage(PacketHeader header, byte[] remainingBytes)
        {
            switch (header.MessageType)
            {
                case MessageType.ProtocolVersionInfo:
                    //never requested
                    byte[] versionBytes = BitConverter.GetBytes(MAX_PROTOCOL_VERSION);
                    _udpServer.Send(PacketHelpers.CreatePacket(new PacketHeader(), versionBytes));
                    break;
                case MessageType.ConnectedControllersInfo:
                    HandleConnectedControllersInfo(remainingBytes);
                    break;
                default:
                    break;
            }
        }

        private void HandleConnectedControllersInfo(byte[] remainingBytes)
        {
            int numPorts = BitConverter.ToInt32(remainingBytes, 0);
            byte[] array = new byte[numPorts];
            for (int i = 0; i < numPorts; i++)
            {
                array[i] = (byte) (remainingBytes[i+4] & 0xFF);
            }
            Console.Write($"Num Ports: {numPorts}");
            Console.Write("Port array: ");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
            //send data
        }
    }
}