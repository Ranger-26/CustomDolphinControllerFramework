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
        
        public static byte[] GetBytes(UInt48 value)
        {
            byte[] bytes = new byte[6];
            bytes[0] = (byte)value;
            bytes[1] = (byte)(value >> 8);
            bytes[2] = (byte)(value >> 16);
            bytes[3] = (byte)(value >> 24);
            bytes[4] = (byte)(value >> 32);
            bytes[5] = (byte)(value >> 40);
            return bytes;
        }
    }
}