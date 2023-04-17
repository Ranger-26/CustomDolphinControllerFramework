using System;
using System.Collections.Generic;

namespace CustomDolphinController.Structs
{
    public struct ActualControllerDataInfo
    {
        public bool IsConnected;//1 byte
        public uint PacketNumber;//4 bytes
        public byte PadBitMask;//1 byte
        public byte ButtonBitMask;//1 byte
        public bool HomeButton;//1 byte
        public bool TouchButton;//1 byte
        //left and right sticks
        public byte LeftStickX;//1 byte//plus rightward
        public byte LeftStickY;//1 byte//plus upward
        public byte RightStickX;//1 byte//plus rightward
        public byte RightStickY;//1 byte//plus upward
        //dpad
        public byte AnalogDLeft;//1 byte
        public byte AnalogDDown;//1 byte
        public byte AnalogDRight;//1 byte
        public byte AnalogDup;//1 byte
        //analog buttons
        public byte AnalogY;//1 byte
        public byte AnalogB;//1 byte
        public byte AnalogA;//1 byte
        public byte AnalogX;//1 byte
        public byte AnalogR1;//1 byte
        public byte AnalogL1;//1 byte
        public byte AnalogR2;//1 byte
        public byte AnalogL2;//1 byte
        //Touches
        public ButtonTouchData FirstTouch;//6 bytes
        public ButtonTouchData SecondTouch;//6 bytes
        //other motion data
        public ulong MotionDataTimeStep;//8 bytes
        //accelerometer data
        public float AccelerometerX;//4 bytes
        public float AccelerometerY;//4 bytes
        public float AccelerometerZ;//4 bytes
        //gyroscope data
        public float GyroscopePitch;//4 bytes
        public float GyroscopeYaw;//4 bytes
        public float GyroscopeRoll;//4 bytes
        
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            // Serialize IsConnected (1 byte)
            bytes.Add(Convert.ToByte(IsConnected));

            // Serialize PacketNumber (4 bytes, little endian)
            bytes.AddRange(BitConverter.GetBytes(PacketNumber));

            // Serialize PadBitMask (1 byte)
            bytes.Add(PadBitMask);

            // Serialize ButtonBitMask (1 byte)
            bytes.Add(ButtonBitMask);

            // Serialize HomeButton (1 byte)
            bytes.Add(Convert.ToByte(HomeButton));

            // Serialize TouchButton (1 byte)
            bytes.Add(Convert.ToByte(TouchButton));

            // Serialize LeftStickX (1 byte)
            bytes.Add(LeftStickX);

            // Serialize LeftStickY (1 byte)
            bytes.Add(LeftStickY);

            // Serialize RightStickX (1 byte)
            bytes.Add(RightStickX);

            // Serialize RightStickY (1 byte)
            bytes.Add(RightStickY);

            // Serialize AnalogDLeft (1 byte)
            bytes.Add(AnalogDLeft);

            // Serialize AnalogDDown (1 byte)
            bytes.Add(AnalogDDown);

            // Serialize AnalogDRight (1 byte)
            bytes.Add(AnalogDRight);

            // Serialize AnalogDup (1 byte)
            bytes.Add(AnalogDup);

            // Serialize AnalogY (1 byte)
            bytes.Add(AnalogY);

            // Serialize AnalogB (1 byte)
            bytes.Add(AnalogB);

            // Serialize AnalogA (1 byte)
            bytes.Add(AnalogA);

            // Serialize AnalogX (1 byte)
            bytes.Add(AnalogX);

            // Serialize AnalogR1 (1 byte)
            bytes.Add(AnalogR1);

            // Serialize AnalogL1 (1 byte)
            bytes.Add(AnalogL1);

            // Serialize AnalogR2 (1 byte)
            bytes.Add(AnalogR2);

            // Serialize AnalogL2 (1 byte)
            bytes.Add(AnalogL2);

            // Serialize FirstTouch (6 bytes)
            bytes.AddRange(FirstTouch.GetBytes());

            // Serialize SecondTouch (6 bytes)
            bytes.AddRange(SecondTouch.GetBytes());

            // Serialize MotionDataTimeStep (8 bytes, little endian)
            bytes.AddRange(BitConverter.GetBytes(MotionDataTimeStep));

            // Serialize AccelerometerX (4 bytes)
            bytes.AddRange(BitConverter.GetBytes(AccelerometerX));

            // Serialize AccelerometerY (4 bytes)
            bytes.AddRange(BitConverter.GetBytes(AccelerometerY));

            // Serialize AccelerometerZ (4 bytes)
            bytes.AddRange(BitConverter.GetBytes(AccelerometerZ));

            // Serialize GyroscopePitch (4 bytes)
            bytes.AddRange(BitConverter.GetBytes(GyroscopePitch));

            // Serialize GyroscopeYaw (4 bytes)
            bytes.AddRange(BitConverter.GetBytes(GyroscopeYaw));

            // Serialize GyroscopeRoll (4 bytes)
            bytes.AddRange(BitConverter.GetBytes(GyroscopeRoll));

            return bytes.ToArray();
        }
    }
}