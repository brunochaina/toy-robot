using System;
using System.Collections.Generic;
using System.Text;
using ToyRobot.Domain;

namespace ToyRobot.Console
{
    public class RobotStringCommandInterpreter
    {
        private readonly Robot _robot;

        public RobotStringCommandInterpreter(Robot robot)
        {
            _robot = robot ?? throw new ArgumentNullException(nameof(robot));
        }

        public void ExecuteCommand(string command)
        {
            try
            {
                command = command.Trim().ToLower();

                if (command.StartsWith("place"))
                {
                    ExtractCommandValuesAndPlace(command);
                    System.Console.WriteLine("Placed!");
                    _robot.Report();
                }
                else
                {
                    switch (command)
                    {
                        case "move":
                            _robot.Move();
                            System.Console.WriteLine("Moved!");
                            _robot.Report();
                            break;
                        case "left":
                            _robot.TurnLeft();
                            System.Console.WriteLine("Turned left!");
                            _robot.Report();
                            break;
                        case "right":
                            _robot.TurnRight();
                            System.Console.WriteLine("Turned right!");
                            _robot.Report();
                            break;
                        case "report":
                            _robot.Report();
                            break;
                        default:
                            System.Console.WriteLine("Command not recognized, please try again.");
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.Message);
            }
        }

        private void ExtractCommandValuesAndPlace(string command)
        {
            var errorMessage = "Please provide place command like this: place 0,0,NORTH";

            //  TODO: Use regex & cleanup
            var xPositionStr = command.Substring(6, 1);
            if (!int.TryParse(xPositionStr, out var xPosition))
            {
                throw new Exception(errorMessage);
            }

            var yPositionStr = command.Substring(8, 1);
            if (!int.TryParse(yPositionStr, out var yPosition))
            {
                throw new Exception(errorMessage);
            }

            var facingDirectionStr = command.Substring(10);
            FacingDirection facingDirection = facingDirectionStr switch
                                              {
                                                  "north" => new NorthFacingDirection(),
                                                  "east" => new EastFacingDirection(),
                                                  "west" => new WestFacingDirection(),
                                                  "south" => new SouthFacingDirection(),
                                                  _ => throw new Exception(errorMessage)
                                              };

            _robot.Place(xPosition, yPosition, facingDirection);
        }
    }
}
