using System;
using System.Collections;
using System.Linq;
using Entity.State_Machine;
using Helpers.Mask;
using UnityEngine;

namespace Entity
{
    public abstract class EntityController<TStateEnum> : MonoBehaviour where TStateEnum : Enum 
    {
        private static MetroidMask TerrainMask => new MetroidMask("Terrain");
    
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health <= 0) StartCoroutine(_die());
            }
        }
        private int _health;

        private Animator _animator;
        private Rigidbody2D _rb;
        protected SpriteRenderer SR;
        private BoxCollider2D[] _bcs;

        public TStateEnum DefaultState;
        public Vector2 StartingVelocity;
        public int StartingHealth;

        #region Unity Events

        public void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _bcs = GetComponentsInChildren<BoxCollider2D>();
            SR = GetComponent<SpriteRenderer>();
        }
    
        public void Start()
        {
            _rb.velocity = StartingVelocity;
            _health = StartingHealth;
            _startStateMachine();
            _animationStart();
        }

        public void Update()
        {
            _updateStateMachine();
            _playerReactUpdate();
            _physicsUpdate();
            _animationUpdate();
        }
    
        public void OnCollisionEnter2D(Collision2D other)
        {
            _physicsOnCollisionEnter(other);
        }
    
        #endregion

        #region Physics

        protected bool IsGrounded;

        private void _physicsUpdate()
        {
            // Check if no longer grounded
            if (Mathf.Abs(_rb.velocity.y) > 0.01f) IsGrounded = false;
            foreach (var bc in _bcs) bc.size = SR.sprite.bounds.size;
        }
        
        private void _physicsOnCollisionEnter(Collision2D other)
        {            
            // Only listen for terrain collisions
            var mask = TerrainMask;
            mask.GO = other.gameObject;
            if (!mask.HasLayer) return;
            
            // Only listen for collisions below the bird
            if (!CollisionDirection.CheckDirection(gameObject, other.gameObject, Vector2.down)) return;

            // Remember this property's setter also informs the animator
            IsGrounded = true;
        }

        private void _physicsDie()
        {
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;            
            foreach (var bc in _bcs) bc.enabled = false;
        }
        
        #endregion

        #region Combat
        
        protected virtual IEnumerator _die()
        {
            _physicsDie();

            if (_animatorHasDeath) _animator.SetTrigger("Death");
            yield return new WaitForSeconds(DeathAnimation?.length ?? 0f);
            
            Destroy(gameObject);
        }
        
        #endregion
        
        #region Animation
        
        public AnimationClip DeathAnimation;
        
        private bool _animatorHasSpeedX;
        private bool _animatorHasDeath;
        //private bool _animatorHasAttack;
        private bool _animatorHasIsGrounded;

        private void _animationStart()
        {
            _getParameters();
        }
        
        private void _animationUpdate()
        {
            if (_animatorHasSpeedX) _animator.SetFloat("SpeedX", Mathf.Abs(_rb.velocity.x));
            if (_animatorHasIsGrounded) _animator.SetBool("IsGrounded", IsGrounded);
            
            // Set which direction the sprite is facing based on movement direction
            if (_rb.velocity.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
            if (_rb.velocity.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void _getParameters()
        {
            foreach (var param in _animator.parameters)
            {
                switch (param.name)
                {
                    case "Death":
                        _animatorHasDeath = true;
                        break;
                    case "Attack":
                        //_animatorHasAttack = true;
                        break;
                    case "SpeedX":
                        _animatorHasSpeedX = true;
                        break;
                    case "IsGrounded":
                        _animatorHasIsGrounded = true;
                        break;
                }
            }
        }

        #endregion

        #region Player Reaction

        protected virtual bool ReactsToPlayer => false;
        protected virtual float PlayerReactDistance => 1f;

        private Transform PlayerTransform => _playerTransform ??= GameObject.FindWithTag("Player").transform;
        private Transform _playerTransform;

        private bool _isReactingToPlayer;

        private void _playerReactUpdate()
        {
            // If entity does not listen for this event, ignore the rest of this code
            if (!ReactsToPlayer) return;

            var distanceToPlayer = (transform.position - PlayerTransform.position).magnitude;

            if (_isReactingToPlayer)
            {
                // If player is still within react range return
                if (distanceToPlayer <= PlayerReactDistance) return;

                _isReactingToPlayer = false;
                _reactToPlayerStop(PlayerTransform);
            }
            else
            {
                // If player is still out of react range return
                if (distanceToPlayer > PlayerReactDistance) return;

                _isReactingToPlayer = true;
                _reactToPlayerStart(PlayerTransform);
            }
        }

        // Called when ReactsToPlayer is true and player comes within PlayerReactDistance units from the entity
        protected virtual void _reactToPlayerStart(Transform playerTransform)
        {
        }

        // Called when ReactsToPlayer is true and player stops being within PlayerReactDistance units from the entity
        protected virtual void _reactToPlayerStop(Transform playerTransform)
        {
        }

        #endregion

        #region State Machine

        protected MetroidBehaviour[] Behaviours;
        protected MetroidTransition<TStateEnum>[] Transitions;

        protected TStateEnum CurrentBehaviour
        {
            get;
            private set;
        }
        private StateMachineContext StateContext => _stateContext ??= new StateMachineContext(gameObject, _rb);
        private StateMachineContext _stateContext;
        protected void Transition(TStateEnum behaviour)
        {
            Behaviours[(int)(object)CurrentBehaviour].IsLoaded = false;
            CurrentBehaviour = behaviour;
            Behaviours[(int)(object)CurrentBehaviour].IsLoaded = true;
        }

        private void _startStateMachine()
        {
            foreach (var b in Behaviours)
            {
                b.Init(StateContext);
            }

            Behaviours[(int)(object)DefaultState].IsLoaded = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void _updateStateMachine()
        {
            // Number of transitions the machine will try before assuming an infinite loop
            var maxTransitions = 20;
            var numTransitions = 0;

            // Will keep trying to transition until no more conditions are met
            var didTransition = true;
            while (didTransition)
            {
                didTransition = false;
                foreach (var t in Transitions.Where(t => t.Current.Equals(CurrentBehaviour)))
                {
                    // Check transition condition.
                    if (!t.ShouldTransition.Invoke(StateContext)) continue;

                    Transition(t.Destination);

                    // Loop control.
                    numTransitions++;
                    didTransition = true;
                    break;
                }

                // Stop infinite loops.
                if (numTransitions > maxTransitions)
                {
                    Debug.LogError("State machine reached max transition count. Exiting.");
                    break;
                }
            }

            Behaviours[(int)(object)CurrentBehaviour].Update();
        }

        #endregion
    }
}