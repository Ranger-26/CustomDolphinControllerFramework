using System;
using System.IO;
using System.Text;
using CustomDolphinController.Enums;
using CustomDolphinController.Types;

namespace CustomDolphinController.Structs
{
    public struct ControllerDataHeader : IEquatable<ControllerDataHeader>
    {
        public byte Slot;
        public SlotState SlotState;
        public DeviceModel DeviceModel;
        public ConnectionType ConnectionType;
        public UInt48 MacAddress;
        public BatteryStatus BatteryStatus;

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[11];
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Slot);
                    writer.Write((byte)SlotState);
                    writer.Write((byte)DeviceModel);
                    writer.Write((byte)ConnectionType);
                    writer.Write(UInt48.GetBytes(MacAddress));
                    writer.Write((byte)BatteryStatus);
                }
            }

            return bytes;
        }

        public bool Equals(ControllerDataHeader other)
        {
            return Slot == other.Slot && SlotState == other.SlotState && DeviceModel == other.DeviceModel && ConnectionType == other.ConnectionType && MacAddress.Equals(other.MacAddress) && BatteryStatus == other.BatteryStatus;
        }

        public override bool Equals(object obj)
        {
            return obj is ControllerDataHeader other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Slot, (int) SlotState, (int) DeviceModel, (int) ConnectionType, MacAddress, (int) BatteryStatus);
        }
    }
}