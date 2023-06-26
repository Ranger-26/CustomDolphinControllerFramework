using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using CustomDolphinController.Enums;
using CustomDolphinController.Helpers;
using CustomDolphinController.Structs;
using CustomDolphinController.Types;

namespace CustomDolphinController.Core
{
    public class CustomDsuServer
    {
        private Socket _udpServer;
        private const ushort MAX_PROTOCOL_VERSION = 1001;
        private const string SERVER_CODE = "DSUS";
        private uint _serverId;
        private uint _packetNumber;

        private ControllerBase _controllerBase;
        private EndPoint _controllerDataEndpoint;
        public CustomDsuServer(ControllerBase controllerBase)
        {
            if (!controllerBase.Initialize())
            {
                throw new Exception("Controller failed to initialize!");
            }
            
            _udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _packetNumber = 0;
            var r = new Random();
            _serverId = (uint)r.Next();
            _controllerBase = controllerBase;
        }

        public void Start(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            _udpServer.Bind(localEndPoint);
            Console.WriteLine($"UDP Server started on port {port}");
            StartServerListenThread();
            StartServerSendThread();
        }

        private void SendData(byte[] data, EndPoint endPoint)
        {
            _udpServer.SendTo(data, endPoint);
            _packetNumber += 1;
        }

        private PacketHeader CreateHeader(MessageType type)
        {
            return new PacketHeader()
            {
                MagicString = SERVER_CODE,
                ProtocolVersion = MAX_PROTOCOL_VERSION,
                ClientId = _serverId,
                MessageType = type
            };
        }
        
        public void StartServerListenThread()
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
                       // Console.WriteLine($"Got Header: {header}");
                        //Get remaning data from the packet
                        byte[] remainingBytes = new byte[header.PacketLength - 4];
                        Array.Copy(buffer, 20, remainingBytes, 0, header.PacketLength - 4);
                        HandleMessage(header, remainingBytes, remoteEndPoint);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An exception occured, stopping server: {e}");
                    _udpServer.Close();
                }
            }).Start();
        }

        public void StartServerSendThread()
        {
            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        PacketHeader header = CreateHeader(MessageType.ControllerData);
                        //create header for the controller data
                        ControllerDataHeader controllerDataHeader = _controllerBase.GetControllerDataHeader();
                    
//                    Console.WriteLine($"Slot to report about: {slotToReport}");

                        ActualControllerDataInfo info = _controllerBase.GetActualControllerInfo(_packetNumber);

                        List<byte> otherBytes = new List<byte>();
                        otherBytes.AddRange(controllerDataHeader.GetBytes());
                        otherBytes.AddRange(info.GetBytes());
                        if (_controllerDataEndpoint != null)
                        {
                            //Console.WriteLine($"Sending controller data: {info}");
                            SendData(PacketHelpers.CreatePacket(header, otherBytes.ToArray()), _controllerDataEndpoint);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }).Start();
        }
        
        public void HandleMessage(PacketHeader header, byte[] remainingBytes, EndPoint endPoint)
        {
            Console.WriteLine($"Got message: Type: {header.MessageType}, Endpoint: {endPoint}");
            switch (header.MessageType)
            {
                case MessageType.ProtocolVersionInfo:
                    //never requested
                    byte[] versionBytes = BitConverter.GetBytes(MAX_PROTOCOL_VERSION);
                    SendData(PacketHelpers.CreatePacket(new PacketHeader(), versionBytes), endPoint);
                    break;
                case MessageType.ConnectedControllersInfo:
                    SendConnectedControllersInfo(remainingBytes, endPoint);
                    break;
                case MessageType.ControllerData:
                    if (_controllerDataEndpoint == null)
                        _controllerDataEndpoint = endPoint;
                    SendActualControllerData(remainingBytes, endPoint);
                    break;
                default:
                    break;
            }
        }

        private void SendConnectedControllersInfo(byte[] remainingBytes, EndPoint remoteEndPoint)
        {
            int numPorts = BitConverter.ToInt32(remainingBytes, 0);
            byte[] array = new byte[numPorts];
            for (int i = 0; i < numPorts; i++)
            {
                array[i] = (byte) (remainingBytes[i+4] & 0xFF);
            }
            //Console.Write($"Num Ports: {numPorts}, ");
            //Console.Write("Port array: ");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
            //send data
            PacketHeader header = CreateHeader(MessageType.ConnectedControllersInfo);

            ControllerDataHeader controllerDataHeader = _controllerBase.GetControllerDataHeader();

            //needs a zero byte at the end
            byte[] extraBytes = new byte[controllerDataHeader.GetBytes().Length + 1];
            Array.Copy(controllerDataHeader.GetBytes(), 0, extraBytes, 0,  controllerDataHeader.GetBytes().Length);
            char zeroByte = '\0';
            extraBytes[controllerDataHeader.GetBytes().Length] = (byte) zeroByte;
            
            SendData(PacketHelpers.CreatePacket(header, extraBytes), remoteEndPoint);
        }

        private void SendActualControllerData(byte[] remainingBytes, EndPoint remoteEndPoint)
        {
            //TODO:handle slot ip endpoints
            RegistrationType actions = (RegistrationType)remainingBytes[0];
            Console.WriteLine(actions);
            switch (actions)
            {
                case RegistrationType.AllControllers:
                case RegistrationType.SlotBasedRegistration:
                    byte slotToReport = remainingBytes[1];
                    //create packet header
                    PacketHeader header = CreateHeader(MessageType.ControllerData);
                    //create header for the controller data
                    ControllerDataHeader controllerDataHeader = _controllerBase.GetControllerDataHeader();
                    
//                    Console.WriteLine($"Slot to report about: {slotToReport}");

                    ActualControllerDataInfo info = _controllerBase.GetActualControllerInfo(_packetNumber);

                    List<byte> otherBytes = new List<byte>();
                    otherBytes.AddRange(controllerDataHeader.GetBytes());
                    otherBytes.AddRange(info.GetBytes());

                    SendData(PacketHelpers.CreatePacket(header, otherBytes.ToArray()), remoteEndPoint);
                    
                    break;
                default:
                    break;
            }
        }
        
        
        
        
    }
}