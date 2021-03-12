using System;
using Entity.State_Machine;
using Helpers.Mask;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Ambient.Bird
{
    public class BirdControllerStateMachine : EntityController<BirdStates>
    {
        private MetroidMask TerrainMask => new MetroidMask("Terrain");
        protected override bool ReactsToPlayer => true;
        protected override float PlayerReactDistance => 2f;
        public float MinHeightToLand;
        public Vector2 LandingSpeed;
        public int LandRarity;

        public new void Awake()
        {
            base.Awake();
            
            Behaviours = new MetroidBehaviour[]
            {
                new BirdStateIdle(),
                new BirdStateFlying(),
                new BirdStateScared(),
                new BirdStateLanding(SR, LandingSpeed)
            };

            Transitions = new []
            {
                new MetroidTransition<BirdStates>(BirdStates.Idle, BirdStates.Flying,
                    c => !IsGrounded),
                new MetroidTransition<BirdStates>(BirdStates.Flying, BirdStates.Idle,
                    c => IsGrounded),
                new MetroidTransition<BirdStates>(BirdStates.Landing, BirdStates.Idle,
                    c => IsGrounded)
            };
        }

        public new void Update()
        {
            base.Update();
            _tryLand();
        }
        
        private void _tryLand()
        {
            if (CurrentBehaviour != BirdStates.Flying) return;
            if (Random.Range(0, LandRarity) != 0) return;

            var mask = TerrainMask;
            var hit = Physics2D.Raycast(transform.position, Vector2.down, float.PositiveInfinity, mask.LayerMask);
            if (!hit.collider) return;
            if (hit.distance < MinHeightToLand) return;

            Transition(BirdStates.Landing);
        }
        
        protected override void _reactToPlayerStart(Transform playerTransform)
        {
            Transition(BirdStates.Scared);
        }

        protected override void _reactToPlayerStop(Transform playerTransform)
        {
            Transition(BirdStates.Flying);
        }
    }

    public enum BirdStates
    {
        Idle,
        Flying,
        Scared,
        Landing
    }
}