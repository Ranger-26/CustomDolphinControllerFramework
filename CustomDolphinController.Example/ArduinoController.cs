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

        private ConcurrentQueue<ArduinoInputData> _inputs = new();

        protected override bool RequiresMacAddress { get; set; } = false;
        
        private ArduinoInputData _lastArduinoInputData;

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
                        Console.WriteLine(data);
                        ArduinoInputData arduinoInputData = ArduinoInputData.ParseInput(data);
                        Console.WriteLine(arduinoInputData);
                        _lastArduinoInputData = arduinoInputData;
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
            return GetControllerData(packetNumber, _lastArduinoInputData);
        }


        private ActualControllerDataInfo GetControllerData(uint packetNumber, ArduinoInputData data)
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
                AnalogA = (byte) (data.buttonAState == 1 ? 255 : 0),
                AnalogL1 = (byte) (data.buttonJState == 0 ? 255 : 0)
            };
        }
    }
    
    public struct ArduinoInputData : IEquatable<ArduinoInputData>
    {
        public int x;
        public int y;
        public int buttonAState;
        public int buttonJState;
        
        public override string ToString()
        {
            return $"|x = {x}, y = {y}, button_a_state = {buttonAState}, button_j_state = {buttonJState}|";
        }
        
        public static ArduinoInputData ParseInput(string input)
        {
            try
            {
                string[] parts = input.Split(',');

                Dictionary<string, int> variables = new Dictionary<string, int>();

                foreach (string part in parts)
                {
                    string[] keyValue = part.Split('=');
                    string variableName = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    if (int.TryParse(value, out int parsedValue))
                    {
                        variables[variableName] = parsedValue;
                    }
                }

                int x = variables["x"];
                int y = variables["y"];
                int buttonAState = variables["button_a_state"];
                int buttonJState = variables["button_j_state"];

                return new ArduinoInputData { x = x, y = y, buttonAState = buttonAState, buttonJState = buttonJState};
            }
            catch (Exception e)
            {
                //in case the serial port doesn't read every part of the string
                return new ArduinoInputData();
            }
        }

        public bool Equals(ArduinoInputData other)
        {
            return x == other.x && y == other.y && buttonAState == other.buttonAState && buttonJState == other.buttonJState;
        }

        public override bool Equals(object obj)
        {
            return obj is ArduinoInputData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, buttonAState, buttonJState);
        }
    }

}