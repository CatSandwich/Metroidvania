using Entity.Player;
using Entity.State_Machine;
using Helpers.Mask;
using UnityEngine;

namespace Entity.Ambient.Coin
{
    public class CoinController : EntityController<CoinStates>
    {
        private static MetroidMask _playerMask => new MetroidMask("Player");

        public int Value;
        
        public new void Awake()
        {
            base.Awake();

            Behaviours = new[]
            {
                new CoinStateIdle()
            };

            Transitions = new MetroidTransition<CoinStates>[]{};
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            var mask = _playerMask;
            mask.GO = other.gameObject;
            if (!mask.HasLayer) return;

            var playerController = other.gameObject.GetComponentInParent<PlayerController>();
            playerController.Coins += Value;
            
            Destroy(gameObject);
        }
    }

    public enum CoinStates
    {
        Idle
    }
}