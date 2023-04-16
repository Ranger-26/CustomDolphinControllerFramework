using System;
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
        private const ushort MAX_PROTOCOL_VERSION = 1010;
        private const string SERVER_CODE = "DSUS";
        private uint _serverId;
        public CustomDsuServer()
        {
            _udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var r = new Random();
            _serverId = (uint)r.Next();;
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

        public void HandleMessage(PacketHeader header, byte[] remainingBytes, EndPoint endPoint)
        {
            switch (header.MessageType)
            {
                case MessageType.ProtocolVersionInfo:
                    //never requested
                    byte[] versionBytes = BitConverter.GetBytes(MAX_PROTOCOL_VERSION);
                    _udpServer.SendTo(PacketHelpers.CreatePacket(new PacketHeader(), versionBytes), endPoint);
                    break;
                case MessageType.ConnectedControllersInfo:
                    HandleConnectedControllersInfo(remainingBytes, endPoint);
                    break;
                default:
                    break;
            }
        }

        private void HandleConnectedControllersInfo(byte[] remainingBytes, EndPoint remoteEndPoint)
        {
            int numPorts = BitConverter.ToInt32(remainingBytes, 0);
            byte[] array = new byte[numPorts];
            for (int i = 0; i < numPorts; i++)
            {
                array[i] = (byte) (remainingBytes[i+4] & 0xFF);
            }
            Console.Write($"Num Ports: {numPorts}, ");
            Console.Write("Port array: ");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
            //send data
            PacketHeader header = CreateHeader(MessageType.ConnectedControllersInfo);
            ControllerDataHeader controllerDataHeader = new ControllerDataHeader()
            {
                Slot = 0,
                SlotState = SlotState.Connected,
                DeviceModel = DeviceModel.Gyro,
                ConnectionType = ConnectionType.NotApplicable,
                MacAddress = GetMacAddress(),
                BatteryStatus = BatteryStatus.Charged,
            };

            //needs a zero byte at the end
            byte[] extraBytes = new byte[controllerDataHeader.GetBytes().Length + 1];
            Array.Copy(controllerDataHeader.GetBytes(), 0, extraBytes, 0,  controllerDataHeader.GetBytes().Length);
            char zeroByte = '\0';
            extraBytes[controllerDataHeader.GetBytes().Length] = (byte) zeroByte;
            _udpServer.SendTo(PacketHelpers.CreatePacket(header, extraBytes), remoteEndPoint);
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
        
        public static UInt48 GetMacAddress()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    PhysicalAddress address = ni.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    if (bytes.Length >= 6)
                    {
                        UInt48 value = (UInt48)(
                            ((UInt64)bytes[0] << 40) |
                            ((UInt64)bytes[1] << 32) |
                            ((UInt64)bytes[2] << 24) |
                            ((UInt64)bytes[3] << 16) |
                            ((UInt64)bytes[4] << 8) |
                            (UInt64)bytes[5]
                        );
                        return value;
                    }
                }
            }
            throw new Exception("MAC address not found");
        }

    }
}