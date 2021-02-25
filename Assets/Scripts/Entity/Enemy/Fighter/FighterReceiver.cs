using Entity.Hitbox;

namespace Entity.Enemy.Fighter
{
    public class FighterReceiver : HitboxReceiver
    {
        public FighterController Controller;
        // Start is called before the first frame update
        public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
    }
}
