namespace ToyRobot.Domain
{
    public abstract class FacingDirection
    {
        public abstract void RotateClockwise(Robot robot);
        public abstract void RotateAnticlockwise(Robot robot);
        public abstract Position GetMovementPositionDelta();
        public new abstract string ToString();
    }

    public class NorthFacingDirection : FacingDirection
    {
        public override void RotateClockwise(Robot robot) => robot.FacingDirection = new EastFacingDirection();

        public override void RotateAnticlockwise(Robot robot) => robot.FacingDirection = new WestFacingDirection();

        public override Position GetMovementPositionDelta() => new Position(0, 1);

        public override string ToString() => "North";
    }

    public class EastFacingDirection : FacingDirection
    {
        public override void RotateClockwise(Robot robot) => robot.FacingDirection = new SouthFacingDirection();

        public override void RotateAnticlockwise(Robot robot) => robot.FacingDirection = new NorthFacingDirection();

        public override Position GetMovementPositionDelta() => new Position(1, 0);

        public override string ToString() => "East";
    }

    public class SouthFacingDirection : FacingDirection
    {
        public override void RotateClockwise(Robot robot) => robot.FacingDirection = new WestFacingDirection();

        public override void RotateAnticlockwise(Robot robot) => robot.FacingDirection = new EastFacingDirection();

        public override Position GetMovementPositionDelta() => new Position(0, -1);

        public override string ToString() => "South";
    }

    public class WestFacingDirection : FacingDirection
    {
        public override void RotateClockwise(Robot robot) => robot.FacingDirection = new NorthFacingDirection();

        public override void RotateAnticlockwise(Robot robot) => robot.FacingDirection = new SouthFacingDirection();

        public override Position GetMovementPositionDelta() => new Position(-1, 0);

        public override string ToString() => "West";
    }
}