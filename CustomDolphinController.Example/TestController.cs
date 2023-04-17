using CustomDolphinController.Core.ControllerFramework;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Example
{
    public class TestController : ControllerBase
    {
        protected override bool RequiresMacAddress { get; set; } = false;
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
            return new ActualControllerDataInfo()
            {
                IsConnected = true,
                PacketNumber = packetNumber
            };
        }
    }
}