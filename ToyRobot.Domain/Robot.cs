using System;

namespace ToyRobot.Domain
{
    public class Robot
    {
        private readonly IMessageWriter _messageWriter;
        private readonly int _tableSize;
        private Position? _position;

        public Robot(IMessageWriter messageWriter, int tableSize)
        {
            _messageWriter = messageWriter ?? throw new ArgumentNullException(nameof(messageWriter));
            _tableSize = tableSize;
        }

        public Position? Position
        {
            get => _position;
            private set
            {
                ValidatePositionBoundaries(value, $"{value} is outside the boundaries of the {_tableSize}x{_tableSize} table");
                _position = value;
            }
        }

        public FacingDirection FacingDirection { get; set; }

        public void Place(int xPosition, int yPosition, FacingDirection facingDirection)
        {
            Position = new Position(xPosition, yPosition);
            FacingDirection = facingDirection;
        }

        public void TurnRight()
        {
            ValidateRobotIsPlaced();

            FacingDirection.RotateClockwise(this);
        }

        public void TurnLeft()
        {
            ValidateRobotIsPlaced();

            FacingDirection.RotateAnticlockwise(this);
        }

        public void Report()
        {
            ValidateRobotIsPlaced();

            _messageWriter.Write($"Current position: {Position}, Facing: {FacingDirection?.ToString()}");
        }

        public void Move()
        {
            ValidateRobotIsPlaced();

            var newPosition = Position + FacingDirection.GetMovementPositionDelta();
            ValidatePositionBoundaries(newPosition, $"Cannot move {FacingDirection.ToString()} anymore!");

            Position = newPosition;
        }

        private void ValidateRobotIsPlaced()
        {
            if (!Position.HasValue)
            {
                throw new RobotException("Robot must be placed somewhere first.");
            }
        }

        private void ValidatePositionBoundaries(in Position? position, string errorMessage)
        {
            if (!position.HasValue ||
                position.Value.X < 0 ||
                position.Value.Y < 0 ||
                position.Value.X > _tableSize ||
                position.Value.Y > _tableSize)
            {
                throw new RobotException(errorMessage);
            }
        }
    }
}