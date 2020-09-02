namespace ToyRobot.Domain
{
    public readonly struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y);

        public override string ToString() => $"({X}, {Y})";
    }
}