using UnityEngine;

namespace Entity.Player
{
    public class PlayerFallFaster : MonoBehaviour
    {
        public Rigidbody2D RB;
        public float UpwardsScale;
        public float DownwardsScale;

        private void FixedUpdate()
        {
            RB.gravityScale = (RB.velocity.y < 0 ? DownwardsScale : UpwardsScale);
        }
    }
}
