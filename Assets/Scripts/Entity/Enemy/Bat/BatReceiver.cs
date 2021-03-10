using Entity.Enemy.Base;
using Entity.Hitbox;

public class BatReceiver : HitboxReceiver
{
    public EnemyController Controller => _controller ??= GetComponentInParent<EnemyController>();
    private EnemyController _controller;
    public override void TakeDamage(int damage) => Controller.Health -= damage;
}
