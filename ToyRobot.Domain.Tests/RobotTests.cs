using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ToyRobot.Domain.Tests
{
    public class RobotTests
    {
        private const int TableSize = 5;
        private const string RobotMustBePlacedSomewhereFirstMessage = "Robot must be placed somewhere first.";
        private readonly IMessageWriter _messageWriterMock = Substitute.For<IMessageWriter>();
        private readonly Robot _robot;

        public RobotTests() => _robot = new Robot(_messageWriterMock, TableSize);

        #region Report

        [Theory]
        [AutoData]
        public void ShouldReportPositionAndFacingDirection([Range(0, TableSize)] int xPosition,
                                                           [Range(0, TableSize)] int yPosition,
                                                           SouthFacingDirection facingDirection)
        {
            _robot.Place(xPosition, yPosition, facingDirection);

            _robot.Report();

            _messageWriterMock.Received(1).Write($"Current position: ({xPosition}, {yPosition}), Facing: {facingDirection.ToString()}");
        }

        [Fact]
        public void ShouldFailReportingIfItIsNotPlaced()
        {
            Action action = () => _robot.Report();

            action.Should().Throw<RobotException>().WithMessage(RobotMustBePlacedSomewhereFirstMessage);
        }

        #endregion

        #region Movement

        public static IEnumerable<object[]> MovementData =>
            new[]
            {
                new object[] {0, 0, new NorthFacingDirection(), 0, 1},
                new object[] {0, 0, new EastFacingDirection(), 1, 0},
                new object[] {1, 1, new SouthFacingDirection(), 1, 0},
                new object[] {1, 1, new WestFacingDirection(), 0, 1}
            };

        [Theory]
        [MemberData(nameof(MovementData))]
        public void ShouldMoveInTheRightDirection(int initialXPosition,
                                                  int initialYPosition,
                                                  FacingDirection initialFacingDirection,
                                                  int expectedXPosition,
                                                  int expectedYPosition)
        {
            _robot.Place(initialXPosition, initialYPosition, initialFacingDirection);

            _robot.Move();

            _robot.Position.Value.X.Should().Be(expectedXPosition);
            _robot.Position.Value.Y.Should().Be(expectedYPosition);
            _robot.FacingDirection.Should().Be(initialFacingDirection);
        }

        public static IEnumerable<object[]> PreventMovementData =>
            new[]
            {
                new object[] {0, 0, new SouthFacingDirection()},
                new object[] {0, 0, new WestFacingDirection()},
                new object[] {TableSize, 1, new EastFacingDirection()},
                new object[] {1, TableSize, new NorthFacingDirection()}
            };

        [Theory]
        [MemberData(nameof(PreventMovementData))]
        public void ShouldPreventMovingOutsideTableBoundaries(int initialXPosition,
                                                              int initialYPosition,
                                                              FacingDirection initialFacingDirection)
        {
            _robot.Place(initialXPosition, initialYPosition, initialFacingDirection);

            Action moveAction = () => _robot.Move();

            moveAction.Should().Throw<RobotException>().WithMessage($"Cannot move {initialFacingDirection.ToString()} anymore!");
            _robot.Position.Value.X.Should().Be(initialXPosition);
            _robot.Position.Value.Y.Should().Be(initialYPosition);
            _robot.FacingDirection.Should().Be(initialFacingDirection);
        }

        [Fact]
        public void ShouldFailMovingIfItIsNotPlaced()
        {
            Action moveAction = () => _robot.Move();

            moveAction.Should().Throw<RobotException>().WithMessage(RobotMustBePlacedSomewhereFirstMessage);
        }

        #endregion

        #region Placement

        [Theory]
        [AutoData]
        public void ShouldBePlacedInTheRightPosition([Range(0, TableSize)] int xPosition,
                                                     [Range(0, TableSize)] int yPosition,
                                                     SouthFacingDirection facingDirection)
        {
            _robot.Place(xPosition, yPosition, facingDirection);

            _robot.Position.Value.X.Should().Be(xPosition);
            _robot.Position.Value.Y.Should().Be(yPosition);
            _robot.FacingDirection.Should().Be(facingDirection);
        }

        [Theory]
        [InlineAutoData(-1, -4)]
        [InlineAutoData(3, 7)]
        [InlineAutoData(8, 1)]
        public void ShouldFailWhenItIsPlacedOutsideTableBoundaries(int xPosition,
                                                                   int yPosition,
                                                                   EastFacingDirection facingDirection)
        {
            Action placeAction = () => _robot.Place(xPosition, yPosition, facingDirection);

            placeAction.Should()
                       .Throw<RobotException>($"({xPosition}, {yPosition}) is outside the boundaries of the {TableSize}x{TableSize} table");
        }

        [Theory]
        [AutoData]
        public void ShouldBeAbleToBePlacedMultipleTimes([Range(0, TableSize)] int xPosition,
                                                        [Range(0, TableSize)] int yPosition,
                                                        SouthFacingDirection facingDirection)
        {
            _robot.Place(1, 4, new NorthFacingDirection());
            _robot.Place(0, 1, new WestFacingDirection());
            _robot.Place(xPosition, yPosition, facingDirection);

            _robot.Position.Value.X.Should().Be(xPosition);
            _robot.Position.Value.Y.Should().Be(yPosition);
            _robot.FacingDirection.Should().Be(facingDirection);
        }

        #endregion

        #region Rotation

        [Theory]
        [MemberData(nameof(TurnLeftData))]
        public void ShouldTurnLeft(FacingDirection initialFacingDirection,
                                              FacingDirection newFacingDirection)
        {
            _robot.Place(1, 3, initialFacingDirection);

            _robot.TurnLeft();

            _robot.FacingDirection.Should().BeOfType(newFacingDirection.GetType());
        }

        public static IEnumerable<object[]> TurnLeftData =>
            new[]
            {
                new object[] {new NorthFacingDirection(), new WestFacingDirection()},
                new object[] {new WestFacingDirection(), new SouthFacingDirection()},
                new object[] {new SouthFacingDirection(), new EastFacingDirection()},
                new object[] {new EastFacingDirection(), new NorthFacingDirection()}
            };

        [Theory]
        [MemberData(nameof(TurnRightData))]
        public void ShouldTurnRight(FacingDirection initialFacingDirection,
                                          FacingDirection newFacingDirection)
        {
            _robot.Place(0, 0, initialFacingDirection);

            _robot.TurnRight();

            _robot.FacingDirection.Should().BeOfType(newFacingDirection.GetType());
        }

        public static IEnumerable<object[]> TurnRightData =>
            new[]
            {
                new object[] {new NorthFacingDirection(), new EastFacingDirection()},
                new object[] {new EastFacingDirection(), new SouthFacingDirection()},
                new object[] {new SouthFacingDirection(), new WestFacingDirection()},
                new object[] {new WestFacingDirection(), new NorthFacingDirection()}
            };

        [Fact]
        public void ShouldBeAbleToRotateMultipleTimes()
        {
            _robot.Place(2, 4, new SouthFacingDirection());

            _robot.TurnRight(); //  Facing west
            _robot.TurnRight(); //  Facing north
            _robot.TurnLeft(); //  Facing west

            _robot.FacingDirection.Should().BeOfType<WestFacingDirection>();
        }

        [Fact]
        public void ShouldFailTurningRightIfItIsNotPlaced()
        {
            Action action = () => _robot.TurnRight();

            action.Should().Throw<RobotException>().WithMessage(RobotMustBePlacedSomewhereFirstMessage);
        }

        [Fact]
        public void ShouldFailTurningLeftIfItIsNotPlaced()
        {
            Action action = () => _robot.TurnRight();

            action.Should().Throw<RobotException>().WithMessage(RobotMustBePlacedSomewhereFirstMessage);
        }

        #endregion
    }
}