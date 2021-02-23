using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Hitbox;
using Assets.Scripts.Entity.Player;
using UnityEngine;

public class PlayerReceiver : HitboxReceiver
{
    public PlayerController Controller;
    // Start is called before the first frame update
    public override void TakeDamage(int damage) => Controller.TakeDamage(damage);
}
