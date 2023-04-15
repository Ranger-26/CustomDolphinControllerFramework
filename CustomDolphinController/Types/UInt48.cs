using System;

namespace CustomDolphinController.Types
{
    public struct UInt48
    {
        private UInt64 value;
    
        public UInt48(UInt64 value)
        {
            this.value = value & 0x0000FFFFFFFFFFFF;
        }
    
        public static explicit operator UInt48(UInt64 value)
        {
            return new UInt48(value);
        }
    
        public static implicit operator UInt64(UInt48 value)
        {
            return value.value;
        }
    }
}