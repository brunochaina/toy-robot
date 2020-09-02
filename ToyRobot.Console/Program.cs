using ToyRobot.Domain;

namespace ToyRobot.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var consoleMessageWriter = new ConsoleMessageWriter();
            var robot = new Robot(consoleMessageWriter, 5);
            var interpreter = new RobotStringCommandInterpreter(robot);

            ShowMenu();

            while (true)
            {
                System.Console.Write("Enter a command:\t");
                var command = System.Console.ReadLine();
                if (command.ToLower() == "exit")
                {
                    return;
                }
                interpreter.ExecuteCommand(command);
            }
        }

        private static void ShowMenu()
        {
            System.Console.WriteLine("Hi there! Please enter one of the following commands:\n");
            System.Console.WriteLine("----------");
            System.Console.WriteLine("PLACE X,Y,FACING");
            System.Console.WriteLine("Puts the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST.");
            System.Console.WriteLine("----------");
            System.Console.WriteLine("MOVE");
            System.Console.WriteLine("Moves the toy robot one unit forward in the direction it is currently facing.");
            System.Console.WriteLine("----------");
            System.Console.WriteLine("LEFT");
            System.Console.WriteLine("Will rotate the robot 90° anticlockwise without changing the position of the robot.");
            System.Console.WriteLine("----------");
            System.Console.WriteLine("RIGHT");
            System.Console.WriteLine("Rotate the robot 90° clockwise without changing the position of the robot.");
            System.Console.WriteLine("----------");
            System.Console.WriteLine("REPORT");
            System.Console.WriteLine("Outputs the X,Y and F of the robot.");
            System.Console.WriteLine("----------");
            System.Console.WriteLine("EXIT");
            System.Console.WriteLine("To leave");
            System.Console.WriteLine("----------\n");
        }
    }
}