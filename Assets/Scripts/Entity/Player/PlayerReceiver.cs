using Entity.Hitbox;

namespace Entity.Player
{
    public class PlayerReceiver : HitboxReceiver
    {
        public PlayerController Controller;
        // Start is called before the first frame update
        public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
    }
}
