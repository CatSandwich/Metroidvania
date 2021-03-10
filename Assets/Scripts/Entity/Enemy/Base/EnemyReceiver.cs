using Entity.Enemy.Base;
using Entity.Hitbox;
using UnityEngine;

public class EnemyReceiver : HitboxReceiver
{
    public EnemyController Controller => _controller ??= GetComponentInParent<EnemyController>();
    private EnemyController _controller;
    public override void TakeDamage(int damage) => Controller.Health -= damage;
}
