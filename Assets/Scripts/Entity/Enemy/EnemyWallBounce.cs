using UnityEngine;

namespace Entity.Enemy
{
    public class EnemyWallBounce : MonoBehaviour
    {
        public Rigidbody2D RB;

        private float _velocity;

        public void FixedUpdate()
        {
            //if things get stuck on walls, its because of this float equality comparison
            if (RB.velocity.x != 0f) _velocity = RB.velocity.x;
        }
    
        // Start is called before the first frame update
        public void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Terrain")) return;

            var isLeft = CollisionDirection.CheckDirection(gameObject, other.gameObject, Vector2.left);
            var isRight = CollisionDirection.CheckDirection(gameObject, other.gameObject, Vector2.right);
            if (RB.velocity.x < 0f && !isLeft) return;
            if (RB.velocity.x > 0f && !isRight) return;

            RB.velocity = new Vector2(-_velocity, RB.velocity.y);
        }
    }
}
