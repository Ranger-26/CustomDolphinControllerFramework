namespace CustomDolphinController.Enums
{
    public enum MessageType
    {
        ProtocolVersionInfo = 0x100000,
        ConnectedControllersInfo = 0x100001,
        ControllerData = 0x100002,
        ControllerMotorInfo = 0x110001,
        RumbleControllerMotor = 0x110002
    }
}