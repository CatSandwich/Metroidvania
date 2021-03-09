using System.Collections;
using Entity.Hitbox;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerSenderSword : HitboxSender
    {
        public Animator Animator;
        public AnimationClip AttackAnimation;
        
        public void Start()
        {
            StartCoroutine(_attack());
        }

        private IEnumerator _attack()
        {
            var r = new System.Random();
            Animator.SetTrigger(r.Next(0, 2) == 0 ? "1" : "2");
            yield return new WaitForSeconds(AttackAnimation.length);
            Destroy(gameObject);
        }
    }
}
