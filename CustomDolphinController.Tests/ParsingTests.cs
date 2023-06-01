﻿using CustomDolphinController.Enums;
using CustomDolphinController.Example;
using CustomDolphinController.Helpers;
using CustomDolphinController.Structs;
using NUnit.Framework;

namespace CustomDolphinController.Tests
{
    public class Parsingtests
    {
        
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void ArduinoParsingTest()
        {
            InputData expected = new InputData()
            {
                x = 504,
                y = 506,
                buttonAState = 0,
                buttonJState = 1
            };
            Assert.AreEqual(expected, InputData.ParseInput("x = 504, y = 506, button_a_state = 0, button_j_state = 1"));
        }

        
        [Test]
        public void ArduinoParsingTest2()
        {
            InputData expected = new InputData()
            {
                x = 1004,
                y = 806,
                buttonAState = 0,
                buttonJState = 1
            };
            Assert.AreEqual(expected, InputData.ParseInput("x = 1004, y = 806, button_a_state = 0, button_j_state = 1"));
        }
    }
}