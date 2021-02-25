using System;
using Animators;
using UnityEngine;

namespace Entity.Hitbox
{
    public abstract class HitboxReceiver : MonoBehaviour
    {
        public event EventHandler<ReceiveHitEventArgs> ReceiveHit;

        public GameObject DamageIndicator;

        public float HitboxCooldown;
        private float? _lastHit = null;
        
        public virtual bool OnReceiveHit(int damage)
        {
            var e = new ReceiveHitEventArgs(damage);
            ReceiveHit?.Invoke(this, e);
            if (!e.Default) return false;
            
            TakeDamage(e.Damage);
            _damageIndicator(e.Damage);
            _lastHit = Time.time;
            ReceiveHit += _cooldown;
            return true;
        }

        private void _cooldown(object sender, ReceiveHitEventArgs e)
        {
            if (Time.time - _lastHit.Value <= HitboxCooldown)
                e.PreventDefault();
            else
                ReceiveHit -= _cooldown;
        }

        private void _damageIndicator(int damage)
        {
            if (!DamageIndicator) return;
            var indicator = Instantiate(DamageIndicator, transform.position, Quaternion.identity);
            var script = indicator.GetComponent<DamageIndicator>();
            script.Setup(damage);
        }
        
        public abstract void TakeDamage(int damage);
    }
}
