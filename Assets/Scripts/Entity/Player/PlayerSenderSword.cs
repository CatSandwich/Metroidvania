using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Entity.Hitbox
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
