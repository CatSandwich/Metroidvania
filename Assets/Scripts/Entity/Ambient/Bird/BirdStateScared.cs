using Entity.State_Machine;
using UnityEngine;

namespace Entity.Ambient.Bird
{
    public class BirdStateScared : MetroidBehaviour
    {
        private GameObject Player => _player ??= GameObject.FindWithTag("Player");
        private GameObject _player;
        protected override void Load()
        {
            var direction = Player.transform.position.x > Context.Position.x ? -1f : 1f;
            Context.Rigidbody.velocity = new Vector2(5f * direction, 2f);
        }
    }
}