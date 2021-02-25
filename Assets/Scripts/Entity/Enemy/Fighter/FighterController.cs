using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace Entity.Enemy.Fighter
{
    public class FighterController : MonoBehaviour
    {
        public BoxCollider2D TriggerBc;
        public AnimationClip DeathAnimation;
        public Animator Animator;
        private Rigidbody2D _rb;
        private BoxCollider2D _bc;
        private SpriteRenderer _sr;
    
        public float Speed;
        public int MaxHealth;
        public int StartDirection;

        private int _health;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
            _sr = GetComponent<SpriteRenderer>();

            _rb.velocity = new Vector2(Speed * StartDirection, 0);

            _health = MaxHealth;
        }
    
        // Update is called once per frame
        private void Update()
        {
            var sprite = _sr.sprite;
            _bc.size = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
            TriggerBc.size = _bc.size;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            print(_health);
            if (_health <= 0) StartCoroutine(_die());
        }

        private IEnumerator _die()
        {
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            foreach (var bc in GetComponentsInChildren<BoxCollider2D>()) bc.enabled = false;
            Animator.SetTrigger("Death");
            yield return new WaitForSeconds(DeathAnimation.length);
            Destroy(gameObject);
        }
    }
}
