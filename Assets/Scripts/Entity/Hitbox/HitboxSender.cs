using System;
using System.Linq;
using Helpers.Mask;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Entity.Hitbox
{
    public class HitboxSender : MonoBehaviour
    {
        public string[] Hits;
        public int Damage;
        public bool OneHit;
        public bool Verbose;
        public event EventHandler<SendHitEventArgs> SendHit;

        private Rigidbody2D _rb;

        private MetroidMask _mask;
        
        // Start is called before the first frame update
        protected void Awake()
        {
            _mask = new MetroidMask(Hits, null);
            
            _rb = GetComponent<Rigidbody2D>();
            if (!_rb) Debug.LogError("Hitbox missing RigidBody2D component");
            _rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            if (OneHit) SendHit += (sender, e) => { Destroy(gameObject); };

            _verbose($"[Verbose] {gameObject.name}: Awake()");
        }

        public virtual void OnTriggerStay2D(Collider2D other)
        {
            _verbose($"[Verbose] {gameObject.name}: Trigger stay. Other: {other.gameObject.name}");

            _mask.GO = other.gameObject;
            if (!_mask.HasLayer) 
                return;
            
            _verbose($"[Verbose] {gameObject.name}: Triggered layer in hit list");

            var e = new SendHitEventArgs(Damage);
            _verbose($"[Verbose] {gameObject.name}: Invoking event");
            SendHit?.Invoke(this, e);
            if (!e.Default) return;

            _verbose($"[Verbose] {gameObject.name}: Sending hit to receiver");
            var hit = other.gameObject.GetComponent<HitboxReceiver>()?.OnReceiveHit(e.Damage);
            //TODO: separate hit into two events, one for before sending, one for if successful
        }

        private void _verbose(string msg)
        {
            if (Verbose) print(msg);
        }
    }
}
