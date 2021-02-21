namespace Assets.Scripts.Entity.Player.EventArgs
{
    public class RunEventArgs : MetroidEventArgs
    {
        public readonly bool Direction;
        public readonly bool Grounded;
        
        public static bool Left => false;
        public static bool Right => true;

        public RunEventArgs(bool direction, bool grounded)
        {
            Direction = direction;
            Grounded = grounded;
        }
    }
}
