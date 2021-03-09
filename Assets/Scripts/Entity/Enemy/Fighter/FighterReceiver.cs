using Entity.Hitbox;

namespace Entity.Enemy.Fighter
{
    public class FighterReceiver : HitboxReceiver
    {
        public FighterController Controller;
        public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
    }
}
