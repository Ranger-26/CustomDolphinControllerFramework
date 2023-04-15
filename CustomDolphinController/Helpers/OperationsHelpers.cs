namespace CustomDolphinController.Helpers
{
    public static class OperationsHelper
    {
        public static uint ComputeCrc32(this byte[] bytes)
        {
            uint crc = uint.MaxValue;
            foreach (byte b in bytes)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    uint mask = (uint)-(crc & 1);
                    crc = (crc >> 1) ^ (0xEDB88320 & mask);
                }
            }
            return ~crc;
        }
        
        public static uint SwapEndian(uint value)
        {
            return (value << 24) |
                   ((value << 8) & 0x00FF0000) |
                   ((value >> 8) & 0x0000FF00) |
                   (value >> 24);
        }
    }
}