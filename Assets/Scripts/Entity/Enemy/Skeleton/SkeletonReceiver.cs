using Entity.Hitbox;

namespace Entity.Enemy.Skeleton
{
    public class SkeletonReceiver : HitboxReceiver
    {
        public SkeletonController Controller;
        public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
    }
}
