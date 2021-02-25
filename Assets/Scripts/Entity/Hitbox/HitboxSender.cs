using System;
using System.Linq;
using UnityEngine;

namespace Entity.Hitbox
{
    public abstract class HitboxSender : MonoBehaviour
    {
        public string[] Hits;
        public int Damage;
        public bool OneHit;
        public bool Verbose;
        public event EventHandler<SendHitEventArgs> SendHit;

        private Rigidbody2D _rb;
        private int[] _hits;


        // Start is called before the first frame update
        protected void Awake()
        {
            _hits = Hits.Select(LayerMask.NameToLayer).ToArray();
            if (Verbose) foreach (var hit in Hits) print($"[Verbose] {gameObject.name} hits {hit}");
            if (Verbose) foreach(var hit in _hits) print($"[Verbose] {gameObject.name} hits {hit}");

            _rb = GetComponent<Rigidbody2D>();
            _rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            if (!_rb) Debug.LogError("Hitbox missing RigidBody2D component");

            if (OneHit) SendHit += (sender, e) => { Destroy(gameObject); };
            
            if(Verbose) print($"[Verbose] {gameObject.name}: Awake()");
        }
        
        public virtual void OnTriggerStay2D(Collider2D other)
        {
            if (Verbose) print($"[Verbose] {gameObject.name}: Trigger stay. Other: {other.gameObject.name}");
            if (!_hits.Contains(other.gameObject.layer)) return;
            if (Verbose) print($"[Verbose] {gameObject.name}: Triggered layer in hit list");

            var e = new SendHitEventArgs(Damage);
            if (Verbose) print($"[Verbose] {gameObject.name}: Invoking event");
            SendHit?.Invoke(this, e);

            if (Verbose) print($"[Verbose] {gameObject.name}: Sending hit to receiver");
            var hit = other.gameObject.GetComponent<HitboxReceiver>()?.OnReceiveHit(e.Damage);
            //TODO: separate hit into two events, one for before sending, one for if successful
        }
    }
}
