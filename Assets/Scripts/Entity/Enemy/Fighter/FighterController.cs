using System.Linq;
using Assets.Scripts.Entity.Hitbox;
using UnityEngine;

namespace Assets.Scripts.Entity.Enemy.Fighter
{
    public class FighterController : MonoBehaviour
    {
        public BoxCollider2D TriggerBc;
        private Rigidbody2D _rb;
        private BoxCollider2D _bc;
        private SpriteRenderer _sr;
    
        public float Speed;
        public int MaxHealth;
        public int StartDirection;

        private int _direction;
        private int _health;

        void Start()
        {
            _direction = StartDirection;
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
            _sr = GetComponent<SpriteRenderer>();

            _health = MaxHealth;
        }
    
        // Update is called once per frame
        void Update()
        {
            _rb.velocity = new Vector2(Speed * _direction, _rb.velocity.y);
        
            var sprite = _sr.sprite;
            _bc.size = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
            TriggerBc.size = _bc.size;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            print(_health);
            if(_health <= 0) Destroy(gameObject);
        }
    }
}
