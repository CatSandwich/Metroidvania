using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimator : MonoBehaviour
{
    public BoxCollider2D TriggerBC;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rb.velocity.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
        if (_rb.velocity.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);

        _bc.size = new Vector2(_sr.sprite.bounds.size.x, _sr.sprite.bounds.size.y);
        TriggerBC.size = new Vector2(_sr.sprite.bounds.size.x, _sr.sprite.bounds.size.y);

        _animator.SetFloat("SpeedX", Mathf.Abs(_rb.velocity.x));
    }
}
