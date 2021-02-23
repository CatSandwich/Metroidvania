using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallFaster : MonoBehaviour
{
    public Rigidbody2D RB;
    public float UpwardsScale;
    public float DownwardsScale;
    void FixedUpdate()
    {
        RB.gravityScale = (RB.velocity.y < 0 ? DownwardsScale : UpwardsScale);
    }
}
