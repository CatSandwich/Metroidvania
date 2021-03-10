using UnityEngine;

namespace Entity.Ambient
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class AmbientController : MonoBehaviour
    {
        //override this to true if entity should listen for nearby player
        protected virtual bool ReactsToPlayer => false;
        protected virtual float PlayerReactDistance => 1f;
        
        private Transform PlayerTransform => _playerTransform ??= GameObject.FindWithTag("Player").transform;
        private Transform _playerTransform;
        
        public Vector2 StartingVelocity;
        
        protected Rigidbody2D _rb;
        protected Animator _animator;
        protected bool _isReactingToPlayer;

        

        public void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _rb.velocity = StartingVelocity;
        }
        
        public void Update()
        {
            _checkPlayerReact();
            if (_rb.velocity.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
            if (_rb.velocity.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        // Called when ReactsToPlayer is true and player comes within PlayerReactDistance units from the entity
        protected virtual void _reactToPlayerStart(Transform playerTransform) { }
        
        // Called when ReactsToPlayer is true and player stops being within PlayerReactDistance units from the entity
        protected virtual void _reactToPlayerStop(Transform playerTransform) { }
        
        private void _checkPlayerReact()
        {
            if (!ReactsToPlayer) return;

            var distanceToPlayer = (transform.position - PlayerTransform.position).magnitude;
            
            if (_isReactingToPlayer)
            {
                if (distanceToPlayer <= PlayerReactDistance) return;
                _isReactingToPlayer = false;
                _reactToPlayerStop(PlayerTransform);
            }
            else
            {
                if (distanceToPlayer > PlayerReactDistance) return;
                _isReactingToPlayer = true;
                _reactToPlayerStart(PlayerTransform);
            }
        }
    }
}
