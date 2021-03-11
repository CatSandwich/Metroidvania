using System;
using System.Collections;
using Helpers.Mask;
using UnityEngine;
using Random = System.Random;

namespace Entity.Ambient.Bird
{
    public class BirdController : AmbientController
    {
        // Must have this line to react to nearby players
        protected override bool ReactsToPlayer => true;
        // Sets a custom reaction radius
        protected override float PlayerReactDistance => 2f;
        
        // Mask (filter) that checks if a given game object is of layer "Terrain"
        private MetroidMask TerrainMask => new MetroidMask("Terrain");

        // Boolean property for whether or not the bird is grounded (sitting on terrain)
        // Get simply returns the value
        // Set sets the value then informs the animator and stops vertical movement if applicable
        private bool IsGrounded
        {
            get => _isGrounded;
            set
            { 
                // Set value into backing field
                _isGrounded = value;
                // If bird lands, it is done landing so it is no longer in the process of landing
                // If it takes off, it's no longer landing
                // Either way it should be set to false
                _isLanding = false;
                // Inform the animator
                Animator.SetBool("IsGrounded", _isGrounded);
                // Set y velocity to 0
                if (_isGrounded) RB.velocity *= new Vector2(1f, 0f);
            }
        }
        private bool _isGrounded;

        // WingSpeed property is used solely to increase the fly animation speed
        // _wingSpeed is the backing field (properties don't store data, needs a backing field to do so)
        // Everything is normal except setting the value also informs the animator
        private float WingSpeed
        {
            set
            {
                Animator.SetFloat("WingSpeed", value);
            }
        }

        public Vector2 LandSpeed;
        public float MinHeightToLand;

        private SpriteRenderer _sr;
        
        private readonly Random _random = new Random();
        private bool _isLanding = false;

        public new void Start()
        {
            base.Start();
            _sr = GetComponent<SpriteRenderer>();
        }
        
        public new void Update()
        {
            base.Update();
            _tryLand();
        }
        
        // Listen for collisions that would ground the bird
        public void OnCollisionEnter2D(Collision2D other)
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

        // This taps into the base class' functionality of running code when the player comes close enough
        protected override void _reactToPlayerStart(Transform playerTransform)
        {
            // Make the bird fly away
            IsGrounded = false;
            WingSpeed = 2f;
            
            var direction = playerTransform.position.x > transform.position.x ? -1f : 1f;
            RB.velocity = new Vector2(5f * direction, 2f);
        }

        // Should the bird land
        private void _tryLand()
        {
            if (IsGrounded) return;
            if (_isLanding) return;
            if (IsReactingToPlayer) return;
            if (_random.Next(0, 250) != 0) return;

            var mask = TerrainMask;
            var hit = Physics2D.Raycast(transform.position, Vector2.down, Single.PositiveInfinity, mask.LayerMask);
            if (!hit.collider) return;
            if (hit.distance < MinHeightToLand) return;

            _isLanding = true;
            StartCoroutine(_land(hit.point));
        }

        // Land
        private IEnumerator _land(Vector2 destination)
        {
            var direction = RB.velocity.x < 0f ? -1 : 1;
            var distance = transform.position.y - destination.y - _sr.sprite.bounds.size.y / 2;

            var timeStart = Time.time;
            var deltaTime = distance / LandSpeed.y;
            
            while (!IsGrounded)
            {
                var timePassed = Time.time - timeStart;
                var xVel = Mathf.Cos(timePassed / deltaTime * Mathf.PI) * direction;
                RB.velocity = new Vector2(xVel * LandSpeed.x, -LandSpeed.y);
                yield return new WaitForEndOfFrame();
            }

            RB.velocity *= new Vector2(0f, 0f);
        }
    }
}
