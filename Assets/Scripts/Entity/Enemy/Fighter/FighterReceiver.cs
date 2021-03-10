using Entity.Enemy.Base;
using Entity.Hitbox;

namespace Entity.Enemy.Fighter
{
    public class FighterReceiver : HitboxReceiver
    {
        public EnemyController Controller => _controller ??= GetComponentInParent<EnemyController>();
        private EnemyController _controller;
        public override void TakeDamage(int damage) => Controller.Health -= damage;
    }
}
