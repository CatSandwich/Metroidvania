using Helpers;

namespace Entity.Hitbox
{
    public class SendHitEventArgs : MetroidEventArgs
    {
        public int Damage;

        public SendHitEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}
