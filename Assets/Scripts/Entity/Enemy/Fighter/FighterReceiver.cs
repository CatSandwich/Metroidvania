using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Enemy.Fighter;
using Assets.Scripts.Entity.Hitbox;
using UnityEngine;

public class FighterReceiver : HitboxReceiver
{
    public FighterController Controller;
    // Start is called before the first frame update
    public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
}
