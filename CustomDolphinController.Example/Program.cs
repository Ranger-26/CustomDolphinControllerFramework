using System;
using System.Runtime.InteropServices;
using System.Text;
using CustomDolphinController.Core;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            new CustomDsuServer(new TestController()).Start(26760);
        }
    }
}