using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using CustomDolphinController.Core.ControllerFramework;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Example
{
    public class ArduinoController : ControllerBase
    {

        private ConcurrentQueue<InputData> _inputs = new();

        protected override bool RequiresMacAddress { get; set; } = false;
        
        private InputData _lastInputData;

        private volatile bool _recivedRequest = false;

        private Stopwatch _stopwatch;
        public override bool Initialize()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            new Thread(() =>
            {
                SerialPort port = new SerialPort("COM3", 9600); // replace COM3 with the port name of your Arduino and 9600 with the baud rate you've set on the Arduino
                port.Open();

                Console.WriteLine("Arduino Controller started listening for inputs.");
                try
                {
                    while (true)
                    {
                        string data = port.ReadLine(); // read a line of data from the serial port
                        InputData inputData = InputData.ParseInput(data);
                        _lastInputData = inputData;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    port.Close();
                }
            }).Start();
            return true;
        }


        protected override BatteryStatus GetBatteryStatus()
        {
            return BatteryStatus.Charged;
        }

        protected override ConnectionType GetConnectionType()
        {
            return ConnectionType.USB;
        }

        protected override DeviceModel GetDeviceModel()
        {
            return DeviceModel.NotApplicable;
        }

        protected override SlotState GetSlotState()
        {
            return SlotState.Connected;
        }

        protected override bool IsConnected()
        {
            return GetSlotState() == SlotState.Connected;
        }
        
        public override ActualControllerDataInfo GetActualControllerInfo(uint packetNumber)
        {
            /*
            if (_inputs.TryDequeue(out InputData data))
            {
                Console.WriteLine($"Recieving: {data}");
                _lastInputData = data;
                return GetControllerData(packetNumber, data);
            }
            Console.WriteLine("Could not find any new inputs, returning the last input.");
            */
            return GetControllerData(packetNumber, _lastInputData);
        }


        private ActualControllerDataInfo GetControllerData(uint packetNumber, InputData data)
        {
            if (!_recivedRequest)
            {
                _recivedRequest = true;
            }
            return new ActualControllerDataInfo()
            {
                IsConnected = IsConnected(),
                PacketNumber = packetNumber,
                LeftStickX = (byte) ((float)data.x/4),
                LeftStickY = (byte) ((float)data.y/4),
                AnalogA = (byte) (data.buttonState == 0 ? 255 : 0)
            };
        }
    }
    
    public struct InputData
    {
        public int x;
        public int y;
        public int buttonState;
        
        public override string ToString()
        {
            return $"|x = {x}, y = {y}, button_state = {buttonState}|";
        }
        
        public static InputData ParseInput(string inputString)
        {
            try
            {
                int xIndex = inputString.IndexOf("x = ") + 4;
                int yIndex = inputString.IndexOf("y = ") + 4;
                int buttonIndex = inputString.IndexOf("button_state =") + 14;

                string xStr = inputString.Substring(xIndex, inputString.IndexOf(",", xIndex) - xIndex).Trim();
                string yStr = inputString.Substring(yIndex, inputString.IndexOf(",", yIndex) - yIndex).Trim();
                string buttonStr = inputString.Substring(buttonIndex).Trim();

                int x = int.Parse(xStr);
                int y = int.Parse(yStr);
                int buttonState = int.Parse(buttonStr);

                return new InputData { x = x, y = y, buttonState = buttonState };
            }
            catch (Exception e)
            {
                return new InputData();
            }
        }
    }

}