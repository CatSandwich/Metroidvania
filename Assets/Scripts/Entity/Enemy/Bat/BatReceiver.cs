using System.Collections;
using System.Collections.Generic;
using Entity.Hitbox;
using UnityEngine;

public class BatReceiver : HitboxReceiver
{
    public BatController Controller;
    public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
}
