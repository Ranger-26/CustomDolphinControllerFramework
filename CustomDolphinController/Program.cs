using System;
using System.IO.Ports;
using CustomDolphinController.Core;

namespace CustomDolphinController
{
    class Program
    {
        public static InputData ParseInput(string inputString)
        {
            int xIndex = inputString.IndexOf("x = ") + 4;
            int yIndex = inputString.IndexOf("y = ") + 4;
            int buttonIndex = inputString.IndexOf("button_state =") + 14;

            string xStr = inputString.Substring(xIndex, inputString.IndexOf(",", xIndex) - xIndex).Trim();
            string yStr = inputString.Substring(yIndex, inputString.IndexOf(",", yIndex) - yIndex).Trim();
            string buttonStr = inputString.Substring(buttonIndex).Trim();

            int x = int.Parse(xStr);
            int y = int.Parse(yStr);
            int buttonState = int.Parse(buttonStr);

            return new InputData { x = x, y = y, buttonState = buttonState };
        }
        
        
        static void Main(string[] args)
        {
            /*
            SerialPort port = new SerialPort("COM3", 9600); // replace COM3 with the port name of your Arduino and 9600 with the baud rate you've set on the Arduino
            port.Open();

            port.ReadLine();
            port.ReadLine();

            try
            {
                while (true)
                {
                    string data = port.ReadLine(); // read a line of data from the serial port
                    InputData inputData = ParseInput(data);
                    Console.WriteLine(ParseInput(data)); // print the data to the console
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                port.Close();
            }
            */
            CustomDsuServer server = new CustomDsuServer();
            server.Start(26761);
        }
    }
}