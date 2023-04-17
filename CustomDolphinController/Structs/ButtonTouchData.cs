using System;
using System.Collections.Generic;

namespace CustomDolphinController.Structs
{
    public struct ButtonTouchData
    {
        public bool IsTouchActive;//1 byte
        public byte TouchId;//1 byte
        public ushort TouchX;//2 bytes
        public ushort TouchY;//2 bytes

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            // Serialize IsTouchActive (1 byte)
            bytes.Add(Convert.ToByte(IsTouchActive));

            // Serialize TouchId (1 byte)
            bytes.Add(TouchId);

            // Serialize TouchX (2 bytes, little endian)
            bytes.AddRange(BitConverter.GetBytes(TouchX));

            // Serialize TouchY (2 bytes, little endian)
            bytes.AddRange(BitConverter.GetBytes(TouchY));

            return bytes.ToArray();
        }
    }
}