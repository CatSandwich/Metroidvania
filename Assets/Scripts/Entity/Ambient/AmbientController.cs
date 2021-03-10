using UnityEngine;

namespace Entity.Ambient
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class AmbientController : MonoBehaviour
    {
        // Override this to true if entity should listen for nearby player
        protected virtual bool ReactsToPlayer => false;
        // Override this to set a custom radius to react to nearby players
        protected virtual float PlayerReactDistance => 1f;
        
        // These two lines demonstrate a common pattern you'll see me use a lot. 
        // There is a public property (property = custom getters and setters) which returns:
        //     privateBackingField ??= Get the value here
        // This gets the value the first time it's referenced, then stores it for later use.
        private Transform PlayerTransform => _playerTransform ??= GameObject.FindWithTag("Player").transform;
        private Transform _playerTransform;
        
        public Vector2 StartingVelocity;
        
        protected Rigidbody2D RB;
        protected Animator Animator;
        protected bool IsReactingToPlayer;
        
        public void Start()
        {
            // Get needed components
            RB = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            
            // Set starting movement
            RB.velocity = StartingVelocity;
        }
        
        public void Update()
        {
            // Handle player reaction
            _checkPlayerReact();
            
            // Set which direction the sprite is facing based on movement direction
            if (RB.velocity.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
            if (RB.velocity.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        // Called when ReactsToPlayer is true and player comes within PlayerReactDistance units from the entity
        protected virtual void _reactToPlayerStart(Transform playerTransform) { }
        
        // Called when ReactsToPlayer is true and player stops being within PlayerReactDistance units from the entity
        protected virtual void _reactToPlayerStop(Transform playerTransform) { }
        
        // Handle player reaction and call appropriate functions
        private void _checkPlayerReact()
        {
            // If entity does not listen for this event, ignore the rest of this code
            if (!ReactsToPlayer) return;

            var distanceToPlayer = (transform.position - PlayerTransform.position).magnitude;
            
            if (IsReactingToPlayer)
            {
                // If player is still within react range return
                if (distanceToPlayer <= PlayerReactDistance) return;
                
                IsReactingToPlayer = false;
                _reactToPlayerStop(PlayerTransform);
            }
            else
            {
                // If player is still out of react range return
                if (distanceToPlayer > PlayerReactDistance) return;
                
                IsReactingToPlayer = true;
                _reactToPlayerStart(PlayerTransform);
            }
        }
    }
}
