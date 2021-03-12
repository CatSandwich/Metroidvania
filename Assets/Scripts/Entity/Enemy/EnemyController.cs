using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Entity.Enemy.Base
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyController : MonoBehaviour
    {
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health <= 0) StartCoroutine(_die());
            }
        }
        private int _health;

        public AnimationClip DeathAnimation;

        public int StartingHealth;
        public int StartingDirection;
        public float Speed;

        protected Animator _animator;
        protected Rigidbody2D _rb;
        private BoxCollider2D[] _bcs;
        private SpriteRenderer _sr;

        private bool _animatorHasSpeedX;
        private bool _animatorHasDeath;

        // Start is called before the first frame update
        public void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _bcs = GetComponentsInChildren<BoxCollider2D>();
            _sr = GetComponent<SpriteRenderer>();

            _rb.velocity = new Vector2(Speed * StartingDirection, 0f);
            _health = StartingHealth;
            
            _getParameters();
        }

        // Update is called once per frame
        public void Update()
        {
            if (_rb.velocity.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
            if (_rb.velocity.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);

            var spriteSize = _sr.sprite.bounds.size;
            foreach (var bc in _bcs)
            {
                bc.size = spriteSize;
            }

            _updateAnimator();
        }

        #region Animation

        protected virtual void _updateAnimator()
        {
            if(_animatorHasSpeedX) _animator.SetFloat("SpeedX", Mathf.Abs(_rb.velocity.x));
        }

        protected virtual IEnumerator _die()
        {
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            foreach (var bc in _bcs) bc.enabled = false;

            if(_animatorHasDeath) _animator.SetTrigger("Death");
            yield return new WaitForSeconds(DeathAnimation?.length ?? 0f);
            Destroy(gameObject);
        }

        private void _getParameters()
        {
            foreach (var param in _animator.parameters)
            {
                switch (param.name)
                {
                    case "Death":
                        _animatorHasDeath = true;
                        break;
                    case "Attack":
                        //_animatorHasAttack = true;
                        break;
                    case "SpeedX":
                        _animatorHasSpeedX = true;
                        break;
                }
            }
        }
        #endregion
    }
}