using Helpers;

namespace Entity.Player.EventArgs
{
    public class JumpEventArgs : MetroidEventArgs
    {
        public int Jumps;

        public JumpEventArgs(int jumps)
        {
            Jumps = jumps;
        }
    }
    
}
