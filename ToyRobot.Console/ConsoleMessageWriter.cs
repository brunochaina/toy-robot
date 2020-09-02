using ToyRobot.Domain;

namespace ToyRobot.Console
{
    public class ConsoleMessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}