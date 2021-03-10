using System;
using Helpers.Mask;
using UnityEngine;

namespace Entity.Ambient.Bird
{
    public class BirdController : AmbientController
    {
        protected override bool ReactsToPlayer => true;
        protected override float PlayerReactDistance => 2f;
        private MetroidMask TerrainMask => new MetroidMask("Terrain");

        private bool IsGrounded
        {
            get => _isGrounded;
            set
            { 
                _isGrounded = value;
                _animator.SetBool("IsGrounded", _isGrounded);
                if (_isGrounded) _rb.velocity *= new Vector2(1f, 0f);
            }
        }
        private bool _isGrounded;

        private float WingSpeed
        {
            get => _wingSpeed;
            set
            {
                _wingSpeed = value;
                _animator.SetFloat("WingSpeed", _wingSpeed);
            }
        }
        private float _wingSpeed = 1f;
        
        public void OnCollisionEnter2D(Collision2D other)
        {
            var mask = TerrainMask;
            mask.GO = other.gameObject;
            if (!mask.HasLayer) return;
            if (!CollisionDirection.CheckDirection(gameObject, other.gameObject, Vector2.down)) return;

            IsGrounded = true;
        }

        protected override void _reactToPlayerStart(Transform playerTransform)
        {
            IsGrounded = false;
            WingSpeed = 2f;
            
            var direction = playerTransform.position.x > transform.position.x ? -1f : 1f;
            _rb.velocity = new Vector2(5f * direction, 2f);
        }
    }
}
