using UnityEngine;

namespace Entity.Enemy.Fighter
{
    public class FighterAnimator : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rb;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            var s = transform.localScale;
            if (_rb.velocity.x < 0f) s.x = -1f;
            if (_rb.velocity.x < 0f) s.x = 1f;
            transform.localScale = s;
            _animator.SetFloat("SpeedX", Mathf.Abs(_rb.velocity.x));
        }
    }
}
