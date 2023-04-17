using System.Threading;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;
using CustomDolphinController.Types;

namespace CustomDolphinController.Core.ControllerFramework
{
    public abstract class ControllerBase
    {
        public Thread UpdateThread;
        
        public ControllerBase()
        {
            UpdateThread = new Thread(Update);
            UpdateThread.Start();
        }

        protected abstract bool RequiresMacAddress { get; set; }

        protected abstract BatteryStatus GetBatteryStatus();

        protected abstract ConnectionType GetConnectionType();

        protected abstract DeviceModel GetDeviceModel();

        protected abstract SlotState GetSlotState();

        protected abstract bool IsConnected();
        
        public abstract ActualControllerDataInfo GetActualControllerInfo(uint packetNumber);

        public ControllerDataHeader GetControllerDataHeader()
        {
            return new ControllerDataHeader()
            {
                Slot = 0,
                SlotState = GetSlotState(),
                DeviceModel = GetDeviceModel(),
                ConnectionType = GetConnectionType(),
                MacAddress = GetMacAddress(),
                BatteryStatus = GetBatteryStatus()
            };
        }

        public virtual void Update()
        {
            
        }
        
        protected virtual UInt48 GetMacAddress()
        {
            return (UInt48) 0;
        }
    }
}