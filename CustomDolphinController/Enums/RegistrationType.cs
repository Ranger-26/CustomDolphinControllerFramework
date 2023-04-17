using System;

namespace CustomDolphinController.Enums
{
    [Flags]
    public enum RegistrationType : byte
    {
        AllControllers = 0,
        SlotBasedRegistration = 1,
        MacBasedRegistration = 2
    }
}