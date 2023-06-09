using System;
using System.Net.NetworkInformation;
using CustomDolphinController.Types;

namespace CustomDolphinController.Helpers
{
    public static class NetworkInfoHelper
    {
        public static UInt48 GetMacAddress()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    PhysicalAddress address = ni.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    if (bytes.Length >= 6)
                    {
                        UInt48 value = (UInt48)(
                            ((UInt64)bytes[0] << 40) |
                            ((UInt64)bytes[1] << 32) |
                            ((UInt64)bytes[2] << 24) |
                            ((UInt64)bytes[3] << 16) |
                            ((UInt64)bytes[4] << 8) |
                            (UInt64)bytes[5]
                        );
                        return value;
                    }
                }
            }
            throw new Exception("MAC address not found");
        }
    }
}