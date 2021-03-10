using System;
using Helpers.Mask;
using UnityEngine;

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
                _isGrounded = value;
                Animator.SetBool("IsGrounded", _isGrounded);
                if (_isGrounded) RB.velocity *= new Vector2(1f, 0f);
            }
        }
        private bool _isGrounded;

        // WingSpeed property is used solely to increase the fly animation speed
        // _wingSpeed is the backing field (properties don't store data, needs a backing field to do so)
        // Everything is normal except setting the value also informs the animator
        private float WingSpeed
        {
            get => _wingSpeed;
            set
            {
                _wingSpeed = value;
                Animator.SetFloat("WingSpeed", _wingSpeed);
            }
        }
        private float _wingSpeed = 1f;
        
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
    }
}
