using System;
using System.Diagnostics;
using CustomDolphinController.Core.ControllerFramework;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Example
{
    public class TestController : ControllerBase
    {
        private Stopwatch _stopwatch;

        public TestController()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
        protected override BatteryStatus GetBatteryStatus()
        {
            return BatteryStatus.Charged;
        }

        protected override ConnectionType GetConnectionType()
        {
            return ConnectionType.NotApplicable;
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
            float x = 255*MathF.Sin((float) _stopwatch.ElapsedMilliseconds / 1000);
            float y = 255*MathF.Cos((float) _stopwatch.ElapsedMilliseconds / 1000);
            return new ActualControllerDataInfo()
            {
                IsConnected = true,
                PacketNumber = packetNumber,
                LeftStickX = (byte)x,
                LeftStickY = (byte)y,
                RightStickX = (byte)x,
                RightStickY = (byte)y,
                AnalogL1 = 255
            };
        }
    }
}