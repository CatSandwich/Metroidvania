namespace Entity.Hitbox
{
    public class SendHitEventArgs
    {
        public int Damage;

        public SendHitEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}
