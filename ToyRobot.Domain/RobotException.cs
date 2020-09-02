using System;

namespace ToyRobot.Domain
{
    public class RobotException : Exception
    {
        public RobotException(string message) : base(message) { }
    }
}