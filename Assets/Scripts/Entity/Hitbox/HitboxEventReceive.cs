using Helpers;

namespace Entity.Hitbox
{
    public class ReceiveHitEventArgs : MetroidEventArgs
    {
        public int Damage;

        public ReceiveHitEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}
