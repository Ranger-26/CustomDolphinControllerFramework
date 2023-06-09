using Force.Crc32;

namespace CustomDolphinController.Helpers
{
    public static class OperationsHelper
    {
        //compute checksum
        public static uint ComputeCrc32(this byte[] bytes)
        {
            return Crc32Algorithm.Compute(bytes);
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