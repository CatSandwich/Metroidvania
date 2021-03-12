using System;
using Entity.State_Machine;
using Helpers.Mask;
using UnityEngine;

namespace Entity.Ambient.Bird
{
    public class BirdStateLanding : MetroidBehaviour
    {
        private Vector2 Position => Context.Position;
        private Rigidbody2D RB => Context.Rigidbody;
        private static MetroidMask TerrainMask => new MetroidMask("Terrain");

        private readonly SpriteRenderer _sr;
        private readonly Vector2 _landSpeed;

        private Vector2 _destination;
        
        private int _direction;
        private float _distance;
        private float _timeStart;
        private float _timeDelta;

        public BirdStateLanding(SpriteRenderer sr, Vector2 landSpeed)
        {
            _sr = sr;
            _landSpeed = landSpeed;
        }
        
        protected override void Load()
        {
            var mask = TerrainMask;
            var hit = Physics2D.Raycast(Position, Vector2.down, Single.PositiveInfinity, mask.LayerMask);
            _destination = hit.point;
            
            _direction = RB.velocity.x < 0f ? -1 : 1;
            _distance = Context.GameObject.transform.position.y - _destination.y - _sr.sprite.bounds.size.y / 2;

            _timeStart = Time.time;
            _timeDelta = _distance / _landSpeed.y;
        }

        public override void Update()
        {                
            var timePassed = Time.time - _timeStart;
            var xVel = Mathf.Cos(timePassed / _timeDelta * Mathf.PI) * _direction;
            RB.velocity = new Vector2(xVel * _landSpeed.x, -_landSpeed.y);
        }
    }
}