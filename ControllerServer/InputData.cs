namespace ControllerServer
{
    public struct InputData
    {
        public int x;
        public int y;
        public int buttonState;
        
        public override string ToString()
        {
            return $"|x = {x}, y = {y}, button_state = {buttonState}|";
        }
    }
}